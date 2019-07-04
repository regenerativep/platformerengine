using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// manages console input and output
    /// </summary>
    public static class ConsoleManager
    {
        /// <summary>
        /// the current line to write new console information to
        /// </summary>
        public static int CurrentWriteLine = 0;
        /// <summary>
        /// ms between each time we check for a keystroke
        /// </summary>
        public static int KeyInputCheckRate = 16;
        /// <summary>
        /// the current text input from the user
        /// </summary>
        public static string CurrentInput = "";
        /// <summary>
        /// list of commands
        /// </summary>
        public static List<ICommand> Commands = new List<ICommand>();
        private static int flavorLength = 4;
        /// <summary>
        /// write a line to the console
        /// </summary>
        /// <param name="text"></param>
        public static void WriteLine(string text, string flavor = "info")
        {
            CurrentWriteLine++;
            Console.SetCursorPosition(0, CurrentWriteLine);
            WriteClear();
            Console.SetCursorPosition(0, CurrentWriteLine);
            while(flavor.Length < flavorLength)
            {
                flavor += " ";
            }
            if(flavor.Length > flavorLength)
            {
                flavor = flavor.Substring(0, flavorLength);
            }
            Console.WriteLine(">[" + flavor + "] " + text);
            WriteInput();
        }
        /// <summary>
        /// shows user input
        /// </summary>
        private static void WriteInput()
        {
            Console.SetCursorPosition(0, CurrentWriteLine + 1);
            WriteClear();
            Console.SetCursorPosition(0, CurrentWriteLine + 1);
            Console.WriteLine("<" + CurrentInput);
        }
        /// <summary>
        /// clears a line
        /// </summary>
        private static void WriteClear()
        {
            string clearStr = "";
            for(int i = 0; i < Console.BufferWidth; i++)
            {
                clearStr += " ";
            }
            Console.WriteLine(clearStr);
        }
        /// <summary>
        /// starts the console manager (to start reading user input)
        /// </summary>
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
                        WriteLine("<" + CurrentInput);
                        ProcessCommand(CurrentInput);
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
        /// <summary>
        /// calls a command
        /// </summary>
        /// <param name="input">the string input of the command</param>
        public static void ProcessCommand(string input)
        {
            string[] parts = input.Split(' ');
            if (parts.Length == 0) return;
            foreach(ICommand command in Commands)
            {
                if (command.CallCommand.Equals(parts[0]) && command.ArgumentCount + 1 == parts.Length /*plus one because parts includes the name of the command*/)
                {
                    command.Invoke(parts);
                    return;
                }
            }
            WriteLine("failed to find commmand \"" + parts[0] + "\"");
        }
    }
}
