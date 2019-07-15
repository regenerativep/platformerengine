using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class WorldItemListElement : ListElement
    {
        public List<WorldItemType> WorldItemTypes;
        public WorldItemListElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            ButtonElement resetObject = new ButtonElement(Game, new Vector2(0, 0), new Vector2(64, 32), Layer + 0.01f, Name + "_none", "-none-");
            resetObject.Click = () =>
            {
                Game.CurrentWorldItemType = null;
            };
            AddItem(resetObject);
            WorldItemTypes = new List<WorldItemType>();
        }
        public void AddWorldItem(WorldItemType item)
        {
            GroupElement group = new GroupElement(Game, new Vector2(0, 0), new Vector2(128, 32), Layer + 0.01f, Name + "_" + item.Name);
            ButtonElement itemButton = new ButtonElement(Game, new Vector2(0, 0), new Vector2(96, 32), Layer + 0.01f, group.Name + "_button", item.Name);
            itemButton.Click = () =>
            {
                Game.CurrentWorldItemType = item;
            };
            group.Elements.Add(itemButton);
            WorldItemTypes.Add(item);
            AddItem(group);
        }
        public WorldItemType GetWorldItemTypeFromName(string internalName)
        {
            for (int i = 0; i < WorldItemTypes.Count; i++)
            {
                WorldItemType type = WorldItemTypes[i];
                if (type.InternalName.Equals(internalName))
                {
                    return type;
                }
            }
            return null;
        }
    }
}
