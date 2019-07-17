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
    public class UIManager
    {
        public AssetManager Assets;
        public Dictionary<string, UIElement> Elements;
        public IInputable CurrentInput;
        public float ScrollMultiplier;
        public MouseState PreviousMouseState;
        public MouseState MouseState;
        public GroupElement TopUINode;
        public KeyboardState KeyboardState;
        public KeyboardState PreviousKeyboardState;
        private Keys[] lastPressedKeys;
        public bool InputShifted;
        private int lastScrollAmount;
        public Dictionary<Keys, char> KeyToCharMap;
        public Dictionary<Keys, char> KeyToShiftedCharMap;
        public Game Game;
        public UIManager(Game game, AssetManager assetManager)
        {
            Game = game;
            Assets = assetManager;
            Elements = new Dictionary<string, UIElement>();
            CurrentInput = null;
            ScrollMultiplier = -16;
            lastPressedKeys = new Keys[0];
            lastScrollAmount = 0;
            KeyToCharMap = new Dictionary<Keys, char>();
            KeyToShiftedCharMap = new Dictionary<Keys, char>();
            InputShifted = false;
            TopUINode = new GroupElement(this, new Vector2(0, 0), new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), 0f, "top");
            AddKeyToChar(Keys.D0, '0', ')');
            AddKeyToChar(Keys.D1, '1', '!');
            AddKeyToChar(Keys.D2, '2', '@');
            AddKeyToChar(Keys.D3, '3', '#');
            AddKeyToChar(Keys.D4, '4', '$');
            AddKeyToChar(Keys.D5, '5', '%');
            AddKeyToChar(Keys.D6, '6', '^');
            AddKeyToChar(Keys.D7, '7', '&');
            AddKeyToChar(Keys.D8, '8', '*');
            AddKeyToChar(Keys.D9, '9', '(');
            AddKeyToChar(Keys.OemPipe, '\\', '|');
            AddKeyToChar(Keys.OemQuestion, '/', '?');
            AddKeyToChar(Keys.OemMinus, '-', '_');
            AddKeyToChar(Keys.OemPlus, '=', '+');
            AddKeyToChar(Keys.OemComma, ',', '<');
            AddKeyToChar(Keys.OemPeriod, '.', '>');
            AddKeyToChar(Keys.OemQuotes, '\'', '"');
            AddKeyToChar(Keys.OemSemicolon, ';', ':');
            AddKeyToChar(Keys.OemOpenBrackets, '[', '{');
            AddKeyToChar(Keys.OemCloseBrackets, ']', '}');
            AddKeyToChar(Keys.OemTilde, '`', '~');
            AddKeyToChar(Keys.A, 'a', 'A');
            AddKeyToChar(Keys.B, 'b', 'B');
            AddKeyToChar(Keys.C, 'c', 'C');
            AddKeyToChar(Keys.D, 'd', 'D');
            AddKeyToChar(Keys.E, 'e', 'E');
            AddKeyToChar(Keys.F, 'f', 'F');
            AddKeyToChar(Keys.G, 'g', 'G');
            AddKeyToChar(Keys.H, 'h', 'H');
            AddKeyToChar(Keys.I, 'i', 'I');
            AddKeyToChar(Keys.J, 'j', 'J');
            AddKeyToChar(Keys.K, 'k', 'K');
            AddKeyToChar(Keys.L, 'l', 'L');
            AddKeyToChar(Keys.M, 'm', 'M');
            AddKeyToChar(Keys.N, 'n', 'N');
            AddKeyToChar(Keys.O, 'o', 'O');
            AddKeyToChar(Keys.P, 'p', 'P');
            AddKeyToChar(Keys.Q, 'q', 'Q');
            AddKeyToChar(Keys.R, 'r', 'R');
            AddKeyToChar(Keys.S, 's', 'S');
            AddKeyToChar(Keys.T, 't', 'T');
            AddKeyToChar(Keys.U, 'u', 'U');
            AddKeyToChar(Keys.V, 'v', 'V');
            AddKeyToChar(Keys.W, 'w', 'W');
            AddKeyToChar(Keys.X, 'x', 'X');
            AddKeyToChar(Keys.Y, 'y', 'Y');
            AddKeyToChar(Keys.Z, 'z', 'Z');
        }
        public UIElement GetUIElement(string name)
        {
            if (Elements.ContainsKey(name))
            {
                return Elements[name];
            }
            return null;
        }
        public void DestroyUIElement(UIElement element)
        {
            string victim = null;
            foreach (KeyValuePair<string, UIElement> pair in Elements)
            {
                if (pair.Value == element)
                {
                    victim = pair.Key;
                    break;
                }
            }
            if (victim != null)
            {
                Elements.Remove(victim);
            }
        }
        public void AddKeyToChar(Keys key, char normal, char shift)
        {
            KeyToCharMap[key] = normal;
            KeyToShiftedCharMap[key] = shift;
        }
        public char KeyToChar(Keys key, bool shifted) //there oughta be a better way to do this
        {
            if (shifted)
            {
                if (KeyToShiftedCharMap.ContainsKey(key))
                {
                    return KeyToShiftedCharMap[key];
                }
            }
            else
            {
                if (KeyToCharMap.ContainsKey(key))
                {
                    return KeyToCharMap[key];
                }
            }
            return ' ';
        }
        public List<T> FindChanges<T>(T[] a, T[] b)
        {
            List<T> changes = new List<T>();
            for (int i = 0; i < a.Length; i++)
            {
                T itemA = a[i];
                bool foundItem = false;
                for (int j = 0; j < b.Length; j++)
                {
                    T itemB = b[j];
                    if (itemA.Equals(itemB))
                    {
                        foundItem = true;
                        break;
                    }
                }
                if (!foundItem)
                {
                    changes.Add(itemA);
                }
            }
            return changes;
        }
        public void Update()
        {
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || KeyboardState.IsKeyDown(Keys.Escape))
                Game.Exit();
            TopUINode.Update();
            //mouse clicks
            Vector2 mousePos = new Vector2(MouseState.X, MouseState.Y);
            if ((MouseState.LeftPressed() && !PreviousMouseState.LeftPressed()) || (MouseState.RightPressed() && !PreviousMouseState.RightPressed()) || (MouseState.MiddlePressed() && !PreviousMouseState.MiddlePressed()))
            {
                CurrentInput = null;
                TopUINode.MousePressed(MouseState, new Vector2(0, 0));
            }
            if ((!MouseState.LeftPressed() && PreviousMouseState.LeftPressed()) || (!MouseState.RightPressed() && PreviousMouseState.RightPressed()) || (!MouseState.MiddlePressed() && PreviousMouseState.MiddlePressed()))
            {
                TopUINode.MouseReleased(MouseState, new Vector2(0, 0));
            }
            //scrolling
            int scrollAmount = MouseState.ScrollWheelValue;
            float scrollValue = Math.Sign(lastScrollAmount - scrollAmount) * ScrollMultiplier;
            if (scrollValue != 0)
            {
                TopUINode.Scroll(MouseState, scrollValue);
            }
            //text input
            Keys[] pressedKeys = KeyboardState.GetPressedKeys();
            List<Keys> newKeys = FindChanges(pressedKeys, lastPressedKeys);
            List<Keys> releasedKeys = FindChanges(lastPressedKeys, pressedKeys);
            if (newKeys.Contains(Keys.LeftShift) || newKeys.Contains(Keys.RightShift))
            {
                InputShifted = true;
            }
            else if (!releasedKeys.Contains(Keys.LeftShift) && !releasedKeys.Contains(Keys.RightShift))
            {
                InputShifted = false;
            }
            lastPressedKeys = pressedKeys;
            if (CurrentInput != null)
            {
                for (int i = 0; i < newKeys.Count; i++)
                {
                    Keys key = newKeys[i];
                    char keyChar = KeyToChar(key, InputShifted);
                    char[] validKeys = CurrentInput.ValidKeys;
                    bool isValid = false;
                    for (int j = 0; j < validKeys.Length; j++)
                    {
                        if (validKeys[j].Equals(keyChar))
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (isValid)
                    {
                        CurrentInput.Text = CurrentInput.Text + keyChar;
                    }
                    else
                    {
                        if (key.Equals(Keys.Back))
                        {
                            if (CurrentInput.Text.Length > 0)
                            {
                                CurrentInput.Text = CurrentInput.Text.Substring(0, CurrentInput.Text.Length - 1);
                            }
                        }
                    }
                }
            }

            PreviousMouseState = MouseState;
            PreviousKeyboardState = KeyboardState;
            lastScrollAmount = scrollAmount;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            TopUINode.Draw(spriteBatch, new Vector2(0, 0));
            spriteBatch.End();
        }
    }
}
