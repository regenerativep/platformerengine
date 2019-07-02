using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PlatformerGame : Game
    {
        public GraphicsDeviceManager Graphics;
        private SpriteBatch spriteBatch;
        public bool IsRunning;

        public PlatformerGame()
        {
            IsRunning = true;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            ChangeResolution(1024, 768);
            base.Initialize();
        }
        public void SetFullscreen(bool full)
        {
            Graphics.IsFullScreen = full;
            Graphics.ApplyChanges();
        }
        public void ChangeResolution(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = 1024;
            Graphics.PreferredBackBufferHeight = 768;
            Graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        protected override void Dispose(bool disposing)
        {
            IsRunning = false;
            base.Dispose(disposing);
        }
    }
}
