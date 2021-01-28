using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EntityFX.NetBenchmark.Core;
using EntityFX.NetBenchmark.Core.Generic;
using System.IO;
using EntityFX.NetBenchmark.Core.Dhrystone;
using EntityFX.NetBenchmark.Core.Scimark2;
using EntityFX.NetBenchmark.Core.Whetstone;
using EntityFX.NetBenchmark.Core.Linpack;

namespace EntityFX.NetBenchmarks.Mobile
{
    public partial class Form1 : Form
    {
        private IWriter writer;

        public Form1()
        {
            InitializeComponent();
            writer = new TextBoxWriter(textBox1, "Output.log");
            BenchmarkBase.IterrationsRatio = BenchmarkBase.IterrationsRatio * 0.01;
#if DEBUG
            BenchmarkBase.IterrationsRatio = BenchmarkBase.IterrationsRatio * 0.002;
#endif
        }

        private void WriteResult(BenchResult benchResult)
        {
            writer.WriteTitle("{0,-30}", benchResult.BenchmarkName);
            writer.WriteValue("{0,15} ms", string.Format("{0:F2}", benchResult.Elapsed.TotalMilliseconds));
            writer.WriteValue("{0,15} pts", string.Format("{0:F2}", benchResult.Points));
            writer.WriteValue("{0,15} {1}", string.Format("{0:F2}", benchResult.Result), benchResult.Units);
            writer.WriteLine();
            writer.WriteValue("Iterrations: {0,15}, Ratio: {1,15}", benchResult.Iterrations, benchResult.Ratio);

            writer.WriteLine();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            var benchMarks = new IBenchamrk[] 
            {
                new ArithemticsBenchmark(writer),
                new MathBenchmark(writer),
                new CallBenchmark(writer),
                new IfElseBenchmark(writer),
                new StringManipulation(writer),
                new MemoryBenchmark(true, writer),
                new RandomMemoryBenchmark(true, writer),
                new Scimark2Benchmark(writer),
                new DhrystoneBenchmark(writer),
                new WhetstoneBenchmark(writer),
                new LinpackBenchmark(writer),
                new HashBenchmark(writer),
            };

            TimeSpan singleThreadTotal = TimeSpan.Zero;
            TimeSpan total = TimeSpan.Zero;

            double singleThreadTotalPoints = 0;
            double totalPoints = 0;


            List<BenchResult> result = new List<BenchResult>();


            writer.WriteHeader("Warmup");
            foreach (var bench in benchMarks)
            {
                bench.Warmup(0.05);
                writer.Write(".");
            }

            writer.WriteLine();
            writer.WriteHeader("Bench");
            int i = 1;
            foreach (var bench in benchMarks)
            {
                writer.WriteHeader("[{0}] {1}", i, bench.Name);
                var r = bench.Bench();
                if (!bench.IsParallel)
                {
                    singleThreadTotal += r.Elapsed;
                    singleThreadTotalPoints += r.Points;
                }
                total += r.Elapsed;
                totalPoints += r.Points;
                WriteResult(r);
                result.Add(r);
                i++;
            }

            writer.WriteLine();
            writer.WriteTitle("{0,-30}", "Total:");
            writer.WriteValue("{0,15} ms", string.Format("{0:F2}", total.TotalMilliseconds));
            writer.WriteValue("{0,15} pts", string.Format("{0:F2}", totalPoints));
            writer.WriteLine();

            var headerCommon = "Operating System,Runtime,Threads Count,Memory Used";
            var headerTotals = ",Total Points,Total Time (ms)";

            int processors = 1;
            long workingSet = 0;


            writer.WriteLine();
            writer.WriteHeader("Single-thread results");
            writer.WriteTitle(headerCommon);
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteTitle(",{0}", r.BenchmarkName));
            writer.WriteTitle(headerTotals);
            writer.WriteLine();
            writer.WriteTitle("{0},{1},{2},{3}", Environment.OSVersion, Environment.Version, processors, workingSet);
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteValue(string.Format(",{0:F2}", r.Points)));
            writer.WriteTitle(",{0},{1}", string.Format("{0:F2}", totalPoints), string.Format("{0:F2}", total.TotalMilliseconds));

            writer.WriteLine();
            writer.WriteHeader("Single-thread  Units results");
            writer.WriteTitle(headerCommon);
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteTitle(",{0}", r.BenchmarkName));
            writer.WriteTitle(headerTotals);
            writer.WriteLine();
            writer.WriteTitle("{0},{1},{2},{3}", Environment.OSVersion, Environment.Version, processors, workingSet);
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteValue(string.Format(",{0:F2}", r.Result)));
            writer.WriteTitle(",{0},{1}", string.Format("{0:F2}", totalPoints), string.Format("{0:F2}", total.TotalMilliseconds));
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var sw = new StreamWriter(saveFileDialog1.FileName);
                sw.Write(writer.Output);
            }

        }
    }
}