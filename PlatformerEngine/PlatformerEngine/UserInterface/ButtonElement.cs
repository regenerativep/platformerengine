using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;
using Microsoft.Xna.Framework.Input;

namespace PlatformerEngine.UserInterface
{
    public class ButtonElement : UIElement
    {
        public TextElement TextElement;
        public Vector2 TextPadding;
        public Action Click;
        // it should be noted that the textelement requires a font
        public ButtonElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name, string text) : base(uiManager, position, size, layer, name)
        {
            TextPadding = new Vector2(2, 2);
            TextElement = new TextElement(UIManager, TextPadding, Size - TextPadding, Layer + 0.01f, name + "_text", Color.Black, text);
            Click = null;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + Size + offset, Color.White, Color.Black, Layer);
            TextElement.Draw(spriteBatch, Position + offset);
            base.Draw(spriteBatch, offset);
        }
        public override void MousePressed(MouseState mouseState, Vector2 offset)
        {
            Click?.Invoke();
            base.MousePressed(mouseState, offset);
        }
        public override void Destroy(bool hardDestroy = false)
        {
            TextElement.Destroy(hardDestroy);
            base.Destroy();
        }
    }
}
