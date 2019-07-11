using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEditor
{
    public class TextElement : UIElement
    {
        public Color TextColor;
        public string Text;
        public bool ShowRectangle;
        public int TextSize;
        public SpriteFont Font;
        public TextElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name, int textSize, Color color, string text = "") : base(game, position, size, layer, name)
        {
            TextSize = textSize;
            TextColor = color;
            Text = text;
            Game.Assets.RequestFont("main", (font) =>
            {
                Font = font;
            });
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, Position + offset, TextColor);
            }
            base.Draw(spriteBatch, offset);
        }
    }
}
