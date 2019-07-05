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
    /// a tile that can be put into the game world
    /// </summary>
    public abstract class GameTile
    {
        /// <summary>
        /// gets a game tile type from its corresponding string name
        /// </summary>
        public static Dictionary<string, Type> NameToType = new Dictionary<string, Type>();
        /// <summary>
        /// position of the tile
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// sprite of the tile
        /// </summary>
        public SpriteData Sprite;
        /// <summary>
        /// room with the tile
        /// </summary>
        public Room Room;
        /// <summary>
        /// initializes the tile
        /// </summary>
        /// <param name="room">the room with the tile</param>
        /// <param name="position">the position of the tile</param>
        public GameTile(Room room, Vector2 position)
        {
            Room = room;
            Position = position;
            Sprite = null;
        }
        /// <summary>
        /// updates the tile
        /// </summary>
        public void Update()
        {
            Sprite?.Update();
        }
        /// <summary>
        /// draws the tile
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        /// <param name="viewPosition">the view offset correpsonding to the view position</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Sprite?.Draw(spriteBatch, Position - viewPosition);
        }
        /// <summary>
        /// gets a tile object type given its corresponding name as a string
        /// </summary>
        /// <param name="name">name of the tile as a string</param>
        /// <returns>the corresonding tile object type</returns>
        public static Type GetTypeFromName(string name)
        {
            if (NameToType.ContainsKey(name))
            {
                return NameToType[name];
            }
            return null;
        }
    }
}
