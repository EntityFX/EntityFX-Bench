package EntityFX.Core.Generic;

import java.io.FileNotFoundException;

import EntityFX.Core.Writer;

public abstract class StringManipulationBase<TResult> extends BenchmarkBase<TResult> {

    public StringManipulationBase(Writer writer, boolean printToConsole)  throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 5000000;
        Ratio = 10;
    }


    protected static String doStringManipilation(String str) {
        return (String.join("/", str.split(" ")).replace("/", "_").toUpperCase() + "AAA").toLowerCase().replace("aaa", ".");
    }

}