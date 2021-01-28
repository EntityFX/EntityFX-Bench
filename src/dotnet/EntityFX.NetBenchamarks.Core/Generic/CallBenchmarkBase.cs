using System;
using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class CallBenchmarkBase<TResult> : BenchmarkBase<TResult>, IBenchamrk
    {
        public CallBenchmarkBase()
        {
            Iterrations = 2000000000;
            Ratio = 0.01;
        }

        //[MethodImpl(MethodImplOptions.NoInlining)]
        protected static float DoCall(float i, float b)
        {
            float z = i * 0.7f;
            float z1 = i * 0.01f;
            return z + z1 + 0.5f;
        }

        protected long DoCallBench()
        {
            BeforeBench();
            var sw = new Stopwatch();

            sw.Start();

            long elapsed1, elapsed2 = 0;
            int i = 0;
            float a = 0.0f;

            for (i = 0; i < Iterrations; ++i)
            {
                float z = a * 0.7f;
                float z1 = a * 0.01f;
                a = z + z1 + 0.5f;
            }
            elapsed1 = sw.ElapsedMilliseconds;
            a = 0.0f;
            i = 0;
            sw.Stop();
            sw.Start();
            for (i = 0; i < Iterrations; ++i)
            {
                a = DoCall(a, 0.01f);
            }
            elapsed2 = sw.ElapsedMilliseconds;

            long callTime;

            if (elapsed2 <= elapsed1)
            {
                callTime = elapsed1 - elapsed2;
            }
            else
            {
                callTime = elapsed2 - elapsed1;
            }

            sw.Stop();

            return callTime;
        }

    }
}
