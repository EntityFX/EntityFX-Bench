package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.AbstractMap.SimpleEntry;
import java.util.concurrent.*;

import EntityFX.Core.Writer;

public class CallBenchmark extends CallBenchmarBase<Double> {
    public CallBenchmark(final Writer writer, final boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
    }

    public BenchResult bench() throws IOException, InterruptedException, ExecutionException {
        this.beforeBench();

        final ExecutorService executor = Executors.newCachedThreadPool();
        final FutureTask<BenchResult> futureTask = new FutureTask<BenchResult>(() -> {
            final long start = System.currentTimeMillis();
            final SimpleEntry<Long, Double> result = doCallBench();
            final BenchResult benchResult = buildResult(start);
            benchResult.Elapsed = result.getKey();
            return benchResult;
        });

        executor.execute(futureTask);
        BenchResult callResult = futureTask.get();
        executor.shutdown();
        final BenchResult result = this.populateResult(callResult, callResult.Result);
        doOutput(result);
        this.afterBench(result);
        return result;
    }

    @Override
    public Double benchImplementation() throws IOException {
        return 0.0;
    }
}