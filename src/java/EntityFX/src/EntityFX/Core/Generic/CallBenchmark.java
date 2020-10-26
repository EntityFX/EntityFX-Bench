package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;

public class CallBenchmark extends BenchmarkBase<Float> {
    public CallBenchmark(final Writer writer, final boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Iterrations = 2000000000;
        Ratio = 0.01;
    }

    private static float doCall(float i, float b)
    {
        float z = i * 0.7f;
        float z1 = i * b;
        return  z + z1 + 0.5f;
    }

    public BenchResult bench() throws IOException {
        this.beforeBench();
        long start = System.currentTimeMillis();
        long elapsed1, elapsed2 = 0;
        int i = 0;
        Float a = 0.0f;

        for (i = 0; i < Iterrations; ++i)
        {
            float z = a * 0.7f;
            float z1 = a * 0.01f;
            a = z + z1 + 0.5f;
        }
        elapsed1 = System.currentTimeMillis() - start;
        a = 0.0f;
        i = 0;
        start = System.currentTimeMillis();
        for (i = 0; i < Iterrations; ++i)
        {
            a = doCall(a, 0.01f);
        }
        elapsed2 = System.currentTimeMillis() - start;

        output.write("Elapsed No Call: %d", elapsed1);
        output.writeLine();
        output.write("Elapsed Call: %d", elapsed2);
        output.writeLine();
        long callTime = 0;

        if (elapsed2 <= elapsed1) {
            callTime = elapsed1 - elapsed2;
        } else {
            callTime = elapsed2 - elapsed1;
        }

        long callStart = System.currentTimeMillis() - callTime;

        final BenchResult result = this.populateResult(this.buildResult(callStart), a);
        doOutput(result);
        this.afterBench(result);
        return result;
    }

    @Override
    public Float benchImplementation() throws IOException {
        return null;
    }
}