using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using EntityFX.NetBenchmark.Core;
using System.Windows.Forms;
using System.IO;

namespace EntityFX.NetBenchmarks.Mobile
{
    class AsyncWriterEventArgs : EventArgs
    {
        public string Text { get; set; }

        public AsyncWriterEventArgs(string text)
        {
            this.Text = text;
        }
    }

    class AsyncWriter : IWriter
    {
        private IWriter _writer;

        public event EventHandler<AsyncWriterEventArgs> OnWrite;

        public event EventHandler OnWriteLine;

        public AsyncWriter(string filePath)
        {
            _writer = new Writer(filePath);
            
        }

        #region IWriter Members

        public void Dispose()
        {
            _writer.Dispose();
        }

        public string Output
        {
            get { return _writer.Output; }
        }

        public bool UseConsole
        {
            get { return _writer.UseConsole; }
            set { _writer.UseConsole = value; }
        }

        public bool UseFile
        {
            get;
            set;
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            _writer.Write(color, format, args);
            if (!UseConsole)
            {
                return;
            }
            if (OnWrite != null)
            {
                OnWrite.Invoke(this, new AsyncWriterEventArgs( String.Format(format, args)));
            }
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
            _writer.WriteLine();
            if (!UseConsole)
            {
                return;
            }

            if (OnWriteLine != null)
            {
                OnWriteLine.Invoke(this, new EventArgs());
            }
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
