using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerEditor
{
    public class TabbedElement : GroupElement
    {
        public HorizontalListElement TabButtonList;
        public Dictionary<string, KeyValuePair<ButtonElement, UIElement>> Tabs;
        public GroupElement CurrentTabContainer;
        public TabbedElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name, int buttonSectionHeight) : base(game, position, size, layer, name)
        {
            TabButtonList = new HorizontalListElement(Game, new Vector2(0, 0), new Vector2(Size.X, buttonSectionHeight), layer, Name + "_buttonlist");
            Tabs = new Dictionary<string, KeyValuePair<ButtonElement, UIElement>>();
            CurrentTabContainer = new GroupElement(Game, new Vector2(0, buttonSectionHeight), new Vector2(Size.X, Size.Y - buttonSectionHeight), layer, Name + "_tabcontainer");
            Elements.Add(TabButtonList);
            Elements.Add(CurrentTabContainer);
        }
        public void AddTab(string name, GroupElement tabContent, int buttonWidth)
        {
            ButtonElement tabButton = new ButtonElement(Game, new Vector2(0, 0), new Vector2(buttonWidth, TabButtonList.Size.Y), Layer + 0.01f, Name + "_" + name + "_button", name);
            tabButton.Click = () =>
            {
                SetTab(name);
            };
            Tabs.Add(name, new KeyValuePair<ButtonElement, UIElement>(tabButton, tabContent));
            TabButtonList.AddItem(tabButton);
        }
        public void SetTab(string name)
        {
            UIElement content = GetTabContent(name);
            CurrentTabContainer.Elements.Clear();
            if (content != null)
            {
                CurrentTabContainer.Elements.Add(content);
            }
        }
        public UIElement GetTabContent(string name)
        {
            if(Tabs.ContainsKey(name))
            {
                return Tabs[name].Value;
            }
            return null;
        }
        public override void Destroy(bool hardDestroy = false)
        {
            if (CurrentTabContainer != null)
            {
                CurrentTabContainer.Destroy(hardDestroy);
            }
            TabButtonList.Destroy(hardDestroy);
            base.Destroy();
        }
    }
}
