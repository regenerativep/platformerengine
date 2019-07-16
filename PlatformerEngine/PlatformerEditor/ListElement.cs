using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;

namespace PlatformerEditor
{
    public class ListElement : HardGroupElement
    {
        public List<UIElement> Items;
        public float MaxScroll;
        public ListElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            Items = new List<UIElement>();
            MaxScroll = 0;
        }
        public void AddItem(UIElement item, int ind = -1)
        {
            if(ind < 0)
            {
                Items.Add(item);
            }
            else
            {
                Items.Insert(ind, item);
            }
            UpdateList();
        }
        public void RemoveItem(UIElement item)
        {
            for(int i = Items.Count - 1; i >= 0; i--)
            {
                if(Items[i] == item)
                {
                    Items.RemoveAt(i);
                    break;
                }
            }
            UpdateList();
        }
        public void RemoveItem(int ind)
        {
            Items.RemoveAt(ind);
            UpdateList();
        }
        public void UpdateList()
        {
            Elements.Clear();
            int nextY = 0;
            for(int i = 0; i < Items.Count; i++)
            {
                UIElement item = Items[i];
                item.Position.Y = nextY;
                nextY += (int)Math.Ceiling(item.Size.Y);
                Elements.Add(item);
            }
            MaxScroll = Math.Max(nextY - Size.Y, 0);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + Size + offset, Color.White, Color.Black, Layer);
            base.Draw(spriteBatch, offset);
        }
        public override void Scroll(MouseState mouseState, float amount)
        {
            SoftOffset.Y += amount;
            if (SoftOffset.Y < -MaxScroll) SoftOffset.Y = -MaxScroll;
            if (SoftOffset.Y > 0) SoftOffset.Y = 0;
            base.Scroll(mouseState, amount);
        }
    }
}
