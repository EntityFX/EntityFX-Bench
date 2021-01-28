using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class StringManipulation : StringManipulationBase<string>, IBenchamrk
    {
        public StringManipulation(IWriter writer) : base(writer)
        {
        }

        public override string BenchImplementation()
        {
            var str = "the quick brown fox jumps over the lazy dog";
            string str1 = string.Empty;
            for (int i = 0; i < Iterrations; i++)
            {
                str1 = DoStringManipilation(str);
            }
            return str1;
        }
    }
}
