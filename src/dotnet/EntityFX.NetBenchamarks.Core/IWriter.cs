using System;
namespace EntityFX.NetBenchmark.Core
{
    public interface IWriter
    {
        void Dispose();
        string Output { get; }
        bool UseConsole { get; set; }
        bool UseFile { get; set; }
        void Write(ConsoleColor color, string format, params object[] args);
        void Write(string format, params object[] args);
        void WriteHeader(string format, params object[] args);
        void WriteLine();
        void WriteLine(string format, params object[] args);
        void WriteTitle(string format, params object[] args);
        void WriteValue(string format, params object[] args);
    }
}
