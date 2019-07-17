using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.UserInterface
{
    public class TextElement : UIElement
    {
        public Color TextColor;
        public string Text;
        public bool ShowRectangle;
        public SpriteFont Font;
        public TextElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name, Color color, string text = "", string fontName = "main") : base(uiManager, position, size, layer, name)
        {
            TextColor = color;
            Text = text;
            UIManager.Assets.RequestFont(fontName, (font) =>
            {
                Font = font;
            });
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, Position + offset, TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer);
            }
            base.Draw(spriteBatch, offset);
        }
    }
}
