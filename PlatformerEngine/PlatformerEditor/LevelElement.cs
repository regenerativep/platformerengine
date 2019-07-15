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
        public WorldItem CurrentWorldItem;
        public LevelElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            SoftOffset = new Vector2(0, 0);
            Snap = new Vector2(32, 32);
            LevelSize = new Vector2(512, 512);
            PanIsPressed = false;
            CurrentWorldItem = null;
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
            //for(int i = 0; i < Game.WorldLayers.Keys.Count; i++)
            foreach(KeyValuePair<int, WorldLayer> pair in Game.WorldLayers)
            {
                //WorldLayer worldLayer = Game.WorldLayers[Game.WorldLayers.Keys.];
                WorldLayer worldLayer = pair.Value;
                if (!worldLayer.IsVisible) continue;
                for(int j = 0; j < worldLayer.WorldItems.Count; j++)
                {
                    WorldItem item = worldLayer.WorldItems[j];
                    item.Draw(spriteBatch, actualOffset);
                }
            }
            //draw mouse item
            if (Game.CurrentWorldItemType == null && CurrentWorldItem != null)
            {
                CurrentWorldItem = null;
            }
            else if ((Game.CurrentWorldItemType != null && CurrentWorldItem == null) || (Game.CurrentWorldItemType != null && CurrentWorldItem.ItemType != Game.CurrentWorldItemType))
            {
                CurrentWorldItem = new WorldItem(Game, Game.CurrentWorldItemType, SnapPosition(GetMousePosition(offset)), 0.6f);
            }
            else if (CurrentWorldItem != null)
            {
                CurrentWorldItem.Position = SnapPosition(GetMousePosition(offset));
            }
            CurrentWorldItem?.Draw(spriteBatch, actualOffset);
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
        public override void MousePressed(MouseState mouseState, Vector2 offset)
        {
            if(mouseState.MiddlePressed())
            {
                PanIsPressed = true;
            }
            else if(mouseState.LeftPressed())
            {
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - offset - Position - SoftOffset;
                if (Game.CurrentWorldLayer != null)
                {
                    WorldItem item = new WorldItem(Game, Game.CurrentWorldItemType, SnapPosition(mousePos), Game.CurrentWorldLayer.DrawLayer);
                    Game.CurrentWorldLayer.WorldItems.Add(item);
                }
            }
            else if(mouseState.RightPressed())
            {
                if(Game.CurrentWorldLayer != null)
                {
                    Vector2 mousePos = GetMousePosition(offset);
                    for (int i = 0; i < Game.CurrentWorldLayer.WorldItems.Count; i++)
                    {
                        WorldItem item = Game.CurrentWorldLayer.WorldItems[i];
                        if (PlatformerMath.PointInRectangle(new Rectangle(item.Position.ToPoint(), item.Size.ToPoint()), mousePos))
                        {
                            Game.CurrentWorldLayer.WorldItems.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            base.MousePressed(mouseState, offset);
        }
        public Vector2 GetMousePosition(Vector2? offset = null)
        {
            return new Vector2(Game.MouseState.X, Game.MouseState.Y) - (offset ?? Vector2.Zero) - Position - SoftOffset;
        }
        public Vector2 SnapPosition(Vector2 position)
        {
            Vector2 newVec = (position / Snap).ToPoint().ToVector2() * Snap;
            if (Position.X < 0)
            {
                newVec.X -= Snap.X;
            }
            if (Position.Y < 0)
            {
                newVec.Y -= Snap.Y;
            }
            return newVec;
        }
        public override void MouseReleased(MouseState mouseState, Vector2 offset)
        {
            if (!mouseState.MiddlePressed())
            {
                PanIsPressed = false;
            }
            base.MouseReleased(mouseState, offset);
        }
    }
}
