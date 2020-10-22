package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;

public class StringManipulation extends StringManipulationBase<String> {
    public StringManipulation(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        // TODO Auto-generated constructor stub
    }

    @Override
    public String benchImplementation() {
        String str = "the quick brown fox jumps over the lazy dog";
        String str1 = "";
        for (int i = 0; i < Iterrations; i++)
        {
            str1 = doStringManipilation(str);
        }
        return str1;
    }
}