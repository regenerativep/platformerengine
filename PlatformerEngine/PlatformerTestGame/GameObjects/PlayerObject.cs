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
        public KeyInputTrigger Left, Right, Up, Down;
        public float Speed;
        public PlayerObject(Room room, Vector2 position) : base(room, position)
        {
            PhysicsObject = new MovingObject(PhysicsSim.GenerateRectangleVertices(32, 32), Position);
            room.Physics.MovingObjects.Add(PhysicsObject);
            room.Physics.GameObjectLinks.Add(new GameObjectLink(this, PhysicsObject));
            Velocity = new Vector2(0, 0);
            Input = new InputManager();
            Speed = 1f;
            Left = new KeyInputTrigger(Keys.A);
            Right = new KeyInputTrigger(Keys.D);
            Up = new KeyInputTrigger(Keys.W);
            Down = new KeyInputTrigger(Keys.S);
            Input.KeyTriggerList.AddRange(new KeyInputTrigger[] { Left, Right, Up, Down });
            room.Engine.Assets.RequestTexture("obj_block", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(32, 32);
                Sprite.Offset = Sprite.Size / 2;
            });
        }
        public override void Update()
        {
            Input.Update();
            if (Left.Pressed)
            {
                PhysicsObject.Velocity.X -= Speed;
            }
            if (Right.Pressed)
            {
                PhysicsObject.Velocity.X += Speed;
            }
            if (Up.Pressed)
            {
                PhysicsObject.Velocity.Y -= Speed;
            }
            if (Down.Pressed)
            {
                PhysicsObject.Velocity.Y += Speed;
            }
            Position += Velocity;
            base.Update();
        }
    }
}
