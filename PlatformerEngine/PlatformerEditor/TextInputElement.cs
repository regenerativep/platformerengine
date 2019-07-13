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
        public char[] ValidKeys
        {
            get
            {
                return "0123456789".ToCharArray();
            }
        }
        public TextInputElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name, "")
        {
            Click = () =>
            {
                Game.CurrentInput = this;
            };
        }
    }
}
