package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.util.concurrent.ExecutionException;

import EntityFX.Core.Writer;

public class ParallelHashBenchmark extends HashBase<BenchResult[]> {

    public ParallelHashBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        isParallel = true;
    }

    @Override
    public BenchResult[] benchImplementation() throws InterruptedException, ExecutionException {
        return benchInParallel(() -> 0, a -> {
            byte[] hash = new byte[0];
            for (int i = 0; i < Iterrations; i++) {
                hash = doHash(i, arrayOfBytes);
            }
            return hash;
        }, (a, r) -> {
        });
    }
}