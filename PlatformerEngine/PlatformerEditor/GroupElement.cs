using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class GroupElement : UIElement
    {
        public List<UIElement> Elements;
        public Vector2 SoftOffset;
        public GroupElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            Elements = new List<UIElement>();
            SoftOffset = new Vector2(0, 0);
        }
        public override void Update()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Update();
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw(spriteBatch, Position + offset + SoftOffset);
            }
        }
        public override void MousePressed(MouseState mouseState)
        {
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - Position;
            for(int i = 0; i < Elements.Count; i++)
            {
                UIElement elem = Elements[i];
                if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                {
                    elem.MousePressed(mouseState);
                }
            }
        }
        public override void MouseReleased(MouseState mouseState)
        {
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - Position;
            for (int i = 0; i < Elements.Count; i++)
            {
                UIElement elem = Elements[i];
                if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                {
                    elem.MouseReleased(mouseState);
                }
            }
        }
        public override void Scroll(MouseState mouseState, float amount)
        {
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - Position;
            for (int i = 0; i < Elements.Count; i++)
            {
                UIElement elem = Elements[i];
                if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                {
                    elem.Scroll(mouseState, amount);
                }
            }
        }
    }
}
