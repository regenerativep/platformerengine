using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// performs and contains information for transitions
    /// </summary>
    public abstract class Transition
    {
        /// <summary>
        /// 0 - 1, how far along are we
        /// </summary>
        public abstract float Progress { get; }
        /// <summary>
        /// start performing a transition
        /// </summary>
        /// <param name="isOpening">false if ending a room, true if opening a room</param>
        public abstract void Perform(bool isOpening);
        /// <summary>
        /// updates the transition
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// draws the current state of the transition
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
