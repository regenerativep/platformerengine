using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
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
        public bool ShowGrid;
        public MouseState PreviousMouseState;
        public MouseState MouseState;
        public float ScrollMultiplier;
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
            IsMouseVisible = true;
            ScrollMultiplier = -4;
            UIElements = new Dictionary<string, UIElement>();
            DrawnUIElements = new List<UIElement>();
            ButtonElement buttonElem = new ButtonElement(this, new Vector2(32), new Vector2(128, 64), 0.5f, "button_test", "test");
            buttonElem.Click = () =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
            };
            DrawnUIElements.Add(buttonElem);

            base.Initialize();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach(UIElement elem in DrawnUIElements)
            {
                elem.Update();
            }
            Vector2 mousePos = new Vector2(MouseState.X, MouseState.Y);
            if((MouseState.LeftPressed() && !PreviousMouseState.LeftPressed()) || (MouseState.RightPressed() && !PreviousMouseState.RightPressed()) || (MouseState.MiddlePressed() && !PreviousMouseState.MiddlePressed()))
            {
                foreach(UIElement elem in DrawnUIElements)
                {
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
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
            if(MouseState.ScrollWheelValue != 0)
            {
                float scrollValue = MouseState.ScrollWheelValue * ScrollMultiplier;
                foreach (UIElement elem in DrawnUIElements)
                {
                    if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                    {
                        elem.Scroll(MouseState, scrollValue);
                    }
                }
            }
            PreviousMouseState = MouseState;
            base.Update(gameTime);
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
