package EntityFX.Core.Scimark2;

import java.io.FileNotFoundException;
import java.util.Arrays;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class ParallelScimark2Benchmark extends BenchmarkBase<BenchResult[]> {

    public ParallelScimark2Benchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Ratio = 10;
        isParallel = true;
    }

    /* error value type 'BenchResult' of method BenchImplementation */
    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return benchInParallel(() -> {
            try {
                return new Scimark2(output, false);
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
            return null;
        }, a -> {
            try {
                return a.bench(Constants.RESOLUTION_DEFAULT, false);
            } catch (Exception e) {
                e.printStackTrace();
            }
            return null;
        }, (a, r) -> {
            r.Points = a.getCompositeScore() * Ratio;
            r.Result = a.getCompositeScore();
            r.Output = a.getOutput();
        });
    }

    /* error value type 'BenchResult' of method PopulateResult */
    @Override
    public BenchResult populateResult(BenchResult benchResult, BenchResult[] scimark2Result) {
        var result = buildParallelResult(benchResult, scimark2Result) /* error */;
        result.Result = Arrays.stream(scimark2Result).mapToDouble(r -> r.Result).sum();
        result.Units = "CompositeScore";
        result.Output = String.join("", Arrays.toString(Arrays.stream(scimark2Result).map(r -> r.Output).toArray()));
        return result;
    }

    @Override
    public void warmup(Double aspect) {
    }
}
