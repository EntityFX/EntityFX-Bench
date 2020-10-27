package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;

public class ParallelArithemticsBenchmark extends ArithmeticsBase<BenchResult[]> /* error link IBenchamrk */ {

    public ParallelArithemticsBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return this.benchInParallel(() -> 0, a ->
        {
            float R2 = 0;
            for (int i = 0; i < Iterrations; i++)
            {
                R2 += doArithmetics(i);
            }
            return R2;
        }, (r, b) -> {});
    }
}
