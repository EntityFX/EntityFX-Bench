package EntityFX.Core.Whetstone;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Arrays;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class ParallelWhetstoneBenchmark extends BenchmarkBase<BenchResult[]> {

    public ParallelWhetstoneBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return benchInParallel(() -> {
            try {
                return new Whetstone(false);
            } catch (FileNotFoundException e1) {
                e1.printStackTrace();
            }
            return null;
        }, a -> {
            try {
                return a.bench(false);
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }, (a, r) -> {
            r.Points = a.getMWIPS() * Ratio;
            r.Result = a.getMWIPS();
            r.Output = a.getOutput();
        });
    }

    @Override
    public BenchResult populateResult(BenchResult benchResult, BenchResult[] whetstonBenchResults) {
        var result = buildParallelResult(benchResult, whetstonBenchResults);
        result.Result = Arrays.stream(whetstonBenchResults).mapToDouble(r -> r.Result).sum();
        result.Units = "MWIPS";
        result.Output = String.join("", Arrays.toString(Arrays.stream(whetstonBenchResults).map(r -> r.Output).toArray()));
        return result;
    }

    @Override
    public void warmup(Double aspect) {
    }
}