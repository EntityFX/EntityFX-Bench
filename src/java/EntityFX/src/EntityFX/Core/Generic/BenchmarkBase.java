package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.concurrent.*;
import java.util.function.*;
import java.util.stream.IntStream;

import EntityFX.Core.Writer;

public abstract class BenchmarkBase<TResult> extends BenchmarkBaseBase implements BenchmarkInterface {

    public BenchmarkBase(final Writer writer, final boolean printToConsole) throws FileNotFoundException {
        super();

        this.Name = this.getClass().getSimpleName();
        this.printToConsole = printToConsole;
        this.output = writer == null ? new Writer(null) : writer;
    }

    public String getName() {
        return this.Name;
    }

    public BenchResult bench() throws Exception {
        this.beforeBench();
        final long start = System.currentTimeMillis();
        final TResult res = this.benchImplementation();
        final BenchResult result = this.populateResult(this.buildResult(start), res);
        doOutput(result);
        this.afterBench(result);
        return result;
    }

    protected <TBench, TBenchResult> BenchResult[] benchInParallel(final Supplier<TBench> buildFunc,
            final Function<TBench, TBenchResult> benchFunc,
            final BiConsumer<TBenchResult, BenchResult> setBenchResultFunc)
            throws InterruptedException, ExecutionException {
        UseConsole(false);
        final int threadNum = Runtime.getRuntime().availableProcessors();
        final List<TBench> benchs = new ArrayList<TBench>();
        for (int i = 0; i < threadNum; i++) {
            benchs.add(buildFunc.get());
        }

        final BenchResult[] results = new BenchResult[threadNum];

        final ExecutorService executor = Executors.newFixedThreadPool(threadNum);
        final List<FutureTask<BenchResult>> taskList = new ArrayList<FutureTask<BenchResult>>();

        int[] threads = IntStream.range(0, threadNum).toArray();
        for (int j : threads) {
            final FutureTask<BenchResult> futureTask = new FutureTask<BenchResult>(() -> {
                final TBench bench = benchs.get(j);
                final long start = System.currentTimeMillis();
                final TBenchResult result = benchFunc.apply(bench);
                final BenchResult benchResult = buildResult(start);
                setBenchResultFunc.accept(result, benchResult);
                return benchResult;
            });
            taskList.add(futureTask);
            executor.execute(futureTask);
        }

        for (int j : threads) {
            final FutureTask<BenchResult> futureTask = taskList.get(j);
            results[j] = futureTask.get();
        }

        executor.shutdown();
        UseConsole(true);
        return results;
    }

    protected void doOutput(final BenchResult result) throws IOException {
        if (result.Output == null) {
            return;
        }
        final FileWriter fileWriter = new FileWriter(Name + ".log");
        final PrintWriter printWriter = new PrintWriter(fileWriter);
        printWriter.print(result.Output);
        printWriter.close();
    }

    public void warmup(final Double aspect) throws Exception {
        Iterrations *= IterrationsRatio;
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

    public abstract TResult benchImplementation() throws IOException, Exception;

    public void UseConsole(final boolean printToConsole) {
        this.printToConsole = printToConsole;
        this.output.UseConsole = printToConsole;
    }

    public BenchResult populateResult(final BenchResult benchResult, final TResult dhrystoneResult) {
        if (dhrystoneResult instanceof BenchResult[]) {
            BenchResult[] results = (BenchResult[]) dhrystoneResult;
            return buildParallelResult(benchResult, results);
        }
        return benchResult;
    }

    protected BenchResult buildResult(final long start) {
        final long elapsed = System.currentTimeMillis() - start;
        final long tElapsed = elapsed == 0 ? 1 : elapsed;
        final double elapsedSeconds = tElapsed / 1000.0;
        final long iterrations = Iterrations;
        final double ratio = Ratio;
        return new BenchResult() {
            {
                BenchmarkName = Name;
                Elapsed = tElapsed;
                Points = iterrations / tElapsed * ratio;
                Result = (double) iterrations / elapsedSeconds;
                Units = "Iter/s";
                Iterrations = iterrations;
                Ratio = ratio;
            }
        };
    }

    protected BenchResult buildParallelResult(final long start, final BenchResult[] results) {
        final BenchResult result = buildResult(start);
        result.Points = Arrays.stream(results).mapToDouble(r -> r.Points).sum();
        result.IsParallel = isParallel;
        return result;
    }

    protected BenchResult buildParallelResult(final BenchResult rootResult, final BenchResult[] results) {
        rootResult.Points = Arrays.stream(results).mapToDouble(r -> r.Points).sum();
        rootResult.IsParallel = isParallel;
        rootResult.Iterrations = Iterrations;
        rootResult.Result = Arrays.stream(results).mapToDouble(r -> r.Result).sum();
        rootResult.Ratio = Ratio;
        return rootResult;
    }

    protected int[] range(final int start, final int stop) {
        final int[] result = new int[stop - start];

        for (int i = 0; i < stop - start; i++)
            result[i] = start + i;

        return result;
    }

    protected int[] randomIntArray(final int size, final int max) {
        final ThreadLocalRandom random = ThreadLocalRandom.current();
        final int[] ar = new int[size];
        for (int i = 0; i < size; i++) {
            ar[i] = random.nextInt(max);
        }
        return ar;
    }

    protected long[] randomLongArray(final int size) {
        final ThreadLocalRandom random = ThreadLocalRandom.current();
        final long[] ar = new long[size];
        for (int i = 0; i < size; i++) {
            ar[i] = random.nextLong();
        }
        return ar;
    }
}