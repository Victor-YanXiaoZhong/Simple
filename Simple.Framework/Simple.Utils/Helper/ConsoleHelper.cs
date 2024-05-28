using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils
{
    public class ConsoleHelper
    {
        private static void Write(object msg, ConsoleColor color)
        {
            Console.Write("{0 :G}：", System.DateTime.Now);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void Info(object msg, ConsoleColor color = ConsoleColor.White)
        {
            Write(msg, color);
        }

        public static void Err(object msg, ConsoleColor color = ConsoleColor.Red)
        {
            Write(msg, color);
        }

        public static void Waring(object msg, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            Write(msg, color);
        }

        public static void Data(object msg, ConsoleColor color = ConsoleColor.DarkCyan, bool inNewLine = false)
        {
            if (inNewLine) { Console.WriteLine(); }
            Write(msg, color);
        }

        public static void Debug(object msg, ConsoleColor color = ConsoleColor.DarkGreen, bool inNewLine = false)
        {
            if (inNewLine) { Console.WriteLine(); }
            Write(msg, color);
        }

        public static void Pause()
        { Console.ReadKey(); }
    }
}