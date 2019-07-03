using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    public class Room
    {
        public int Width;
        public int Height;
        public PlatformerGame Game;
        public Vector2 ViewPosition;
        public SoundManager Sounds;
        public Room(PlatformerGame game)
        {
            Game = game;
            Sounds = new SoundManager();
            Width = 512;
            Height = 512;
            ViewPosition = new Vector2(0, 0);
        }
    }
}
