using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        /// the parent engine
        /// </summary>
        public PEngine Engine;
        /// <summary>
        /// the view offset that corresponds to the view position
        /// </summary>
        public Vector2 ViewPosition;
        /// <summary>
        /// sound manager
        /// </summary>
        public SoundManager Sounds;
        private ITransition currentTransition;
        /// <summary>
        /// creates an instance of a room
        /// </summary>
        /// <param name="engine">the parent engine</param>
        public Room(PEngine engine)
        {
            Engine = engine;
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
            if (currentTransition != null && currentTransition.IsActive)
            {
                currentTransition.Update();
                return;
            }
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
            if (currentTransition != null && currentTransition.IsActive)
            {
                currentTransition.Draw(spriteBatch);
            }
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
        /// applies a transition to this room
        /// </summary>
        /// <param name="trans">the transition to apply</param>
        public void ApplyTransition(ITransition trans)
        {
            currentTransition = trans;
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
                if (obj.GetType() == PEngine.GetTypeFromName(name))
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
                if (obj.GetType() == PEngine.GetTypeFromName(name))
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
        public Room Load(string filename)
        {
            string[] lines = File.ReadAllLines(filename, Encoding.UTF8);
            string json = "";
            for (int i = 0; i < lines.Length; i++)
            {
                json += lines[i] + "\n";
            }
            JObject obj = JObject.Parse(json);
            int width = (int)obj.GetValue("width").ToObject(typeof(int));
            int height = (int)obj.GetValue("height").ToObject(typeof(int));
            JArray layerArray = (JArray)obj.GetValue("layers").ToObject(typeof(JArray));
            foreach(JToken item in layerArray)
            {
                JObject layerObject = (JObject)item.ToObject(typeof(JObject));
                int layer = (int)layerObject.GetValue("layer").ToObject(typeof(int));
                JArray objectArray = (JArray)layerObject.GetValue("objects").ToObject(typeof(JArray));
                foreach (JToken gameObjectToken in objectArray)
                {
                    JObject gameObjectData = (JObject)gameObjectToken.ToObject(typeof(JObject));
                    string internalName = (string)gameObjectData.GetValue("name").ToObject(typeof(string));
                    Type type = PEngine.GetTypeFromName(internalName);
                    if (type == null)
                    {
                        ConsoleManager.WriteLine("could not find object name \"" + internalName + "\"", "err");
                        continue;
                    }
                    Vector2 position = new Vector2((int)gameObjectData.GetValue("x").ToObject(typeof(int)), (int)gameObjectData.GetValue("y").ToObject(typeof(int)));
                    GameObject gameObject = (GameObject)type.GetConstructor(new Type[] { typeof(Room), typeof(Vector2) }).Invoke(new object[] { this, position });
                    gameObject.Sprite.LayerData.Layer = layer;
                    GameObjectList.Add(gameObject);
                }
                JArray tileArray = (JArray)layerObject.GetValue("tiles").ToObject(typeof(JArray));
                foreach (JToken tileToken in tileArray)
                {
                    JObject tileData = (JObject)tileToken.ToObject(typeof(JObject));
                    string internalName = (string)tileData.GetValue("name").ToObject(typeof(string));
                    Type type = PEngine.GetTypeFromName(internalName);
                    if (type == null)
                    {
                        ConsoleManager.WriteLine("could not find tile name \"" + internalName + "\"", "err");
                        continue;
                    }
                    Vector2 position = new Vector2((int)tileData.GetValue("x").ToObject(typeof(int)), (int)tileData.GetValue("y").ToObject(typeof(int)));
                    GameTile tile = (GameTile)type.GetConstructor(new Type[] { typeof(Room), typeof(Vector2) }).Invoke(new object[] { this, position });
                    tile.Sprite.LayerData.Layer = layer;
                    GameTileList.Add(tile);
                }
            }
            return this;
        }
    }
}
