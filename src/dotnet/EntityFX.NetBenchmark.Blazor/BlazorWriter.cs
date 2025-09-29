using EntityFX.NetBenchmark.Core;
using System;
using System.IO;

namespace EntityFX.NetBenchmark.Blazor
{
    public class BlazorWriter : IDisposable, IWriter
    {
        private TextWriter writer = new StringWriter();

        public bool UseConsole { get; set; }

        public bool UseFile { get; set; }

        public event EventHandler<string> TextWrite;

        private string consoleBuffer = string.Empty;


        public BlazorWriter()
        {
            UseConsole = true;
        }

        public string Output
        {
            get
            {
                return writer.ToString();
            }
        }


        public void WriteLine(string format, params object[] args)
        {
            Write(ConsoleColor.Gray, format, args);
            WriteLine();
        }

        public void WriteLine()
        {
            writer.WriteLine();
            if (UseConsole) Console.WriteLine();
            TextWrite?.Invoke(this, "\n");
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
            var text = string.Format(format, args);
            writer.Write(text);
            if (UseConsole)
            {
                Console.Write(text);
            }
            TextWrite?.Invoke(this, text);
        }

        public void WriteValue(string format, params object[] args)
        {
            Write(ConsoleColor.Green, format, args);
        }


        public void WriteTitle(string format, params object[] args)
        {
            Write(ConsoleColor.White, format, args);
        }

        public void Dispose()
        {
            writer.Flush();
        }
    }
}
