package EntityFX.Core.Linpack;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Arrays;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class ParallelLinpackBenchmark extends BenchmarkBase<BenchResult[]> {

    public ParallelLinpackBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Ratio = 5;
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return benchInParallel(() -> {
            try {
                return new Linpack(false);
            } catch (FileNotFoundException e1) {
                e1.printStackTrace();
            }
            return null;
        }, a -> {
            try {
                return a.run_benchmark(2000);
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }, (a, r) -> {
            r.Points = a.MFLOPS * Ratio;
            r.Result = a.MFLOPS;
            r.Output = a.output;
        });
    }

    @Override
    public BenchResult populateResult(BenchResult benchResult, BenchResult[] linpackBenchResults) {
        BenchResult result = buildParallelResult(benchResult, linpackBenchResults);
        result.Result = Arrays.stream(linpackBenchResults).mapToDouble(r -> r.Result).sum();
        result.Units = "MFLOPS";
        result.Output = String.join("", Arrays.toString(Arrays.stream(linpackBenchResults).map(r -> r.Output).toArray()));
        return result;
    }

    @Override
    public void warmup(Double aspect) {
    }
}