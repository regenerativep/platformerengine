using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// extensions for drawing from a spritebatch
    /// </summary>
    public static class DrawExtensions
    {
        /// <summary>
        /// a texture with a single white pixel
        /// </summary>
        private static Texture2D singlePixel = null;
        /// <summary>
        /// draws a line to the spritebatch
        /// </summary>
        /// <param name="sb">this spritebatch</param>
        /// <param name="a">beginning point of the line</param>
        /// <param name="b">ending point of the line</param>
        /// <param name="color">color of the line</param>
        /// <param name="depth">layer to draw the line at</param>
        public static void DrawLine(this SpriteBatch sb, Vector2 a, Vector2 b, Color color, DrawLayer layer)
        {
            if(singlePixel == null)
            {
                singlePixel = new Texture2D(sb.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                singlePixel.SetData(new Color[] { Color.White });
            }
            Vector2 dist = b - a;
            float angle = (float)Math.Atan2(dist.Y, dist.X);
            sb.Draw(singlePixel, a, null, color, angle, Vector2.Zero, new Vector2(dist.Length(), 1), SpriteEffects.None, layer.ActualLayer);
        }
        /// <summary>
        /// draws an X
        /// </summary>
        /// <param name="sb">this spritebatch</param>
        /// <param name="pos">the center location of the X</param>
        /// <param name="size">width of the X</param>
        public static void DrawX(this SpriteBatch sb, Vector2 pos, float size, DrawLayer layer)
        {
            Vector2 sizeVec = new Vector2(size / 2);
            DrawLine(sb, pos - sizeVec, pos + sizeVec, Color.Black, layer);
            sizeVec.Y = -sizeVec.Y;
            DrawLine(sb, pos - sizeVec, pos + sizeVec, Color.Black, layer);
        }
    }
}
