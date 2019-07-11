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
            UIElements = new Dictionary<string, UIElement>();
            DrawnUIElements = new List<UIElement>();

            DrawnUIElements.Add(new GroupElement(this, new Vector2(0, 0), new Vector2(64, 64), 1f, "group_test"));
            GroupElement group = (GroupElement)GetUIElement("group_test");
            group.Elements.Add(new TextElement(this, new Vector2(0, 0), new Vector2(0, 0), 1f, "text_test", 12, Color.Black, "hey"));

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach(UIElement elem in DrawnUIElements)
            {
                elem.Update();
            }
            GetUIElement("group_test").Position.X += 1;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (UIElement elem in DrawnUIElements)
            {
                elem.Draw(spriteBatch, new Vector2(0, 0));
            }
            base.Draw(gameTime);
        }
    }
}
