﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatformerEngine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.UserInterface
{
    public class CheckboxElement : UIElement
    {
        public bool Ticked;
        public Action<bool> Tick;
        public CheckboxElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name, bool ticked) : base(uiManager, position, size, layer, name)
        {
            Ticked = ticked;
        }
        public override void MousePressed(MouseState mouseState, Vector2 offset)
        {
            Ticked = !Ticked;
            Tick?.Invoke(Ticked);
            base.MousePressed(mouseState, offset);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Color backgroundColor = Color.White;
            if (Ticked) backgroundColor = Color.Black;
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + offset + Size, backgroundColor, Color.Black, Layer);
            base.Draw(spriteBatch, offset);
        }
    }
}
