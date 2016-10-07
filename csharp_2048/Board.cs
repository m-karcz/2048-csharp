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
    public class Board
    {
        public static readonly Point offset = new Point(18, 138);
        private bool lockKeyboard = false;
        Keys[] arrows = { Keys.Up, Keys.Left, Keys.Right, Keys.Down };
        public Board()
        {
            AddNew();
            AddNew();
        }
        public void ControlKeyboard(KeyboardState keyboard)
        {
            if (keyboard.GetPressedKeys().Intersect(arrows).Any())
            {
                if (!lockKeyboard)
                {
                    Debug.WriteLine("DZIALA");
                    MakeMove(keyboard.GetPressedKeys()[0]);
                    lockKeyboard = true;
                }
            }
            else
            {
                lockKeyboard = false;
            }
        }
        List<Block> blocks = new List<Block>();
        public void MakeMove(Keys key)
        {
            //  AddNew();
            //AddNew();
            //AddNew();
            int[,] table = new int[4, 4];
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
            Dictionary<int, int> fused = new Dictionary<int, int>();
            bool happenedAnything = false;
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
                            blocks[table[x, y]].state = BlockState.Destroy;
                            blocks[table[x, y - 1]].state = BlockState.Upgrade;
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
                    happenedAnything = happenedAnything || happened;
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
            Clear();
            if (happenedAnything)
            {
                AddNew();
            }
        }
        private void Clear()
        {
            blocks.RemoveAll((Block x) => {return x.state == BlockState.Destroy; });
            for(int i=0; i<blocks.Count(); i++)
            {
                if(blocks[i].state == BlockState.Upgrade)
                {
                    blocks[i].Upgrade();
                }
            }
            foreach(var block in blocks)
            {
                block.from = new Point(block.to.X, block.to.Y);
            }
            
        }
        private void AddNew()
        {
            SortedSet<int> available = new SortedSet<int>(Enumerable.Range(0, 16));
            foreach (var b in blocks)
            {
                available.Remove(b.to.X * 4 + b.to.Y);
            }
            var num = available.ElementAt(Game1.rnd.Next(0, available.Count()));
            blocks.Add(new Block(new Point(num / 4, num % 4)));
            return;
        }
        public void drawBlocks(Dictionary<int, Texture2D> textures)
        {
            foreach (var block in blocks)
            {
                block.Draw3(textures);
            }
        }
    }
}
