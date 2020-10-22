package EntityFX.Core.Generic;

import java.io.FileNotFoundException;

import EntityFX.Core.Writer;

public class CallBenchmark extends BenchmarkBase<Long> {
    public CallBenchmark(final Writer writer, final boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 2000000000;
        Ratio = 0.0001;
    }

    private long doCall(int i)
    {
        return i + 1;
    }

    @Override
    public Long benchImplementation() {
        int i = 0;
        long a= 0;
        for (i = 0; i < Iterrations; ++i)
        {
            a = this.doCall(i);
        }
        return a;
    }
}