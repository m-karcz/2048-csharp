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
    public class Block
    {
        private static readonly Point textureSize = new Point(84, 84);
        private static readonly Point size = new Point(70, 70);
        private static readonly Point gap = new Point(10, 10);
        public static readonly int maxTicks = 10;
        private static Dictionary<int, Texture2D> textures;
        public static void setTextures(Dictionary<int, Texture2D> dict)
        {
            textures = dict;
        }
        public static int ticks = 5;
        internal Point from;
        internal Point to;
        internal BlockState state;
        private int value = 0;
        public int Upgrade()
        {
            value = value << 1;
            state = BlockState.Nothing;
            return value;
        }
        public int GetValue()
        {
            return value;
        }
        private Rectangle rect;
        public Rectangle GetRect()
        {
            rect.X = Board.offset.X + (from.X * (maxTicks - ticks) + to.X * ticks) * (size.X + gap.X) / maxTicks;
            rect.Y = Board.offset.Y + (from.Y * (maxTicks - ticks) + to.Y * ticks) * (size.Y + gap.Y) / maxTicks;
            return rect;
        }
        public void Draw()
        {
            spriteBatch.Draw(textures[GetValue()], GetRect(), Color.White);
        }
        public static SpriteBatch spriteBatch;
        public Block(Point point)
        {
            value = 2;
            from = new Point(point.X, point.Y);
            to = new Point(point.X, point.Y);
            rect = new Rectangle(Board.offset, Block.textureSize);
            state = BlockState.Nothing;
            GetRect();
        }
    }
}
