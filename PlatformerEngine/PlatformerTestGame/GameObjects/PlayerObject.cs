using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PlatformerTestGame.GameObjects
{
    public class PlayerObject : GameObject
    {
        public InputManager Input;
        public Vector2 Velocity;
        public PlayerObject(Room room, Vector2 position) : base(room, position)
        {
            Velocity = new Vector2(0, 0);
            Input = new InputManager();
            float speed = 4;
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.A, (pressed) =>
            {
                Velocity.X -= pressed ? speed : -speed;
            }));
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.D, (pressed) =>
            {
                Velocity.X += pressed ? speed : -speed;
            }));
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.W, (pressed) =>
            {
                Velocity.Y -= pressed ? speed : -speed;
            }));
            Input.KeyTriggerList.Add(new KeyInputTrigger(Keys.S, (pressed) =>
            {
                Velocity.Y += pressed ? speed : -speed;
            }));
            room.Engine.Assets.RequestTexture("obj_block", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(64, 64);
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
