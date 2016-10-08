using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace csharp_2048
{
    public enum ToDraw
    {
        Nothing,
        Win,
        Lose
    }
    /// <summary>
    /// Takes control of blocks and whole logic in game
    /// </summary>
    public class Board
    {
        public static readonly Point offset = new Point(18, 138);
        private bool lockKeyboard = false;
        private bool anythingHappened = false;
        private bool is2048Already = false;
        public ToDraw toDraw = ToDraw.Nothing;
        private int points = 0;
        List<Block> blocks = new List<Block>();
        private static readonly Keys[] arrows = { Keys.Up, Keys.Left, Keys.Right, Keys.Down };  
        public Board()
        {
            AddNew();
            AddNew();
        }
        public int GetPoints()
        {
            return points;
        }
        public void ControlKeyboard()
        {
            KeyboardState keyboard= Keyboard.GetState();
            if (keyboard.GetPressedKeys().Intersect(arrows).Any())
            {
                if (!lockKeyboard && Block.ticks==Block.maxTicks)
                {
                    MakeMove(keyboard.GetPressedKeys()[0]);
                    lockKeyboard = true;
                }
            }
            else
            {
                lockKeyboard = false;
            }
        }
        private void MakeMove(Keys key)
        {
            int[,] table = new int[4, 4];
            Dictionary<int, int> fused = new Dictionary<int, int>();
            anythingHappened = false;
            for (int x = 0; x < table.GetLength(0); x++)
            {
                for (int y = 0; y < table.GetLength(1); y++)
                {
                    table[x, y] = -1;
                }
            }
            int i = 0;
            foreach (var block in blocks)
            {
                switch (key)
                {
                    case Keys.Up:
                        table[block.from.X, block.from.Y] = i++;
                        break;
                    case Keys.Down:
                        table[block.from.X, 3 - block.from.Y] = i++;
                        break;
                    case Keys.Left:
                        table[block.from.Y, block.from.X] = i++;
                        break;
                    case Keys.Right:
                        table[3 - block.from.Y, 3 - block.from.X] = i++;
                        break;
                    default:
                        return;
                }
            }
            for (int x = 0; x < table.GetLength(0); x++)
            {
                bool happened;
                do
                {
                    happened = false;
                    for (int y = 1; y < table.GetLength(0); y++)
                    {
                        if (table[x, y - 1] != -1 && table[x, y] != -1 &&
                            blocks[table[x, y - 1]].GetValue() == blocks[table[x, y]].GetValue() &&
                            blocks[table[x, y]].state == BlockState.Nothing &&
                            blocks[table[x, y - 1]].state == BlockState.Nothing)
                        {
                            fused.Add(table[x, y - 1], table[x, y]);
                            blocks[table[x, y]].state = BlockState.Upgrade;
                            blocks[table[x, y - 1]].state = BlockState.Destroy;
                            table[x, y] = -1;
                            happened = true;
                            break;
                        }
                        if (table[x, y - 1] == -1 && table[x, y] != -1)
                        {
                            table[x, y - 1] = table[x, y];
                            table[x, y] = -1;
                            happened = true;
                            break;
                        }
                    }
                    anythingHappened = anythingHappened || happened;
                } while (happened);
            }
            for (int x = 0; x < table.GetLength(0); x++)
            {
                for (int y = 0; y < table.GetLength(1); y++)
                {
                    if (table[x, y] != -1)
                    {
                        switch (key)
                        {
                            case Keys.Up:
                                blocks[table[x, y]].to = new Point(x, y);
                                break;
                            case Keys.Down:
                                blocks[table[x, y]].to = new Point(x, 3 - y);
                                break;
                            case Keys.Left:
                                blocks[table[x, y]].to = new Point(y, x);
                                break;
                            case Keys.Right:
                                blocks[table[x, y]].to = new Point(3 - y, 3 - x);
                                break;
                        }
                    }
                }
            }
            foreach (KeyValuePair<int, int> pair in fused)
            {
                blocks[pair.Value].to = new Point(blocks[pair.Key].to.X, blocks[pair.Key].to.Y);
            }
            if (anythingHappened)
            {
                Block.ticks = 0;
            }
        }
        /// <summary>
        /// Finishes move made by MakeMove() - destroying and upgrading blocks
        /// </summary>
        private void FinishMove()
        {
            blocks.RemoveAll((Block x) => { return x.state == BlockState.Destroy; });
            blocks.Where(block => block.state == BlockState.Upgrade)
                  .ToList()
                  .ForEach(block => points+=block.Upgrade());
            if(!is2048Already && blocks.Any(block => block.GetValue() == 2048))
            {
                is2048Already = true;
                toDraw = ToDraw.Win;
            }
            CheckIfLose();
        }
        /// <summary>
        /// Adds new block in random free place in 4x4 grid
        /// </summary>
        private void AddNew()
        {
            SortedSet<int> available = new SortedSet<int>(Enumerable.Range(0, 16));
            foreach (var b in blocks)
            {
                available.Remove(b.to.X * 4 + b.to.Y);
            }
            var num = available.ElementAt(Game1.rnd.Next(0, available.Count()));
            blocks.Add(new Block(new Point(num / 4, num % 4)));
        }
        public void DrawBlocks()
        {
            blocks.Where(block => block.state == BlockState.Destroy)
                  .ToList()
                  .ForEach(block => block.Draw());
            blocks.Where(block => block.state != BlockState.Destroy)
                  .ToList()
                  .ForEach(block => block.Draw());
            if (Block.ticks < Block.maxTicks)
            {
                Block.ticks++;
            }
            else
            {
                blocks.ForEach(block => block.from = new Point(block.to.X, block.to.Y) );
                FinishMove();
                if (anythingHappened)
                {
                    anythingHappened = false;
                    AddNew();
                }
            }
        }
        public void Reset()
        {
            blocks.Clear();
            AddNew();
            AddNew();
            toDraw = ToDraw.Nothing;
            is2048Already = false;
            anythingHappened = false;
            points = 0;
        }
        private void CheckIfLose()
        {
            if (blocks.Count() == 16)
            {
                bool same = false;
                var table=new int[4, 4];
                foreach(var block in blocks)
                {
                    table[block.to.X, block.to.Y] = block.GetValue();
                }
                for(int x=0; x<4; x++)
                {
                    for(int y=0; y<3; y++)
                    {
                        same = same || (table[x, y] == table[x, y + 1]);
                    }
                }
                if (!same)
                {
                    for(int y=0; y<4; y++)
                    {
                        for(int x=0; x<3; x++)
                        {
                            same = same || (table[x, y] == table[x + 1, y]);
                        }
                    }
                }
                if (!same)
                {
                    toDraw = ToDraw.Lose;
                }
                
            }
        }
    }
}
