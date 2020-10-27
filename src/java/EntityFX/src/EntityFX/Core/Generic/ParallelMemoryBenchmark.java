package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Arrays;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;

public class ParallelMemoryBenchmark extends MemoryBenchmarkBase<BenchResult[]> /* error link IBenchamrk */ {

    public ParallelMemoryBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return this.benchInParallel(() -> 0, a -> {
            try {
                return benchRandomMemory();
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }, (a, r) -> {
            r.Result = a.Average;
            r.Output = a.Output;
        });
    }

    @Override
    public BenchResult populateResult(BenchResult benchResult, BenchResult[] results) {
        BenchResult result = this.buildParallelResult(benchResult, results);
        double sum = Arrays.stream(results).mapToDouble(r -> r.Result).sum();
        result.Result = sum;
        result.Points = sum * Ratio;
        result.Units = "MB/s";
        result.Output = String.join("", Arrays.toString(Arrays.stream(results).map(r -> r.Output).toArray()));
        return result;
    }
}