using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private SpriteBatch spriteBatch;
        /// <summary>
        /// if this game is currently running
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// creates a new instance of the platformer game
        /// </summary>
        public PlatformerGame()
        {
            IsRunning = true;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        /// <summary>
        /// initializes the platformer game
        /// </summary>
        protected override void Initialize()
        {
            ChangeResolution(1024, 768);
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
            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        
        /// <summary>
        /// called every frame draw event
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        /// <summary>
        /// called when this is being disposed of
        /// </summary>
        /// <param name="disposing">(idk what it is but i dont use it here)</param>
        protected override void Dispose(bool disposing)
        {
            IsRunning = false;
            base.Dispose(disposing);
        }
    }
}
