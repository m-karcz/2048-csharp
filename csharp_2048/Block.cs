using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace csharp_2048
{
    public enum BlockState
    {
        Nothing,
        Upgrade,
        Destroy
    }
    public class Block : Game
    {
        public static readonly Point textureSize = new Point(84, 84);
        public static readonly Point size = new Point(70, 70);
        public static readonly Point gap = new Point(10, 10);
        public Point from;
        public Point to;
        public BlockState state = BlockState.Nothing;
        private int value = 0;
        public void Upgrade()
        {
            value = value << 1;
            state = BlockState.Nothing;
        }
        public int GetValue()
        {
            return value;
        }
        private Rectangle rect;
        public void Draw(Texture2D text, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(text, rect, Color.White);
            return;
        }
        public void Draw2(Texture2D text)
        {
            spriteBatch.Draw(text, rect, Color.White);
            return;
        }
        public Rectangle GetRect()
        {
            rect.X = Board.offset.X + to.X * (size.X+gap.X);
            rect.Y = Board.offset.Y + to.Y * (size.Y+gap.Y);
            return rect;
        }
        public void Draw3(Dictionary<int, Texture2D> textures)
        {
            spriteBatch.Draw(textures[GetValue()], GetRect(), Color.White);
           // Debug.WriteLine("POWINIENEM RYSOWAC");
        }
        public static SpriteBatch spriteBatch;
        public Block(int x, int y)
        {
            value = 2;
            rect = new Rectangle(Board.offset, Block.textureSize);
        }
        public Block(Point point)
        {
            value = 2;
            from = new Point(point.X, point.Y);
            to = new Point(point.X, point.Y);
            rect = new Rectangle(Board.offset, Block.textureSize);
            state = BlockState.Nothing;
            GetRect();
        }
        public static bool test(Block x)
        {
            Debug.WriteLine("testuje");
            return x.state == BlockState.Destroy;
        }
    }
}
