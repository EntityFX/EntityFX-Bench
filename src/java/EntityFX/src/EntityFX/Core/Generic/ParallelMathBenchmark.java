package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;

public class ParallelMathBenchmark extends MathBase<BenchResult[]> /* error link IBenchamrk */ {

    public ParallelMathBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return benchInParallel(() -> 0, a ->
        {
            double R = 0;
            double li = 0;
            for (int i = 0; i < Iterrations; i++)
            {
                R += doMath(i, li);
            }
            return R;
        }, (a, r) -> { });
    }
}
