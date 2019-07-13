using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
using System;
using System.Collections.Generic;

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
        public Dictionary<int, WorldLayer> WorldLayers;
        public bool ShowGrid;
        public MouseState PreviousMouseState;
        public MouseState MouseState;
        public float ScrollMultiplier;
        public WorldLayerListElement WorldLayerListElement;
        public IInputable CurrentInput;
        public KeyboardState KeyboardState;
        public KeyboardState PreviousKeyboardState;
        private Keys[] lastPressedKeys;
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
            UIElements = new Dictionary<string, UIElement>();
            DrawnUIElements = new List<UIElement>();
            WorldLayers = new Dictionary<int, WorldLayer>();

            WorldLayerListElement = new WorldLayerListElement(this, new Vector2(0, 0), new Vector2(128, 256), 0.4f, "list_layers");
            DrawnUIElements.Add(WorldLayerListElement);

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
            WorldLayers.Add(worldLayer.Layer, worldLayer);
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
                foreach(UIElement elem in DrawnUIElements)
                {
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
                        CurrentInput = null;
                        elem.MousePressed(MouseState);
                    }
                }
            }
            if((!MouseState.LeftPressed() && PreviousMouseState.LeftPressed()) || (!MouseState.RightPressed() && PreviousMouseState.RightPressed()) || (!MouseState.MiddlePressed() && PreviousMouseState.MiddlePressed()))
            {
                foreach (UIElement elem in DrawnUIElements)
                {
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
                        elem.MouseReleased(MouseState);
                    }
                }
            }
            //scrolling
            int scrollAmount = MouseState.ScrollWheelValue;
            float scrollValue = Math.Sign(lastScrollAmount - scrollAmount) * ScrollMultiplier;
            if (scrollValue != 0)
            {
                foreach (UIElement elem in DrawnUIElements)
                {
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
                        elem.Scroll(MouseState, scrollValue);
                    }
                }
            }
            //text input
            if(CurrentInput != null)
            {
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
                        newKeys.Add(currentKey);
                    }
                }
                lastPressedKeys = pressedKeys;
                for(int i = 0; i < newKeys.Count; i++)
                {
                    Keys key = newKeys[i];
                    char keyChar = KeyToChar(key);
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
                    if(isValid)
                    {
                        CurrentInput.Text = CurrentInput.Text + keyChar;
                    }
                    else
                    {
                        if(key.Equals(Keys.Back))
                        {
                            if(CurrentInput.Text.Length > 0)
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
        public char KeyToChar(Keys key) //there oughta be a better way to do this
        {
            //TODO: add more keys
            switch(key)
            {
                case Keys.D0:
                    return '0';
                case Keys.D1:
                    return '1';
                case Keys.D2:
                    return '2';
                case Keys.D3:
                    return '3';
                case Keys.D4:
                    return '4';
                case Keys.D5:
                    return '5';
                case Keys.D6:
                    return '6';
                case Keys.D7:
                    return '7';
                case Keys.D8:
                    return '8';
                case Keys.D9:
                    return '9';
            }
            return ' ';
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (UIElement elem in DrawnUIElements)
            {
                elem.Draw(spriteBatch, new Vector2(0, 0));
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
