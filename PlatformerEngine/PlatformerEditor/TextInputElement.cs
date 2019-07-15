using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    class TextInputElement : ButtonElement, IInputable
    {
        public string Text
        {
            get
            {
                if(TextElement != null)
                {
                    return TextElement.Text;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (TextElement != null)
                {
                    TextElement.Text = value;
                }
            }
        }
        public char[] ValidKeys { get; set; }
        public TextInputElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name, "")
        {
            ValidKeys = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ./\\-_".ToCharArray();
            Click = () =>
            {
                Game.CurrentInput = this;
            };
        }
    }
}
