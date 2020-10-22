package EntityFX.Core.Generic;

import java.io.FileNotFoundException;

import EntityFX.Core.Writer;

public abstract class ArithmeticsBase<TResult> extends BenchmarkBase<TResult> {

    protected double R;

    public ArithmeticsBase(Writer writer, boolean printToConsole)  throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 300000000;
        Ratio = 0.03;
    }


    protected static float doArithmetics(final int i) {
        return (i / 10) * (i / 100) * (i / 100) * (i / 100) * 1.11f + (i / 100) * (i / 1000) * (i / 1000) * 2.22f
                - i * (i / 10000) * 3.33f + i * 5.33f;
    }

    protected static double doArithmetics(final long i) {
        return (i / 10) * (i / 100) * (i / 100) * (i / 100) * 1.11 + (i / 100) * (i / 1000) * (i / 1000) * 2.22 - i * (i / 10000) * 3.33 + i * 5.33;
    }

}