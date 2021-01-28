using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using EntityFX.NetBenchmark.Core;
using System.Windows.Forms;

namespace EntityFX.NetBenchmarks.Mobile
{
    class TextBoxWriter : IWriter
    {
        private TextBox _tb;

        public TextBoxWriter(TextBox tb)
        {
            _tb = tb;
        }

        #region IWriter Members

        public void Dispose()
        {
            _tb.Dispose();
        }

        public string Output
        {
            get { return _tb.Text; }
        }

        public bool UseConsole
        {
            get;
            set;
        }

        public bool UseFile
        {
            get;
            set;
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            if (!UseConsole)
            {
                return;
            }
            _tb.Text += String.Format(format, args);
        }

        public void Write(string format, params object[] args)
        {
            Write(ConsoleColor.Gray, format, args);
        }

        public void WriteHeader(string format, params object[] args)
        {
            Write(ConsoleColor.Cyan, format, args);
            WriteLine();
        }

        public void WriteLine()
        {
            if (!UseConsole)
            {
                return;
            }
            _tb.Text += "\r\n";
        }

        public void WriteLine(string format, params object[] args)
        {
            Write(ConsoleColor.Gray, format, args);
            WriteLine();
        }

        public void WriteTitle(string format, params object[] args)
        {
            Write(ConsoleColor.White, format, args);
        }

        public void WriteValue(string format, params object[] args)
        {
            Write(ConsoleColor.Green, format, args);
        }

        #endregion
    }
}
