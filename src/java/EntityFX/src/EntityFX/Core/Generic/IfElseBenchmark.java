package EntityFX.Core.Generic;

import java.io.FileNotFoundException;

import EntityFX.Core.Writer;

public class IfElseBenchmark extends BenchmarkBase<Integer> {
    public IfElseBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 2000000000;
        Ratio = 0.01;
    }

    @Override
    public Integer benchImplementation() {
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
    }
}