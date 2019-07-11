using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class GroupElement : UIElement
    {
        public List<UIElement> Elements;
        public RenderTarget2D Graphics;
        public GroupElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            Elements = new List<UIElement>();
            Graphics = new RenderTarget2D(game.GraphicsDevice, (int)Size.X, (int)Size.Y);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.GraphicsDevice.SetRenderTarget(Graphics);
            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(Color.Wheat);
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw(spriteBatch, new Vector2(0, 0));
            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(Graphics, Position + offset, Color.White);
            spriteBatch.End();
            base.Draw(spriteBatch, offset);
        }
    }
}
