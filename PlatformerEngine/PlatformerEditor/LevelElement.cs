using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;

namespace PlatformerEditor
{
    public class LevelElement : UIElement
    {
        public Vector2 SoftOffset;
        public Vector2 Snap;
        public Vector2 LevelSize;
        public bool PanIsPressed;
        public LevelElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            SoftOffset = new Vector2(0, 0);
            Snap = new Vector2(32, 32);
            LevelSize = new Vector2(512, 512);
            PanIsPressed = false;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            //draw grid
            Vector2 actualOffset = offset + SoftOffset + Position;
            for (int i = (int)actualOffset.X; i <= actualOffset.X + LevelSize.X; i += (int)Snap.X)
            {
                spriteBatch.DrawLine(new Vector2(i, actualOffset.Y), new Vector2(i, actualOffset.Y + LevelSize.Y), Color.Black);
            }
            for (int i = (int)actualOffset.Y; i <= actualOffset.Y + LevelSize.Y; i += (int)Snap.Y)
            {
                spriteBatch.DrawLine(new Vector2(actualOffset.X, i), new Vector2(actualOffset.X + LevelSize.X, i), Color.Black);
            }
            //draw layers
            for(int i = 0; i < Game.WorldLayers.Count; i++)
            {
                WorldLayer worldLayer = Game.WorldLayers[i];
                for(int j = 0; j < worldLayer.WorldItems.Count; j++)
                {
                    WorldItem item = worldLayer.WorldItems[i];
                    item.Draw(spriteBatch, actualOffset);
                }
            }
        }
        public override void Update()
        {
            if(PanIsPressed)
            {
                Vector2 mouseDifference = (Game.MouseState.Position - Game.PreviousMouseState.Position).ToVector2();
                SoftOffset += mouseDifference;
            }
            base.Update();
        }
        public override void MousePressed(MouseState mouseState)
        {
            if(mouseState.MiddlePressed())
            {
                PanIsPressed = true;
            }
            base.MousePressed(mouseState);
        }
        public override void MouseReleased(MouseState mouseState)
        {
            if (!mouseState.MiddlePressed())
            {
                PanIsPressed = false;
            }
            base.MousePressed(mouseState);
        }
    }
}
