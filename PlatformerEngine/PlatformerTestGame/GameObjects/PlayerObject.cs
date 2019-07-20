using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine.Physics;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerTestGame.GameObjects
{
    public class PlayerObject : GameObject
    {
        public InputManager Input;
        public MovingObject PhysicsObject;
        public KeyInputTrigger Left, Right, Jump;
        public float Speed;
        public PlayerObject(Room room, Vector2 position) : base(room, position)
        {
            PhysicsObject = new MovingObject(PhysicsSim.GenerateRectangleVertices(64, 64), Position);
            room.Physics.MovingObjects.Add(PhysicsObject);
            room.Physics.GameObjectLinks.Add(new GameObjectLink(this, PhysicsObject));
            Input = new InputManager();
            Speed = 0.2f;
            Left = new KeyInputTrigger(Keys.A);
            Right = new KeyInputTrigger(Keys.D);
            Jump = new KeyInputTrigger(Keys.Space, (pressed) =>
            {
                if (pressed)
                {
                    PhysicsObject.Velocity.Y = -10f;
                }
            });
            Input.KeyTriggerList.AddRange(new KeyInputTrigger[] { Left, Right, Jump });
            room.Engine.Assets.RequestTexture("obj_block", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(64, 64);
                Sprite.Offset = -Sprite.Size / 2;
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
            base.Update();
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            base.Draw(spriteBatch, viewPosition - Sprite.Offset);
        }
    }
}
