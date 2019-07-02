using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    public static class ConsoleManager
    {
        public static int CurrentWriteLine = 0;
        /// <summary>
        /// ms between each time we check for a keystroke
        /// </summary>
        public static int KeyInputCheckRate = 16;
        public static string CurrentInput = "";
        public static List<ICommand> Commands = new List<ICommand>();
        public static void WriteLine(string text)
        {
            CurrentWriteLine++;
            Console.SetCursorPosition(0, CurrentWriteLine);
            WriteClear();
            Console.SetCursorPosition(0, CurrentWriteLine);
            Console.WriteLine(text);
            WriteInput();
        }
        private static void WriteInput()
        {
            Console.SetCursorPosition(0, CurrentWriteLine + 1);
            WriteClear();
            Console.SetCursorPosition(0, CurrentWriteLine + 1);
            Console.WriteLine(">" + CurrentInput);
        }
        private static void WriteClear()
        {
            string clearStr = "";
            for(int i = 0; i < Console.BufferWidth; i++)
            {
                clearStr += " ";
            }
            Console.WriteLine(clearStr);
        }
        public static void Start()
        {
            Task.Run(() =>
            {
                WriteLine("started");
                while (Program.Game.IsRunning)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if(key.Key == ConsoleKey.Enter)
                    {
                        WriteLine(">" + CurrentInput);
                        DoCommand(CurrentInput);
                        CurrentInput = "";
                        WriteInput();
                    }
                    else if(key.Key == ConsoleKey.Backspace)
                    {
                        CurrentInput = CurrentInput.Substring(0, CurrentInput.Length - 1);
                        WriteInput();
                    }
                    else
                    {
                        CurrentInput += key.KeyChar;
                        WriteInput();
                    }
                    Thread.Sleep(KeyInputCheckRate);
                }
                WriteLine("closing");
            });
        }
        public static void DoCommand(string input)
        {
            string[] parts = input.Split(' ');
            if (parts.Length == 0) return;
            foreach(ICommand command in Commands)
            {
                if (command.CallCommand.Equals(parts[0]))
                {
                    command.Invoke(parts);
                    return;
                }
            }
            WriteLine("failed to find commmand \"" + parts[0] + "\"");
        }
    }
}
