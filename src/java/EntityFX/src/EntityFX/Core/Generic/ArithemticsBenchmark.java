package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;

public class ArithemticsBenchmark extends ArithmeticsBase<Double> {
    public ArithemticsBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        // TODO Auto-generated constructor stub
    }

    @Override
    public Double benchImplementation() {
        R = 0;
        for (long i = 0; i < Iterrations; i++) {
            R += doArithmetics(i);
        }
        return R;
    }
}