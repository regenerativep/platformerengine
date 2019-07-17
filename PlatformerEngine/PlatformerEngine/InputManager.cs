using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// manages input in an easier-to-use way
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// the keyboard state
        /// </summary>
        public KeyboardState KeyboardState;
        /// <summary>
        /// list of input triggers
        /// </summary>
        public List<InputTrigger> KeyTriggerList;
        /// <summary>
        /// creates a new instance of the input manager
        /// </summary>
        public InputManager()
        {
            KeyTriggerList = new List<InputTrigger>();
        }
        /// <summary>
        /// checks for changes in input
        /// </summary>
        public void Update()
        {
            KeyboardState = Keyboard.GetState();
            for(int i = 0; i < KeyTriggerList.Count; i++)
            {
                InputTrigger trig = KeyTriggerList[i];
                trig.Update(KeyboardState);
            }
        }
    }
}
