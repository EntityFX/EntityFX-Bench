import java.io.IOException;

import EntityFX.Core.Writer;
import EntityFX.Core.Dhrystone.Dhrystone2;
import EntityFX.Core.Generic.*;
import EntityFX.Core.Scimark2.Constants;
import EntityFX.Core.Scimark2.Scimark2;
import EntityFX.Core.Whetstone.Whetstone;

public class App {
    public static void main(String[] args) throws Exception {
        Writer writer = new Writer("Output.log");

        BenchmarkInterface[] benchmarks = new BenchmarkInterface[] { 
            new MemoryBenchmark(writer, true),
            new RandomMemoryBenchmark(writer, true),
            new ArithemticsBenchmark(writer, true),
            new MathBenchmark(writer, true),
            new CallBenchmark(writer, true),
            new IfElseBenchmark(writer, true),
            new StringManipulation(writer, true)
        };

        writer.writeHeader("Warmup");

        for (BenchmarkInterface benchmark : benchmarks) {
            benchmark.warmup(0.05);
            writer.write(".");
        }

        writer.writeLine();
        writer.writeHeader("Bench");

        long total = 0;
        double totalPoints = 0;
        String[] points = new String[benchmarks.length];
        int i = 1;
        BenchResult[] result = new BenchResult[benchmarks.length];

        for (BenchmarkInterface benchmark : benchmarks) {
            writer.writeHeader("[%d] %s", i, benchmark.getName());
            BenchResult r = benchmark.bench();
            total += r.Elapsed;
            totalPoints += r.Points;
            points[i-1] = String.format("%.2f", r.Points);
            App.writeResult(writer, r);

            result[i-1] = r;
            i++;
        }

        writer.writeLine();
        writer.writeTitle("%-30s", "Total:");
        writer.writeValue("%15d ms", total);
        writer.writeValue("%13.2f pts", totalPoints);
        writer.writeLine();
    }

    private static void writeResult(Writer writer, BenchResult benchResult) throws IOException {
        writer.writeTitle("%-30s", benchResult.BenchmarkName);
        writer.writeValue("%15d ms", benchResult.Elapsed);
        writer.writeValue("%13.2f pts", benchResult.Points);
        if (benchResult.Result != null) {
            writer.writeValue("%13.2f %s", benchResult.Result, benchResult.Units);
        }
        writer.writeLine();
    }
}
