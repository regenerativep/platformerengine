using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
using PlatformerTestGame.GameObjects;

namespace PlatformerTestGame
{
    /// <summary>
    /// main area for the platformer game
    /// </summary>
    public class PlatformerGame : Game
    {
        /// <summary>
        /// game's graphics device manager
        /// </summary>
        public GraphicsDeviceManager Graphics;
        private PEngine engine;
        private SpriteBatch spriteBatch;
        /// <summary>
        /// creates a new instance of the platformer game
        /// </summary>
        public PlatformerGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        /// <summary>
        /// initializes the platformer game
        /// </summary>
        protected override void Initialize()
        {
            ChangeResolution(1024, 768);

            engine = new PEngine(this);
            PEngine.NameToType["obj_block"] = typeof(BlockObject); //if there is a better way to go about doing this please tell
            PEngine.NameToType["obj_player"] = typeof(PlayerObject);
            
            engine.LoadRoom("Levels\\justplayer.json");

            base.Initialize();
        }
        /// <summary>
        /// sets the game to a state of fullscreen
        /// </summary>
        /// <param name="full">whether to make fullscreen</param>
        public void SetFullscreen(bool full)
        {
            Graphics.IsFullScreen = full;
            Graphics.ApplyChanges();
        }
        /// <summary>
        /// changes the resolution of the game
        /// </summary>
        /// <param name="width">the new width</param>
        /// <param name="height">the new height</param>
        public void ChangeResolution(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
        }
        /// <summary>
        /// loads game content
        /// LoadContent will be called once per game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            engine.Assets.LoadAssetsFromFile("Data\\assets.json");
        }

        /// <summary>
        /// unloads game content
        /// UnloadContent will be called once per game
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// called every tick event
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                Exit();
            engine.Update();
            base.Update(gameTime);
        }
        
        /// <summary>
        /// called every frame draw event
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            engine.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
