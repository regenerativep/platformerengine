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
        public TextElement(Game game, Vector2 position, Vector2 size, float layer, string name, Color color, string text = "") : base(game, position, size, layer, name)
        {
            TextColor = color;
            Text = text;
            try
            {
                IAssettable assetsGame = (IAssettable)game;
                assetsGame.Assets.RequestFont("main", (font) =>
                {
                    Font = font;
                });
            }
            catch(InvalidCastException)
            {
                //
            }
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
