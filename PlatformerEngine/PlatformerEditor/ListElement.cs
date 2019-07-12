using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class ListElement : HardGroupElement
    {
        public List<UIElement> Items;
        public ListElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            Items = new List<UIElement>();
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
                Elements.Add(item);
                nextY += (int)Math.Ceiling(item.Size.Y);
            }
        }
        public override void Scroll(MouseState mouseState, float amount)
        {
            SoftOffset.Y += amount;
        }
    }
}
