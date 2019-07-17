﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// keeps track of the state of one key
    /// </summary>
    public class InputTrigger //TODO: change to an interface or something to allow for more input methods
    {
        /// <summary>
        /// the key
        /// </summary>
        public Keys Key;
        /// <summary>
        /// if the key is pressed
        /// </summary>
        public bool Pressed;
        private bool previousPressed;
        /// <summary>
        /// what to call when key is pressed
        /// </summary>
        public Action<bool> Callback;
        /// <summary>
        /// creates a new input trigger
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <param name="callback">called when key state changes</param>
        public InputTrigger(Keys key, Action<bool> callback = null)
        {
            Pressed = false;
            previousPressed = false;
            Key = key;
            Callback = callback; //TODO: name "Callback" to something else
        }
        /// <summary>
        /// check for any updates in the key
        /// </summary>
        /// <param name="keyState">the keyboard state to check from</param>
        public void Update(KeyboardState keyState)
        {
            if(keyState.IsKeyDown(Key))
            {
                Pressed = true;
            }
            else
            {
                Pressed = false;
            }
            if(previousPressed != Pressed)
            {
                Callback?.Invoke(Pressed);
            }
            previousPressed = Pressed;
        }
    }
}
