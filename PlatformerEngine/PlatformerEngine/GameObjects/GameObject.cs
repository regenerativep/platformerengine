using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.GameObjects
{
    /// <summary>
    /// a (non programming) object that can be put inside the game world
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// the position of the object
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// the velocity of the object
        /// </summary>
        public Vector2 Velocity;
        /// <summary>
        /// the sprite of the object
        /// </summary>
        public SpriteData Sprite;
        /// <summary>
        /// the room with the object
        /// </summary>
        public Room Room;
        /// <summary>
        /// initializes part of the game object
        /// </summary>
        /// <param name="room">the room with the object</param>
        /// <param name="position">the position of the object</param>
        /// <param name="velocity">the velocity of the object</param>
        public GameObject(Room room, Vector2 position, Vector2 velocity)
        {
            Room = room;
            Position = position;
            Velocity = velocity;
            Sprite = null;
        }
        /// <summary>
        /// updates this game object
        /// </summary>
        public virtual void Update()
        {
            Position += Velocity;
            Sprite?.Update();
        }
        /// <summary>
        /// draws this game object
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        /// <param name="viewPosition">the offset to match the view position</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Sprite?.Draw(spriteBatch, Position - viewPosition);
        }
        /// <summary>
        /// gets a game object type given its corresponding name as a string
        /// </summary>
        /// <param name="name">name of the object as a string</param>
        /// <returns>the corresponding game object type</returns>
        public static Type GetObjectFromName(string name)
        {
            switch(name)
            {
                case "":
                    return typeof(GameObject);
            }
            return null;
        }
    }
}
