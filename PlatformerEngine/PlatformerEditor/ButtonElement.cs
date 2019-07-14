using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;
using Microsoft.Xna.Framework.Input;

namespace PlatformerEditor
{
    public class ButtonElement : UIElement
    {
        public TextElement TextElement;
        public Vector2 TextPadding;
        public Action Click;
        public ButtonElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name, string text) : base(game, position, size, layer, name)
        {
            TextPadding = new Vector2(2, 2);
            TextElement = new TextElement(game, position + TextPadding, size - TextPadding, Layer + 0.01f, name + "_text", Color.Black, text);
            Click = null;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + Size + offset, Color.White, Color.Black, Layer);
            TextElement.Draw(spriteBatch, offset);
        }
        public override void MousePressed(MouseState mouseState)
        {
            Click?.Invoke();
            base.MousePressed(mouseState);
        }
    }
}
