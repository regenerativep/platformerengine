using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// contains general math things for the engine
    /// </summary>
    public static class PlatformerMath
    {
        /// <summary>
        /// checks if the given rectangles intersect
        /// got from https://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other
        /// </summary>
        /// <param name="a">a rectangle</param>
        /// <param name="b">a rectangle</param>
        /// <returns>if the given rectangle intersects the other given rectangle</returns>
        public static bool RectangleInRectangle(Rectangle a, Rectangle b)
        {
            return a.X < b.X + b.Width && a.X + a.Width > b.X && a.Y < b.Y + b.Height && a.Y + a.Height > b.Y; //flipped comparisons for y since original code is for cartesian coords
        }
        /// <summary>
        /// adds (a) vector(s) to the position of a rectangle
        /// </summary>
        /// <param name="rect">the rectangle</param>
        /// <param name="vecs">the vectors to add</param>
        /// <returns>the new rectangle with the added vectors</returns>
        public static Rectangle AddVectorToRect(Rectangle rect, params Vector2[] vecs)
        {
            Rectangle newRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            foreach (Vector2 vec in vecs)
            {
                newRect.X += (int)vec.X;
                newRect.Y += (int)vec.Y;
            }
            return newRect;
        }
    }
}
