package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.util.Arrays;

import EntityFX.Core.Writer;

public abstract class HashBase<TResult> extends BenchmarkBase<TResult> {

    public HashBase(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 2000000;
        Ratio = 10;
        Object[] q = Arrays.stream(strs).map(str -> str.getBytes()).toArray();
        artayOfBytes = new byte[q.length][];
        int i = 0;
        for (Object o : q) {
            artayOfBytes[i++] = (byte[]) o;
        }
    }

    protected double R;

    protected String[] strs = new String[] { "the quick brown fox jumps over the lazy dog", "Some red wine",
            "Candels & Ropes" };

    protected byte[][] artayOfBytes;

    // protected static byte[] doHash(long i, unisharp.Outargwrapper<byte[][]> preparedBytes) throws Exception {
    //     try (SHA1Managed sha = new SHA1Managed()) {
    //         try (SHA256Managed sha256 = new SHA256Managed()) {
    //             return sha.ComputeHash(preparedBytes.value[(int) (i % (3L))])
    //                     /* error */.Concat(
    //                             sha256.ComputeHash(preparedBytes.value[(int) (((i + (1L))) % (3L))]) /* error */)
    //                     /* error */.ToArray() /* error */;
    //         }
    //     }
    //}

}