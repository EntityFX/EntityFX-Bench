package EntityFX.Core.Generic;

import java.io.FileNotFoundException;

import EntityFX.Core.Writer;

public class MathBenchmark extends MathBase<Double> {
    public MathBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        // TODO Auto-generated constructor stub
    }

    @Override
    public Double benchImplementation() {
        double R = 0;

        double li = 0;
        for (int i = 0; i < Iterrations; li = i, i++)
        {
            R += doMath(i, li);
        }
        return R;
    }
}