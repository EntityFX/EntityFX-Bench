using System;
using System.IO;

namespace EntityFX.NetBenchmark.Core
{
    public class Writer 
    {
        private TextWriter writer = new StringWriter();

        public bool UseConsole { get; set; } = true;

        public void WriteLine(string format, params object[] args)
        {
            writer.WriteLine(format, args);
            Write(ConsoleColor.Gray, format, args);
            WriteLine();
        }

        public void WriteLine()
        {
            writer.WriteLine();
            if (UseConsole) Console.WriteLine();
        }

        public void WriteHeader(string format, params object[] args)
        {
            writer.WriteLine(format, args);
            Write(ConsoleColor.Cyan, format, args);
            WriteLine();
        }

        public void Write(string format, params object[] args)
        {
            Write(ConsoleColor.Gray, format, args);
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            writer.Write(format, args);
            if (UseConsole)
            {
                var tmpColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.Write(format, args);
                Console.ForegroundColor = tmpColor;
            }
        }

        public void WriteValue(string format, params object[] args)
        {
            Write(ConsoleColor.Green, format, args);
        }


        public void WriteTitle(string format, params object[] args)
        {
            Write(ConsoleColor.White, format, args);
        }

    }
}
