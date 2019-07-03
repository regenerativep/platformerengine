using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// wrapper for easily separating layers of drawing
    /// </summary>
    public class DrawLayer
    {
        /// <summary>
        /// the layer (0 -> 100) to draw at
        /// </summary>
        public int Layer
        {
            get
            {
                return wholeLayer;
            }
            set
            {
                wholeLayer = value;
                ActualLayer = value / 100f;
            }
        }
        private int wholeLayer;
        public float ActualLayer { get; private set; }
        /// <summary>
        /// creates a new draw layer
        /// </summary>
        /// <param name="layer">the layer</param>
        public DrawLayer(int layer)
        {
            Layer = layer;
        }
        /// <summary>
        /// gets a whole layer at 100
        /// </summary>
        public static DrawLayer Default
        {
            get
            {
                return new DrawLayer(100);
            }
        }
    }
}
