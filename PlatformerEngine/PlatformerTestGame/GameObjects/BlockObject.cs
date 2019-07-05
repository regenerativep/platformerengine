using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PlatformerTestGame.GameObjects
{
    public class BlockObject : GameObject
    {
        public BlockObject(Room room, Vector2 position) : base(room, position, new Vector2(0, 0))
        {
            AssetManager.RequestTexture("block", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(64, 64);
            });
        }
    }
}
