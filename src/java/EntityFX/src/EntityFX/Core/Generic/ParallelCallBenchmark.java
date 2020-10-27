package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.util.*;
import java.util.AbstractMap.SimpleEntry;
import java.util.concurrent.*;
import java.util.function.*;
import java.util.stream.IntStream;

import EntityFX.Core.Writer;

public class ParallelCallBenchmark extends CallBenchmarBase<BenchResult[]> /* error link IBenchamrk */ {

    public ParallelCallBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        isParallel = true;
    }

    @Override
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
                final SimpleEntry<Long, Double> result = doCallBench();
                final BenchResult benchResult = buildResult(start);
                benchResult.Elapsed = result.getKey();
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

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return this.benchInParallel(() -> 0, a -> {
            return doCall(a, a);
        }, (a, r) -> {
        });
    }
}