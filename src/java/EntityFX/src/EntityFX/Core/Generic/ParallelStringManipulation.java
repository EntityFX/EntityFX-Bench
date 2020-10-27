package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;

public class ParallelStringManipulation extends StringManipulationBase<BenchResult[]> /* error link IBenchamrk */ {

    public ParallelStringManipulation(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return this.benchInParallel(() -> 0, a ->
        {
            var str = "the quick brown fox jumps over the lazy dog";
            String str1 = "";
            for (int i = 0; i < Iterrations; i++)
            {
                str1 = doStringManipilation(str);
            }
            return str1;
        }, (a, r) -> { });
    }
}