using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PhysicsTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Texture2D singlePixel = null;
        PhysicsSim sim;
        static int updateRate = 1;
        int currentUpdatePosition;
        MovingObject currentSelected;
        MouseState mouseState;
        MouseState prevMouseState;

        public static Vector2 testA, testB;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            currentSelected = null;
            currentUpdatePosition = 0;
            sim = new PhysicsSim();
            MovingObject obj = new MovingObject(new Vector2[] { new Vector2(-8, -8), new Vector2(8, -8), new Vector2(8, 8), new Vector2(-8, 8) }, new Vector2(276, 270), 1);
            obj.Velocity = new Vector2(0, 2);
            sim.MovingObjects.Add(obj);
            ImmobileObject imObj = new ImmobileObject(new Vector2[] { new Vector2(-16, -8), new Vector2(0, -64), new Vector2(64, -48), new Vector2(128, -8), new Vector2(128, 128), new Vector2(-16, 128) }, new Vector2(256, 320));
            sim.ImmobileObjects.Add(imObj);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (singlePixel == null)
            {
                singlePixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                singlePixel.SetData(new Color[] { Color.White });
            }
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
            mouseState = Mouse.GetState();
            if (currentUpdatePosition == 0)
            {
                sim.Update();
                Vector2 mouseDifference = new Vector2(mouseState.X - prevMouseState.X, mouseState.Y - prevMouseState.Y);
                if(currentSelected != null)
                {
                    currentSelected.Velocity = mouseDifference / 16;
                    if(mouseState.LeftButton == ButtonState.Released)
                    {
                        currentSelected = null;
                    }
                }
                else
                {
                    if(mouseState.LeftButton == ButtonState.Pressed)
                    {
                        float? closestDistance = null;
                        for(int i = 0; i < sim.MovingObjects.Count; i++)
                        {
                            MovingObject obj = sim.MovingObjects[i];
                            float dist = (new Vector2(mouseState.X, mouseState.Y) - obj.Position).LengthSquared();
                            if(closestDistance == null || closestDistance > dist)
                            {
                                closestDistance = dist;
                                currentSelected = obj;
                            }
                        }
                    }
                }
                currentUpdatePosition = updateRate;
                prevMouseState = mouseState;
            }
            else
            {
                currentUpdatePosition--;
            }
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
            foreach (PhysicsObject obj in sim.MovingObjects)
            {
                obj.Draw(spriteBatch);
            }
            foreach (PhysicsObject obj in sim.ImmobileObjects)
            {
                obj.Draw(spriteBatch);
            }
            DrawX(spriteBatch, testA);
            DrawX(spriteBatch, testB, 8);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 a, Vector2 b, Color color, float depth = 1f)
        {
            float distX = b.X - a.X;
            float distY = b.Y - a.Y;
            float angle = (float)Math.Atan2(distY, distX);
            float dist = (float)Math.Sqrt(distX * distX + distY * distY);
            sb.Draw(singlePixel, a, null, color, angle, Vector2.Zero, new Vector2(dist, 1), SpriteEffects.None, depth);
        }
        public static void DrawX(SpriteBatch sb, Vector2 pos, float size = 4)
        {
            Vector2 sizeVec = new Vector2(size);
            DrawLine(sb, pos - sizeVec, pos + sizeVec, Color.Black);
            sizeVec.Y = -sizeVec.Y;
            DrawLine(sb, pos - sizeVec, pos + sizeVec, Color.Black);
        }
    }
}
