using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsCollection.Classes
{
    internal static class ColorConsole
    {
        public static void WriteColor(string content, ConsoleColor color)
        {
            ConsoleColor p = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(content);
            Console.ForegroundColor = p;
        }
        public static void WriteLineColor(string content, ConsoleColor color)
        {
            ConsoleColor p = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(content);
            Console.ForegroundColor = p;
        }

        #region ConsoleErrors
        public static void InvalidCommand()
        {
            WriteError("Invalid Command");
        }
        public static void NoValidTags()
        {
            WriteError("No Valid Tags Listed");
        }
        public static void InvalidPath(string path)
        {
            WriteError($"\"{path}\" cannot be used as filename");
        }
        public static void WriteError(string message)
        {
            WriteLineColor(message, ConsoleColor.Red);
        }
        public static void InvalidWordName(string wordName)
        {
            WriteError($"Invalid word name: word \"{wordName}\" does not exist");
        }
        public static void InvalidTagName(string tagName)
        {
            WriteError($"Invalid tag name: tag \"{tagName}\" does not exist");
        }
        #endregion ConsoleErrors
    }
}
