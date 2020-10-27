package EntityFX.Core.Dhrystone;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Arrays;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class ParallelDhrystoneBenchmark extends BenchmarkBase<BenchResult[]> {

    public ParallelDhrystoneBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Ratio = 4;
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return benchInParallel(() -> {
            try {
                return new Dhrystone2(false);
            } catch (FileNotFoundException e1) {
                e1.printStackTrace();
            }
            return null;
        }, a -> {
            try {
                return a.Bench(Dhrystone2.LOOPS);
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }, (a, r) -> {
            r.Points = a.VaxMips * Ratio;
            r.Result = a.VaxMips;
            r.Output = a.Output;
        });
    }

    @Override
    public BenchResult populateResult(BenchResult benchResult, BenchResult[] dhrystonBenchResults) {
        var result = buildParallelResult(benchResult, dhrystonBenchResults);
        result.Result = Arrays.stream(dhrystonBenchResults).mapToDouble(r -> r.Result).sum();
        result.Units = "DMIPS";
        result.Output = String.join("", Arrays.toString(Arrays.stream(dhrystonBenchResults).map(r -> r.Output).toArray()));
        return result;
    }

    @Override
    public void warmup(Double aspect) {
    }
}