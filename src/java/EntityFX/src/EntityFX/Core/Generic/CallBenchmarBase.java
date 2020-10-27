package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.AbstractMap.SimpleEntry;

import EntityFX.Core.Writer;

public abstract class CallBenchmarBase<TResult> extends BenchmarkBase<TResult> {
    public CallBenchmarBase(final Writer writer, final boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 2000000000;
        Ratio = 0.01;
    }

    protected static double doCall(double i, double b)
    {
        double z = i * 0.7;
        double z1 = i * b;
        return  z + z1 + 0.5;
    }

    protected SimpleEntry<Long, Double> doCallBench() throws IOException {
        long start = System.currentTimeMillis();
        long elapsed1, elapsed2 = 0;
        int i = 0;
        double a = 0.0;

        for (i = 0; i < Iterrations; ++i) {
            double z = a * 0.7;
            double z1 = a * 0.01;
            a = z + z1 + 0.5;
        }
        elapsed1 = System.currentTimeMillis() - start;
        a = 0.0;
        i = 0;
        start = System.currentTimeMillis();
        for (i = 0; i < Iterrations; ++i) {
            a = doCall(a, 0.01);
        }
        elapsed2 = System.currentTimeMillis() - start;

        output.write("Elapsed No Call: %d", elapsed1);
        output.writeLine();
        output.write("Elapsed Call: %d", elapsed2);
        output.writeLine();
        long callTime = 0;

        if (elapsed2 <= elapsed1) {
            callTime = elapsed1 - elapsed2;
        } else {
            callTime = elapsed2 - elapsed1;
        }
        return new SimpleEntry<Long, Double>(callTime, a);
    }
}