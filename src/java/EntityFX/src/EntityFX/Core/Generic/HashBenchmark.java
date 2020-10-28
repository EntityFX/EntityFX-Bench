package EntityFX.Core.Generic;


import java.io.FileNotFoundException;

import EntityFX.Core.Writer;

public class HashBenchmark extends HashBase<byte[]> {
    public HashBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
    }

    @Override
    public byte[] benchImplementation() {
        byte[] result = new byte[] { };
        for (int i = 0; i < Iterrations; i++) {
            result = doHash(i, arrayOfBytes);
        }
        return result;
    }
}