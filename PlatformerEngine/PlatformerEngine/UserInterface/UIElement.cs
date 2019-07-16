using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    public abstract class UIElement
    {
        public Vector2 Position;
        public Vector2 Size;
        public float Layer;
        public Game Game;
        public string Name;
        public UIElement(Game game, Vector2 position, Vector2 size, float layer, string name)
        {
            Game = game;
            Position = position;
            Size = size;
            Layer = layer;
            Name = name;
            try
            {
                IElementable elementsGame = (IElementable)Game;
                elementsGame.UIElements.Add(name, this);
            }
            catch (InvalidCastException)
            {
                //
            }
        }
        public virtual void MousePressed(MouseState mouseState, Vector2 offset) { }
        public virtual void MouseReleased(MouseState mouseState, Vector2 offset) { }
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset) { }
        public virtual void Scroll(MouseState mouseState, float amount) { }
        public virtual void Destroy(bool hardDestroy = false)
        {
            try
            {
                IElementable elementsGame = (IElementable)Game;
                elementsGame.DestroyUIElement(this);
            }
            catch (InvalidCastException)
            {
                //
            }
            //
        }
    }
}
