import java.io.IOException;

import java.util.Arrays;
import java.util.Locale;

import EntityFX.Core.Writer;
import EntityFX.Core.Dhrystone.*;
import EntityFX.Core.Generic.*;
import EntityFX.Core.Scimark2.*;
import EntityFX.Core.Whetstone.*;

public class App {
    public static void main(String[] args) throws Exception {
        Locale.setDefault(new Locale("en", "US"));

        BenchmarkBaseBase.IterrationsRatio = args.length > 0 ? Float.parseFloat(args[0]): 1.0;

        Writer writer = new Writer("Output.log");

        BenchmarkInterface[] benchmarks = new BenchmarkInterface[] { 
            new ArithemticsBenchmark(writer, true),
            new ParallelArithemticsBenchmark(writer, true),

            new MathBenchmark(writer, true),
            new ParallelMathBenchmark(writer, true),

            new CallBenchmark(writer, true),
            new ParallelCallBenchmark(writer, true),

            new IfElseBenchmark(writer, true),
            new ParallelIfElseBenchmark(writer, true),

            new StringManipulation(writer, true),
            new ParallelStringManipulation(writer, true),

            new MemoryBenchmark(writer, true),
            new ParallelMemoryBenchmark(writer, true),

            new RandomMemoryBenchmark(writer, true),
            new ParallelRandomMemoryBenchmark(writer, true),

            new Scimark2Benchmark(writer, true),
            new ParallelScimark2Benchmark(writer, true),

            new DhrystoneBenchmark(writer, true),
            new ParallelDhrystoneBenchmark(writer, true),
            
            new WhetstoneBenchmark(writer, true),
            new ParallelWhetstoneBenchmark(writer, true),

            new HashBenchmark(writer, true),
            new ParallelHashBenchmark(writer, true),
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

        String osVersion = System.getProperty("os.name") + " " + System.getProperty("os.version")+ " " + System.getProperty("os.arch");
        String environmentVersion = "Java Version " + System.getProperty("java.version");
        int threadsCount = Runtime.getRuntime().availableProcessors();
        long workingSet = Runtime.getRuntime().totalMemory() - Runtime.getRuntime().freeMemory();

        String headerCommon = "Operating System,Runtime,Threads Count,Memory Used";
        String headerTotals = ",Total Points,Total Time (ms)";

        writer.writeLine();
        writer.writeHeader("Single-thread results");
        writer.writeTitle(headerCommon);
        Arrays.stream(result).filter(r -> !r.IsParallel).forEach(r -> {
            try {
                writer.writeTitle(",%s", r.BenchmarkName);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(headerTotals);
        writer.writeLine();
        writer.writeTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, workingSet);
        Arrays.stream(result).filter(r -> !r.IsParallel).forEach(r -> {
            try {
                writer.writeValue(",%.2f", r.Points);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(",%.2f,%d", totalPoints, total);
        writer.writeLine();


        writer.writeHeader("All results");
        writer.writeTitle(headerCommon);
        Arrays.stream(result).forEach(r -> {
            try {
                writer.writeTitle(",%s", r.BenchmarkName);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(headerTotals);
        writer.writeLine();
        writer.writeTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, workingSet);
        Arrays.stream(result).forEach(r -> {
            try {
                writer.writeValue(";%.2f", r.Points);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(";%.2f;%d", totalPoints, total);
        writer.writeLine();

        writer.writeHeader("Single-thread  Units results");
        writer.writeTitle(headerCommon);
        Arrays.stream(result).filter(r -> !r.IsParallel).forEach(r -> {
            try {
                writer.writeTitle(",%s (%s)", r.BenchmarkName, r.Units);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(headerTotals);
        writer.writeLine();
        writer.writeTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, workingSet);
        Arrays.stream(result).filter(r -> !r.IsParallel).forEach(r -> {
            try {
                writer.writeValue(";%.2f", r.Result);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(";%.2f;%d", totalPoints, total);
        writer.writeLine();

        writer.writeHeader("All  Units results");
        writer.writeTitle(headerCommon);
        Arrays.stream(result).forEach(r -> {
            try {
                writer.writeTitle(",%s (%s)", r.BenchmarkName, r.Units);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(headerTotals);
        writer.writeLine();
        writer.writeTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, workingSet);
        Arrays.stream(result).forEach(r -> {
            try {
                writer.writeValue(";%.2f", r.Result);
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        writer.writeTitle(";%.2f;%d", totalPoints, total);
        writer.writeLine();
    }

    private static void writeResult(Writer writer, BenchResult benchResult) throws IOException {
        writer.writeTitle("%-30s", benchResult.BenchmarkName);
        writer.writeValue("%15d ms", benchResult.Elapsed);
        writer.writeValue("%13.2f pts", benchResult.Points);
        writer.writeValue("%15.2f %s", benchResult.Result, benchResult.Units);
        writer.writeLine();
        writer.writeValue("Iterrations: %15d, Ratio: %15f", benchResult.Iterrations, benchResult.Ratio);
        writer.writeLine();
    }
}
