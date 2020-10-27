package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;

public class ParallelIfElseBenchmark extends BenchmarkBase<BenchResult[]> /* error link IBenchamrk */ {

    public ParallelIfElseBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 2000000000;
        Ratio = .01;
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return this.benchInParallel(() -> 0, a ->
        {
            int d = 0;
            for (long i = 0, c = -1; i < Iterrations; i++, c--)
            {
                c = c == -4 ? -1 : c;
                if (i == -1)
                {
                    d = 3;
                }
                else if (i == -2)
                {
                    d = 2;
                }
                else if (i == -3)
                {
                    d = 1;
                }
                d = d + 1;
            }
            return d;
        }, (a, r) -> { });
    }
}