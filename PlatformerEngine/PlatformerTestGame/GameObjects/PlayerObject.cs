using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine.Physics;

namespace PlatformerTestGame.GameObjects
{
    public class PlayerObject : GameObject
    {
        public InputManager Input;
        public Vector2 Velocity;
        public MovingObject PhysicsObject;
        public PlayerObject(Room room, Vector2 position) : base(room, position)
        {
            PhysicsObject = new MovingObject(PhysicsSim.GenerateRectangleVertices(64, 64), Position);
            room.Physics.MovingObjects.Add(PhysicsObject);
            room.Physics.GameObjectLinks.Add(new GameObjectLink(this, PhysicsObject));
            Velocity = new Vector2(0, 0);
            Input = new InputManager();
            float speed = 4;
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.A, (pressed) =>
            {
                PhysicsObject.Velocity.X -= pressed ? speed : -speed;
            }));
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.D, (pressed) =>
            {
                PhysicsObject.Velocity.X += pressed ? speed : -speed;
            }));
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.W, (pressed) =>
            {
                PhysicsObject.Velocity.Y -= pressed ? speed : -speed;
            }));
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.S, (pressed) =>
            {
                PhysicsObject.Velocity.Y += pressed ? speed : -speed;
            }));
            room.Engine.Assets.RequestTexture("obj_block", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(64, 64);
                Sprite.Offset = Sprite.Size / 2;
            });
        }
        public override void Update()
        {
            Input.Update();
            Position += Velocity;
            base.Update();
        }
    }
}
