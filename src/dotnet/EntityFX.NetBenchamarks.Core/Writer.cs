using System;
using System.IO;

namespace EntityFX.NetBenchmark.Core
{
    public class Writer : IDisposable, EntityFX.NetBenchmark.Core.IWriter
    {
        private TextWriter writer = new StringWriter();

        private StreamWriter streamWriter;

        public bool UseConsole { get; set; }

        public bool UseFile { get; set; }

        public string FilePath { get; private set; }

        public Writer(string filePath)
        {
            UseConsole = true;
            if (filePath != null)
            {
                FilePath = filePath;
                UseFile = true;

                FileStream mystream = new FileStream(filePath,
                    FileMode.OpenOrCreate, FileAccess.Write);

                streamWriter = new StreamWriter(mystream);
                streamWriter.AutoFlush = true;
            }
        }

        public string Output {
            get { 
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
            if (UseFile)
            {
                streamWriter.WriteLine();
            }
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
#if NETSTANDARD2_0
                var tmpColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
#endif
                Console.Write(format, args);
#if NETSTANDARD2_0
                Console.ForegroundColor = tmpColor;
#endif
            }
            if (UseFile)
            {
                streamWriter.Write(format, args);
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

        public void Dispose()
        {
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }
}
