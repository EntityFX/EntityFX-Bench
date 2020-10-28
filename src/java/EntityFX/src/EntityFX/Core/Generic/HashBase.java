package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.util.Arrays;

import EntityFX.Core.Writer;

public abstract class HashBase<TResult> extends BenchmarkBase<TResult> {

    public HashBase(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 2000000;
        Ratio = 10;
        Object[] q = Arrays.stream(strs).map(str -> {
            try {
                return str.getBytes("ASCII");
            } catch (UnsupportedEncodingException e1) {
                e1.printStackTrace();
            }
            return new byte[0];
        }).toArray();
        arrayOfBytes = new byte[q.length][];
        int i = 0;
        for (Object o : q) {
            arrayOfBytes[i++] = (byte[]) o;
        }

    }

    protected String[] strs = new String[] { "the quick brown fox jumps over the lazy dog", "Some red wine",
            "Candels & Ropes" };

    protected byte[][] arrayOfBytes;

    protected static byte[] doHash(int i, byte[][] preparedBytes) {
        MessageDigest sha1Crypt;
        MessageDigest sha256Crypt;

        try {
            sha1Crypt = MessageDigest.getInstance("SHA-1");
            sha256Crypt = MessageDigest.getInstance("SHA-256");
            sha1Crypt.reset();
            sha256Crypt.reset();
            sha1Crypt.update(preparedBytes[i % 3]);
            sha256Crypt.update(preparedBytes[(i + 1) % 3]);
            byte[] sha1bytes = sha1Crypt.digest();
            byte[] sha256bytes = sha256Crypt.digest();
            byte[] result = new byte[sha1bytes.length + sha256bytes.length];
            System.arraycopy(sha1bytes, 0, result, 0, sha1bytes.length);
            System.arraycopy(sha256bytes, 0, result, sha1bytes.length, sha256bytes.length);
            return result;
        } catch (Exception e) {
            e.printStackTrace();
            return new byte[0];
        }
    }

}