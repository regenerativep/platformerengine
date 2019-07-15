using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlatformerEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PlatformerEditor : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public AssetManager Assets = new AssetManager(null);
        public Dictionary<string, UIElement> UIElements;
        public List<UIElement> DrawnUIElements;
        public Dictionary<Keys, char> KeyToCharMap;
        public Dictionary<Keys, char> KeyToShiftedCharMap;
        public Dictionary<int, WorldLayer> WorldLayers;
        public MouseState PreviousMouseState;
        public MouseState MouseState;
        public float ScrollMultiplier;
        public WorldLayerListElement WorldLayerListElement;
        public WorldItemListElement ObjectListElement;
        public WorldItemListElement TileListElement;
        public WorldItemType CurrentWorldItemType;
        public WorldLayer CurrentWorldLayer;
        public IInputable CurrentInput;
        public KeyboardState KeyboardState;
        public KeyboardState PreviousKeyboardState;
        private Keys[] lastPressedKeys;
        public bool InputShifted;
        private int lastScrollAmount;
        public PlatformerEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += (object s, PreparingDeviceSettingsEventArgs args) =>
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            lastPressedKeys = new Keys[0];
            IsMouseVisible = true;
            lastScrollAmount = 0;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            ScrollMultiplier = -16;
            CurrentInput = null;
            CurrentWorldItemType = null;
            CurrentWorldLayer = null;
            KeyToCharMap = new Dictionary<Keys, char>();
            KeyToShiftedCharMap = new Dictionary<Keys, char>();
            InputShifted = false;
            AddKeyToChar(Keys.D0, '0', ')');
            AddKeyToChar(Keys.D1, '1', '!');
            AddKeyToChar(Keys.D2, '2', '@');
            AddKeyToChar(Keys.D3, '3', '#');
            AddKeyToChar(Keys.D4, '4', '$');
            AddKeyToChar(Keys.D5, '5', '%');
            AddKeyToChar(Keys.D6, '6', '^');
            AddKeyToChar(Keys.D7, '7', '&');
            AddKeyToChar(Keys.D8, '8', '*');
            AddKeyToChar(Keys.D9, '9', '(');
            AddKeyToChar(Keys.OemPipe, '\\', '|');
            AddKeyToChar(Keys.OemQuestion, '/', '?');
            AddKeyToChar(Keys.OemMinus, '-', '_');
            AddKeyToChar(Keys.OemPlus, '=', '+');
            AddKeyToChar(Keys.OemComma, ',', '<');
            AddKeyToChar(Keys.OemPeriod, '.', '>');
            AddKeyToChar(Keys.OemQuotes, '\'', '"');
            AddKeyToChar(Keys.OemSemicolon, ';', ':');
            AddKeyToChar(Keys.OemOpenBrackets, '[', '{');
            AddKeyToChar(Keys.OemCloseBrackets, ']', '}');
            AddKeyToChar(Keys.OemTilde, '`', '~');
            AddKeyToChar(Keys.A, 'a', 'A');
            AddKeyToChar(Keys.B, 'b', 'B');
            AddKeyToChar(Keys.C, 'c', 'C');
            AddKeyToChar(Keys.D, 'd', 'D');
            AddKeyToChar(Keys.E, 'e', 'E');
            AddKeyToChar(Keys.F, 'f', 'F');
            AddKeyToChar(Keys.G, 'g', 'G');
            AddKeyToChar(Keys.H, 'h', 'H');
            AddKeyToChar(Keys.I, 'i', 'I');
            AddKeyToChar(Keys.J, 'j', 'J');
            AddKeyToChar(Keys.K, 'k', 'K');
            AddKeyToChar(Keys.L, 'l', 'L');
            AddKeyToChar(Keys.M, 'm', 'M');
            AddKeyToChar(Keys.N, 'n', 'N');
            AddKeyToChar(Keys.O, 'o', 'O');
            AddKeyToChar(Keys.P, 'p', 'P');
            AddKeyToChar(Keys.Q, 'q', 'Q');
            AddKeyToChar(Keys.R, 'r', 'R');
            AddKeyToChar(Keys.S, 's', 'S');
            AddKeyToChar(Keys.T, 't', 'T');
            AddKeyToChar(Keys.U, 'u', 'U');
            AddKeyToChar(Keys.V, 'v', 'V');
            AddKeyToChar(Keys.W, 'w', 'W');
            AddKeyToChar(Keys.X, 'x', 'X');
            AddKeyToChar(Keys.Y, 'y', 'Y');
            AddKeyToChar(Keys.Z, 'z', 'Z');
            UIElements = new Dictionary<string, UIElement>();
            DrawnUIElements = new List<UIElement>();
            WorldLayers = new Dictionary<int, WorldLayer>();
            
            DrawnUIElements.Add(new LevelElement(this, new Vector2(0, 0), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), 0.3f, "level"));
            WorldLayerListElement = new WorldLayerListElement(this, new Vector2(0, 0), new Vector2(128, 256), 0.4f, "list_layers");
            DrawnUIElements.Add(WorldLayerListElement);
            ObjectListElement = new WorldItemListElement(this, new Vector2(0, 0), new Vector2(128, 240), 0.4f, "list_objects");
            TileListElement = new WorldItemListElement(this, new Vector2(0, 0), new Vector2(128, 240), 0.4f, "list_tiles");
            TabbedElement worldItemTabs = new TabbedElement(this, new Vector2(0, 256), new Vector2(128, 256), 0.4f, "tabs_worlditems", 16);
            worldItemTabs.AddTab("objects", ObjectListElement, 64);
            worldItemTabs.AddTab("tiles", TileListElement, 64);
            DrawnUIElements.Add(worldItemTabs);
            TextInputElement filenameInputElement = new TextInputElement(this, new Vector2(0, 512), new Vector2(128, 24), 0.4f, "input_filename");
            ButtonElement loadButton = new ButtonElement(this, new Vector2(0, 536), new Vector2(48, 24), 0.4f, "button_load", "load");
            loadButton.Click = () =>
            {
                string filename = filenameInputElement.Text;
                LoadLevel(filename);
            };
            ButtonElement saveButton = new ButtonElement(this, new Vector2(48, 536), new Vector2(48, 24), 0.4f, "button_save", "save");
            saveButton.Click = () =>
            {
                string filename = filenameInputElement.Text;
                SaveLevel(filename);
            };
            DrawnUIElements.Add(filenameInputElement);
            DrawnUIElements.Add(loadButton);
            DrawnUIElements.Add(saveButton);

            LoadWorldItemTypes("types.json");

            base.Initialize();
        }
        public void AddWorldLayer(int layer)
        {
            AddWorldLayer(new WorldLayer(layer));
        }
        public void AddWorldLayer(WorldLayer worldLayer)
        {
            if(WorldLayers.ContainsKey(worldLayer.Layer))
            {
                System.Diagnostics.Debug.WriteLine("tried to add a layer that already exists");
                return;
            }
            WorldLayerListElement.AddLayer(worldLayer.Layer);
            WorldLayers[worldLayer.Layer] = worldLayer;
        }
        public WorldLayer GetWorldLayer(int layer)
        {
            if(WorldLayers.ContainsKey(layer))
            {
                return WorldLayers[layer];
            }
            return null;
        }
        public UIElement GetUIElement(string name)
        {
            if(UIElements.ContainsKey(name))
            {
                return UIElements[name];
            }
            return null;
        }
        public void RemoveUIElement(UIElement element)
        {
            string victim = null;
            foreach(KeyValuePair<string, UIElement> pair in UIElements)
            {
                if(pair.Value == element)
                {
                    victim = pair.Key;
                    break;
                }
            }
            if(victim != null)
            {
                UIElements.Remove(victim);
            }
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Content = Content;
            Assets.LoadFont("main", "mainFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || KeyboardState.IsKeyDown(Keys.Escape))
                Exit();
            //uielement updates
            foreach(UIElement elem in DrawnUIElements)
            {
                elem.Update();
            }
            //mouse clicks
            Vector2 mousePos = new Vector2(MouseState.X, MouseState.Y);
            if((MouseState.LeftPressed() && !PreviousMouseState.LeftPressed()) || (MouseState.RightPressed() && !PreviousMouseState.RightPressed()) || (MouseState.MiddlePressed() && !PreviousMouseState.MiddlePressed()))
            {
                CurrentInput = null;
                for (int i = DrawnUIElements.Count - 1; i >= 0; i--)
                {
                    UIElement elem = DrawnUIElements[i];
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
                        elem.MousePressed(MouseState, new Vector2(0, 0));
                        break;
                    }
                }
            }
            if((!MouseState.LeftPressed() && PreviousMouseState.LeftPressed()) || (!MouseState.RightPressed() && PreviousMouseState.RightPressed()) || (!MouseState.MiddlePressed() && PreviousMouseState.MiddlePressed()))
            {
                for (int i = DrawnUIElements.Count - 1; i >= 0; i--)
                {
                    UIElement elem = DrawnUIElements[i];
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
                        elem.MouseReleased(MouseState, new Vector2(0, 0));
                        break;
                    }
                }
            }
            //scrolling
            int scrollAmount = MouseState.ScrollWheelValue;
            float scrollValue = Math.Sign(lastScrollAmount - scrollAmount) * ScrollMultiplier;
            if (scrollValue != 0)
            {
                for(int i = DrawnUIElements.Count - 1; i >= 0; i--)
                {
                    UIElement elem = DrawnUIElements[i];
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
                        elem.Scroll(MouseState, scrollValue);
                        break;
                    }
                }
            }
            //text input
            List<Keys> newKeys = new List<Keys>();
            Keys[] pressedKeys = KeyboardState.GetPressedKeys();
            for(int i = 0; i < pressedKeys.Length; i++)
            {
                Keys currentKey = pressedKeys[i];
                bool foundKey = false;
                for(int j = 0; j < lastPressedKeys.Length; j++)
                {
                    Keys lastKey = pressedKeys[j];
                    if(lastKey.Equals(currentKey))
                    {
                        foundKey = true;
                        break;
                    }
                }
                if(!foundKey)
                {
                    //System.Diagnostics.Debug.WriteLine(currentKey);
                    if (currentKey == Keys.LeftShift || currentKey == Keys.RightShift)
                    {
                        InputShifted = true;
                    }
                    else
                    {
                        newKeys.Add(currentKey);
                    }
                }
            }
            //List<Keys> unpressedKeys = new List<Keys>();
            for(int i = 0; i < lastPressedKeys.Length; i++)
            {
                Keys currentLastPressedKey = lastPressedKeys[i];
                bool foundKey = false;
                for (int j = 0; j < pressedKeys.Length; j++)
                {
                    Keys pressedKey = pressedKeys[j];
                    if(currentLastPressedKey.Equals(pressedKey))
                    {
                        foundKey = true;
                        break;
                    }
                }
                if(!foundKey)
                {
                    if(currentLastPressedKey == Keys.LeftShift || currentLastPressedKey == Keys.RightShift)
                    {
                        InputShifted = false;
                    }/*
                    else
                    {
                        unpressedKeys.Add(currentLastPressedKey);
                    }*/
                }
            }
            lastPressedKeys = pressedKeys;
            if (CurrentInput != null)
            {
                for (int i = 0; i < newKeys.Count; i++)
                {
                    Keys key = newKeys[i];
                    char keyChar = KeyToChar(key, InputShifted);
                    char[] validKeys = CurrentInput.ValidKeys;
                    bool isValid = false;
                    for(int j = 0; j < validKeys.Length; j++)
                    {
                        if(validKeys[j].Equals(keyChar))
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (isValid)
                    {
                        CurrentInput.Text = CurrentInput.Text + keyChar;
                    }
                    else
                    {
                        if (key.Equals(Keys.Back))
                        {
                            if (CurrentInput.Text.Length > 0)
                            {
                                CurrentInput.Text = CurrentInput.Text.Substring(0, CurrentInput.Text.Length - 1);
                            }
                        }
                    }
                }
            }
            
            PreviousMouseState = MouseState;
            PreviousKeyboardState = KeyboardState;
            lastScrollAmount = scrollAmount;
            base.Update(gameTime);
        }
        public void AddKeyToChar(Keys key, char normal, char shift)
        {
            KeyToCharMap[key] = normal;
            KeyToShiftedCharMap[key] = shift;
        }
        public char KeyToChar(Keys key, bool shifted) //there oughta be a better way to do this
        {
            if(shifted)
            {
                if(KeyToShiftedCharMap.ContainsKey(key))
                {
                    return KeyToShiftedCharMap[key];
                }
            }
            else
            {
                if (KeyToCharMap.ContainsKey(key))
                {
                    return KeyToCharMap[key];
                }
            }
            return ' ';
        }
        public void LoadWorldItemTypes(string filename)
        {
            string json = File.ReadAllText(filename, Encoding.UTF8);
            JObject obj = JObject.Parse(json);
            JArray objectTypeArray = (JArray)obj.GetValue("objectTypes").ToObject(typeof(JArray));
            foreach(JToken assetToken in objectTypeArray)
            {
                JObject assetObject = (JObject)assetToken.ToObject(typeof(JObject));
                string name = (string)assetObject.GetValue("name").ToObject(typeof(string));
                string internalName = (string)assetObject.GetValue("internalName").ToObject(typeof(string));
                int width = (int)assetObject.GetValue("width").ToObject(typeof(int));
                int height = (int)assetObject.GetValue("height").ToObject(typeof(int));
                ObjectListElement.AddWorldItem(new WorldItemType(name, internalName, new Vector2(width, height), false));
            }
            JArray tileTypeArray = (JArray)obj.GetValue("tileTypes").ToObject(typeof(JArray));
            foreach (JToken assetToken in tileTypeArray)
            {
                JObject assetObject = (JObject)assetToken.ToObject(typeof(JObject));
                string name = (string)assetObject.GetValue("name").ToObject(typeof(string));
                string internalName = (string)assetObject.GetValue("internalName").ToObject(typeof(string));
                int width = (int)assetObject.GetValue("width").ToObject(typeof(int));
                int height = (int)assetObject.GetValue("height").ToObject(typeof(int));
                TileListElement.AddWorldItem(new WorldItemType(name, internalName, new Vector2(width, height), true));
            }
        }
        public void LoadLevel(string filename)
        {
            string json = File.ReadAllText(filename, Encoding.UTF8);
            JObject levelObject = JObject.Parse(json);
            WorldLayers.Clear();
            WorldLayerListElement.ClearLayers();
            CurrentWorldLayer = null;
            LevelElement levelElement = (LevelElement)GetUIElement("level");
            int levelWidth = (int)levelObject.GetValue("width").ToObject(typeof(int));
            int levelHeight = (int)levelObject.GetValue("height").ToObject(typeof(int));
            levelElement.LevelSize = new Vector2(levelWidth, levelHeight);
            JArray layerArray = (JArray)levelObject.GetValue("layers").ToObject(typeof(JArray));
            foreach(JToken token in layerArray)
            {
                JObject layerObject = (JObject)token.ToObject(typeof(JObject));
                int layer = (int)layerObject.GetValue("layer").ToObject(typeof(int));
                WorldLayer worldLayer = new WorldLayer(layer);
                JArray objectArray = (JArray)layerObject.GetValue("objects").ToObject(typeof(JArray));
                JArray tileArray = (JArray)layerObject.GetValue("tiles").ToObject(typeof(JArray));
                List<WorldItem> objectList = LoadWorldItemsFromJArray(objectArray, worldLayer.DrawLayer);
                List<WorldItem> tileList = LoadWorldItemsFromJArray(tileArray, worldLayer.DrawLayer);
                worldLayer.WorldItems.AddRange(objectList);
                worldLayer.WorldItems.AddRange(tileList);
                AddWorldLayer(worldLayer);
            }
        }
        public List<WorldItem> LoadWorldItemsFromJArray(JArray list, float drawLayer)
        {
            List<WorldItem> worldItemList = new List<WorldItem>();
            foreach(JToken token in list)
            {
                JObject itemObject = (JObject)token.ToObject(typeof(JObject));
                int x = (int)itemObject.GetValue("x").ToObject(typeof(int));
                int y = (int)itemObject.GetValue("y").ToObject(typeof(int));
                string internalName = (string)itemObject.GetValue("name").ToObject(typeof(string));
                WorldItem item = new WorldItem(this, GetWorldItemTypeFromName(internalName), new Vector2(x, y), drawLayer);
                worldItemList.Add(item);
            }
            return worldItemList;
        }
        public WorldItemType GetWorldItemTypeFromName(string internalName)
        {
            WorldItemType type = ObjectListElement.GetWorldItemTypeFromName(internalName);
            if(type != null)
            {
                return type;
            }
            type = TileListElement.GetWorldItemTypeFromName(internalName);
            return type;
        }
        public void SaveLevel(string filename)
        {
            LevelElement levelElement = (LevelElement)GetUIElement("level");
            JObject levelObject = new JObject();
            levelObject.Add("width", JToken.FromObject((int)levelElement.LevelSize.X));
            levelObject.Add("height", JToken.FromObject((int)levelElement.LevelSize.Y));
            JArray layerArray = new JArray();
            foreach(KeyValuePair<int, WorldLayer> pair in WorldLayers)
            {
                WorldLayer worldLayer = pair.Value;
                JObject layerObject = new JObject();
                layerObject.Add("layer", JToken.FromObject(worldLayer.Layer));
                JArray objectArray = new JArray();
                JArray tileArray = new JArray();
                for(int j = 0; j < worldLayer.WorldItems.Count; j++)
                {
                    WorldItem item = worldLayer.WorldItems[j];
                    JObject itemObject = new JObject();
                    itemObject.Add("name", JToken.FromObject(item.ItemType.InternalName));
                    itemObject.Add("x", JToken.FromObject((int)item.Position.X));
                    itemObject.Add("y", JToken.FromObject((int)item.Position.Y));
                    if(item.ItemType.IsTile)
                    {
                        tileArray.Add(JToken.FromObject(itemObject));
                    }
                    else
                    {
                        objectArray.Add(JToken.FromObject(itemObject));
                    }
                }
                layerObject.Add("objects", JToken.FromObject(objectArray));
                layerObject.Add("tiles", JToken.FromObject(tileArray));
                layerArray.Add(JToken.FromObject(layerObject));
            }
            levelObject.Add("layers", JToken.FromObject(layerArray));
            string json = levelObject.ToString();
            File.WriteAllText(filename, json);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            foreach (UIElement elem in DrawnUIElements)
            {
                elem.Draw(spriteBatch, new Vector2(0, 0));
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
