using System;
using System.IO;

namespace EntityFX.NetBenchamarks.Core
{
    public class Writer 
    {
        private TextWriter writer = new StringWriter();

        public bool UseConsole { get; set; } = true;

        public void WriteLine(string format, params object[] args)
        {
            writer.WriteLine(format, args);
            if (UseConsole) Console.WriteLine(format, args);
        }

        public void WriteLine()
        {
            writer.WriteLine();
            if (UseConsole) Console.WriteLine();
        }

        public void Write(string format, params object[] args)
        {
            writer.Write(format, args);
            if (UseConsole) Console.Write(format, args);
        }
    }
}
