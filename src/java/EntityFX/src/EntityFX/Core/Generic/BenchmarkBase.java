package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

import EntityFX.Core.Writer;

public abstract class BenchmarkBase<TResult> implements BenchmarkInterface {

    protected int Iterrations;

    protected boolean printToConsole = true;

    public static double AspectRatio = 0.1;

    public double Ratio = 1.0;

    protected String Name = "";

    protected Writer output;

    public BenchmarkBase(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super();
        this.Name = this.getClass().getSimpleName();
        this.printToConsole = printToConsole;
        this.output = writer == null ? new Writer(null) : writer;
    }

    public String getName() {
        return this.Name;
    }

    public BenchResult bench() throws IOException {
        this.beforeBench();
        final long start = System.currentTimeMillis();
        final TResult res = this.benchImplementation();
        final BenchResult result = this.populateResult(this.buildResult(start), res);
        if (result.Output != null) {
            final FileWriter fileWriter = new FileWriter(Name + ".log");
            final PrintWriter printWriter = new PrintWriter(fileWriter);
            printWriter.print(result.Output);
            printWriter.close();
        }
        this.afterBench(result);
        return result;
    }

    public void warmup(final Double aspect) throws IOException {
        final int tmp = Iterrations;
        Iterrations = (int) Math.round(Iterrations * aspect);
        this.UseConsole(false);
        this.bench();
        this.UseConsole(true);
        Iterrations = tmp;
    }

    protected void beforeBench() {

    }

    protected void afterBench(final BenchResult result) {

    }

    public abstract TResult benchImplementation() throws IOException;

    public void UseConsole(final boolean printToConsole) {
        this.printToConsole = printToConsole;
        this.output.UseConsole = printToConsole;
    }

    public BenchResult populateResult(final BenchResult benchResult, final TResult dhrystoneResult) {
        // if (dhrystoneResult is BenchResult[] results)
        // {
        // return BuildParallelResult(benchResult, results);
        // }
        return benchResult;
    }

    protected BenchResult buildResult(final long start) {
        final long elapsed = System.currentTimeMillis() - start;
        final long tElapsed = elapsed == 0 ? 1 : elapsed;
        return new BenchResult() {
            {
                BenchmarkName = Name;
                Elapsed = tElapsed;
                Points = Iterrations / tElapsed * Ratio;
                Result = (double) (Iterrations / tElapsed);
                Units = "Iter/s";
            }
        };
    }

    protected int[] range(int start, int stop) {
        int[] result = new int[stop - start];

        for (int i = 0; i < stop - start; i++)
            result[i] = start + i;

        return result;
    }

    protected int[] randomIntArray(int size, int max) {
        ThreadLocalRandom random = ThreadLocalRandom.current();
        int[]  ar = new int[size];
        for (int i = 0; i < size; i++) {
            ar[i] = random.nextInt(max);
        }
        return ar;
    }

    protected long[] randomLongArray(int size) {
        ThreadLocalRandom random = ThreadLocalRandom.current();
        long[]  ar = new long[size];
        for (int i = 0; i < size; i++) {
            ar[i] = random.nextLong();
        }
        return ar;
    }
}