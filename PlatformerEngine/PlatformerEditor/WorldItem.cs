using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class WorldItem : UIElement
    {
        private static int worldItemCounter = 0;
        public TextElement Title;
        public WorldItemType ItemType;
        public WorldItem(PlatformerEditor game, WorldItemType type, Vector2 position, float layer) : base(game, position, type.Size, layer, "worlditem_" + (worldItemCounter++).ToString())
        {
            Position = position;
            ItemType = type;
            Title = new TextElement(game, position, ItemType.Size, 0.5f, Name + "_text", Color.Black, type.Name);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + ItemType.Size + offset, Color.White, Color.Black, 0.5f, 1);
            Title.Draw(spriteBatch, offset);
        }
    }
}
