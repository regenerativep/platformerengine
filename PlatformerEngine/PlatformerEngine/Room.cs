using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// contains information about a room, and everything in the room
    /// </summary>
    public class Room
    {
        /// <summary>
        /// list of game objects in the room
        /// </summary>
        public List<GameObject> GameObjectList;
        /// <summary>
        /// list of tiles in the room
        /// </summary>
        public List<GameTile> GameTileList;
        /// <summary>
        /// width of the room
        /// </summary>
        public int Width;
        /// <summary>
        /// height of the room
        /// </summary>
        public int Height;
        /// <summary>
        /// the parent game
        /// </summary>
        public Game Game;
        /// <summary>
        /// the view offset that corresponds to the view position
        /// </summary>
        public Vector2 ViewPosition;
        /// <summary>
        /// sound manager
        /// </summary>
        public SoundManager Sounds;
        /// <summary>
        /// creates an instance of a room
        /// </summary>
        /// <param name="game">the parent game</param>
        public Room(Game game)
        {
            Game = game;
            Sounds = new SoundManager();
            Width = 512;
            Height = 512;
            ViewPosition = new Vector2(0, 0);
            GameObjectList = new List<GameObject>();
            GameTileList = new List<GameTile>();
        }
        /// <summary>
        /// updates the room and everything inside it
        /// </summary>
        public void Update()
        {
            foreach(GameObject obj in GameObjectList)
            {
                obj.Update();
            }
            foreach(GameTile tle in GameTileList)
            {
                tle.Update();
            }
        }
        /// <summary>
        /// draws the room and everything inside it
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 ceiledOffset = new Vector2((float)(Math.Ceiling(Math.Abs(ViewPosition.X) * Math.Sign(ViewPosition.X))), (float)(Math.Ceiling(Math.Abs(ViewPosition.Y) * Math.Sign(ViewPosition.Y))));
            foreach (GameObject obj in GameObjectList)
            {
                obj.Draw(spriteBatch, ceiledOffset);
            }
            foreach (GameTile tle in GameTileList)
            {
                tle.Draw(spriteBatch, ceiledOffset);
            }
        }
        /// <summary>
        /// processes a command in running the room
        /// </summary>
        /// <param name="input">the inputted command</param>
        public void ProcessCommand(string input)
        {
            string[] parts = input.Split(' ');
            switch(parts[0])
            {
                case "width":
                    Width = int.Parse(parts[1]);
                    break;
                case "height":
                    Height = int.Parse(parts[1]);
                    break;
                case "createobject":
                    {
                        Type type = GameObject.GetObjectFromName(parts[1]);
                        Vector2 position = new Vector2(int.Parse(parts[2]), int.Parse(parts[3]));
                        GameObject obj = (GameObject)type.GetConstructor(new Type[] { typeof(Room), typeof(Vector2) }).Invoke(new object[] { this, position });
                        obj.Sprite.LayerData.Layer = int.Parse(parts[4]);
                        GameObjectList.Add(obj);
                        break;
                    }
                case "createtile":
                    {
                        Type type = GameTile.GetTileFromName(parts[1]);
                        Vector2 position = new Vector2(int.Parse(parts[2]), int.Parse(parts[3]));
                        GameTile obj = (GameTile)type.GetConstructor(new Type[] { typeof(Room), typeof(Vector2) }).Invoke(new object[] { this, position });
                        obj.Sprite.LayerData.Layer = int.Parse(parts[4]);
                        GameTileList.Add(obj);
                        break;
                    }
            }
        }
        /// <summary>
        /// checks for collision against all other tiles
        /// </summary>
        /// <param name="checkingTile">the tile making this check</param>
        /// <param name="checkPos">the position to check at</param>
        /// <param name="includeTypes">the valid types of tiles to check</param>
        /// <returns>if there is a collision at the given position</returns>
        public bool CheckTileCollision(GameTile checkingTile, Vector2 checkPos, params Type[] includeTypes)
        {
            Rectangle checkingRect = new Rectangle((int)checkingTile.Position.X, (int)checkingTile.Position.Y, (int)checkingTile.Sprite.Size.X, (int)checkingTile.Sprite.Size.Y);
            for (int i = 0; i < GameTileList.Count; i++)
            {
                GameTile tile = GameTileList[i];
                bool isValidTile = false;
                for (int j = 0; j < includeTypes.Length; j++)
                {
                    if (includeTypes[j] == tile.GetType())
                    {
                        isValidTile = true;
                        break;
                    }
                }
                if (!isValidTile || checkingTile == tile)
                {
                    continue;
                }
                Rectangle targetRect = new Rectangle((int)tile.Position.X, (int)tile.Position.Y, (int)tile.Sprite.Size.X, (int)tile.Sprite.Size.Y);
                if (PlatformerMath.RectangleInRectangle(checkingRect, targetRect))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// checks for a tile at the given position
        /// </summary>
        /// <param name="checkPos">the position to check at</param>
        /// <returns>if there is a tile at the given position</returns>
        public bool CheckTileAt(Vector2 checkPos)
        {
            for (int i = 0; i < GameTileList.Count; i++)
            {
                GameTile tile = GameTileList[i];
                if (checkPos.X >= tile.Position.X && checkPos.X < tile.Position.X + tile.Sprite.Size.X && checkPos.Y >= tile.Position.Y && checkPos.Y < tile.Position.Y + tile.Sprite.Size.Y)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// finds any object with the given object name
        /// </summary>
        /// <param name="name">the name to check for</param>
        /// <returns>an object with the given name, null if it could not be found</returns>
        public GameObject FindObject(string name)
        {
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                GameObject obj = GameObjectList[i];
                if (obj.GetType() == GameObject.GetObjectFromName(name))
                {
                    return obj;
                }
            }
            return null;
        }
        /// <summary>
        /// checks for a collision against a rectangle and all other objects of the given name
        /// </summary>
        /// <param name="collider">the rectangle to check</param>
        /// <param name="name">the name of the object to check against</param>
        /// <returns>the object we collide with</returns>
        public GameObject FindCollision(Rectangle collider, string name)
        {
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                GameObject obj = GameObjectList[i];
                if (obj.GetType() == GameObject.GetObjectFromName(name))
                {
                    if (PlatformerMath.RectangleInRectangle(collider, PlatformerMath.AddVectorToRect(new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Sprite.Size.X, (int)obj.Sprite.Size.Y), obj.Position)))
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// loads the room from a file
        /// </summary>
        /// <param name="filename">the filename with the room data</param>
        public void Load(string filename)
        {
            string[] lines = File.ReadAllLines(filename, Encoding.UTF8);
            for (int i = 0; i < lines.Length; i++)
            {
                ProcessCommand(lines[i]);
            }

        }
    }
}
