package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.lang.reflect.Array;
import java.util.AbstractMap;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;
import java.util.AbstractMap.SimpleEntry;
import java.util.Map.Entry;
import java.util.concurrent.ThreadLocalRandom;

import javax.xml.crypto.dsig.keyinfo.KeyValue;

import EntityFX.Core.Writer;

public abstract class MemoryBenchmarkBase<TResult> extends BenchmarkBase<TResult> {

    public MemoryBenchmarkBase(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 500000;
        Ratio = 1;
    }

    public MemoryBenchmarkResult BenchRandomMemory() throws IOException
    {
        var int4k = MeasureArrayRandomRead(1024);
        output.writeLine("int 4k: %.2f MB/s", int4k.getKey());
        var int512k = MeasureArrayRandomRead(131072);
        output.writeLine("int 512k: %.2f MB/s", int512k.getKey());
        var int8m = MeasureArrayRandomRead(2097152);
        output.writeLine("int 8M: %.2f MB/s", int8m.getKey());
        var int32m = MeasureArrayRandomRead(32 * 1024 * 1024 / Integer.BYTES);
        output.writeLine("int 32M: %.2f MB/s", int32m.getKey());

        var long4k = MeasureArrayRandomLongRead(1024);
        output.writeLine("long 4k: %.2f MB/s", long4k.getKey());
        var long512k = MeasureArrayRandomLongRead(131072);
        output.writeLine("long 512k: %.2f MB/s", long512k.getKey());
        var long8m = MeasureArrayRandomLongRead(2097152);
        output.writeLine("long 8M: %.2f MB/s", long8m.getKey());
        var long32m = MeasureArrayRandomLongRead(32 * 1024 * 1024 / Long.BYTES);
        output.writeLine("long 32M: %.2f MB/s", long32m.getKey());
        
        var avg = Arrays.stream(new double[] { 
            int4k.getKey(), int512k.getKey(), int8m.getKey(), int32m.getKey(),
            long4k.getKey(), long512k.getKey(), long8m.getKey(), long32m.getKey(),
        }).average().orElse(-1);

        output.writeLine("Average: %.2f MB/s", avg);

        return new MemoryBenchmarkResult()
        {{
            Average = avg;
            Output = output.Output;
        }};
    }

    protected SimpleEntry<Double, int[]> MeasureArrayRandomRead(int size) {
        int blockSize = 16;
        int[] I = new int[blockSize];

        int[] array = randomIntArray(size, Integer.MAX_VALUE);
        var end = array.length - 1;
        var k0 = (size / 1024);
        var k1 = k0 == 0 ? 1 : k0;
        var iterInternal = Iterrations / k1;
        iterInternal = iterInternal == 0 ? 1 : iterInternal;
        for (int idx = 0; idx < end; idx += blockSize) {
            // System.Buffer.BlockCopy(array, idx, I, 0, blockSizeBytes);
            I[0] = array[idx];
            I[1] = array[idx + 1];
            I[2] = array[idx + 2];
            I[3] = array[idx + 3];
            I[4] = array[idx + 4];
            I[5] = array[idx + 5];
            I[6] = array[idx + 6];
            I[7] = array[idx + 7];
            I[8] = array[idx + 8];
            I[9] = array[idx + 9];
            I[0xA] = array[idx + 0xA];
            I[0xB] = array[idx + 0xB];
            I[0xC] = array[idx + 0xC];
            I[0xD] = array[idx + 0xD];
            I[0xE] = array[idx + 0xE];
            I[0xF] = array[idx + 0xF];
        }
        var start = System.currentTimeMillis();
        for (int i = 0; i < iterInternal; i++) {
            for (int idx = 0; idx < end; idx += blockSize) {
                // System.Buffer.BlockCopy(array, idx, I, 0, blockSizeBytes);
                I[0] = array[idx];
                I[1] = array[idx + 1];
                I[2] = array[idx + 2];
                I[3] = array[idx + 3];
                I[4] = array[idx + 4];
                I[5] = array[idx + 5];
                I[6] = array[idx + 6];
                I[7] = array[idx + 7];
                I[8] = array[idx + 8];
                I[9] = array[idx + 9];
                I[0xA] = array[idx + 0xA];
                I[0xB] = array[idx + 0xB];
                I[0xC] = array[idx + 0xC];
                I[0xD] = array[idx + 0xD];
                I[0xE] = array[idx + 0xE];
                I[0xF] = array[idx + 0xF];
            }
        }
        var elapsed = System.currentTimeMillis() - start;

        return new SimpleEntry<Double, int[]>((double) (iterInternal * array.length * 4.0 / (elapsed / 1000.0) / 1024 / 1024), I);
    }

    protected SimpleEntry<Double, long[]> MeasureArrayRandomLongRead(int size) {
        int blockSize = 8;
        long[] I = new long[blockSize];

        long[] array = this.randomLongArray(size);

        var end = array.length - 1;
        var k0 = (size / 1024);
        var k1 = k0 == 0 ? 1 : k0;
        var iterInternal = Iterrations / k1;
        iterInternal = iterInternal == 0 ? 1 : iterInternal;
        for (int idx = 0; idx < end; idx += blockSize) {
            // System.Buffer.BlockCopy(array, idx, I, 0, blockSizeBytes);
            I[0] = array[idx];
            I[1] = array[idx + 1];
            I[2] = array[idx + 2];
            I[3] = array[idx + 3];
            I[4] = array[idx + 4];
            I[5] = array[idx + 5];
            I[6] = array[idx + 6];
            I[7] = array[idx + 7];
        }
        var start = System.currentTimeMillis();
        for (int i = 0; i < iterInternal; i++) {
            for (int idx = 0; idx < end; idx += blockSize) {
                // System.Buffer.BlockCopy(array, idx, I, 0, blockSizeBytes);
                I[0] = array[idx];
                I[1] = array[idx + 1];
                I[2] = array[idx + 2];
                I[3] = array[idx + 3];
                I[4] = array[idx + 4];
                I[5] = array[idx + 5];
                I[6] = array[idx + 6];
                I[7] = array[idx + 7];
            }
        }
        var elapsed = System.currentTimeMillis() - start;

        return new SimpleEntry<Double, long[]>((double) (iterInternal * array.length * 8.0 / (elapsed / 1000.0) / 1024 / 1024), I);
    }

}