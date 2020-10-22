package EntityFX.Core.Generic;

import java.io.FileNotFoundException;

import EntityFX.Core.Writer;

public abstract class MathBase<TResult> extends BenchmarkBase<TResult> {

    public MathBase(Writer writer, boolean printToConsole)  throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 200000000;
        Ratio = 0.5;
    }


    protected static double doMath(int i, double li)
    {
        double rev = 1.0 / (i + 1.0);
        return Math.abs(i) * Math.acos(rev) * Math.asin(rev) * Math.atan(rev) +
            Math.floor(li) + Math.exp(rev) * Math.cos(i) * Math.sin(i) * Math.PI + Math.sqrt(i);
    }

}