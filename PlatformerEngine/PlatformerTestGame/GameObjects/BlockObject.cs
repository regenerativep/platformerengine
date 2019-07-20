using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PlatformerEngine.Physics;

namespace PlatformerTestGame.GameObjects
{
    public class BlockObject : GameObject
    {
        public BlockObject(Room room, Vector2 position) : base(room, position)
        {
            room.Physics.ImmobileObjects.Add(new ImmobileObject(PhysicsSim.GenerateRectangleVertices(64, 64), position));
            room.Engine.Assets.RequestTexture("obj_block", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(64, 64);
                Sprite.Offset = -Sprite.Size / 2;
            });
        }
    }
}
