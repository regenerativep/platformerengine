﻿using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerTestGame.GameObjects
{
    public class GameWinObject : GameObject
    {
        public static float TextOpacityChange = 0.001f;
        public SpriteFont Font;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                if(Font != null)
                {
                    TextSize = Font.MeasureString(Text);
                }
            }
        }
        private string text;
        public Vector2 TextSize;
        public float TextOpacity;
        public Light Light;
        public GameWinObject(Room room, Vector2 position) : base(room, position)
        {
            Font = null;
            Text = PlatformerMath.Choose("Surely you must be cheating?", "I didn't expect that.", "Game over. For me. I guess.");
            TextOpacity = 0;
        }
        public override void Load(AssetManager assets)
        {
            assets.RequestFont("fnt_main", (font) =>
            {
                Font = font;
                Text = Text;
            });
            Vector2 viewSize = new Vector2(Room.Engine.Game.GraphicsDevice.Viewport.Width, Room.Engine.Game.GraphicsDevice.Viewport.Height);
            Light = new Light(viewSize / 2);
            assets.RequestTexture("lgt_circular", (tex) =>
            {
                Light.Sprite.Change(tex);
                Light.Sprite.Size = new Vector2(384, 256);
                Light.Sprite.Offset = -Light.Sprite.Size / 2;
            });
            Room.LightList.Add(Light);
            base.Load(assets);
        }
        public override void Update()
        {
            if(TextOpacity < 1)
            {
                TextOpacity += TextOpacityChange;
            }
            if(TextOpacity > 1)
            {
                TextOpacity = 1;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            if (!Room.LightList.Contains(Light))
            {
                Room.LightList.Add(Light);
            }
            Vector2 viewSize = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
            spriteBatch.DrawRectangle(new Vector2(0, 0), viewSize, Color.White, 0f);
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, (viewSize - TextSize) / 2, Color.Red * TextOpacity);
            }
            base.Draw(spriteBatch, viewPosition);
        }
    }
}
