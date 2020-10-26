package EntityFX.Core.Scimark2;

import java.io.FileNotFoundException;
import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

import EntityFX.Core.Writer;

public class Scimark2 {

    /* error value type 'Writer' of field output */
    private Writer output;

    public Scimark2(Writer writer, boolean printToConsole) throws FileNotFoundException {
        output = new Writer(null);
        output.UseConsole = printToConsole;
    }

    public Scimark2Result bench(double min_time, boolean isLarge) throws Exception {
        int fFT_size = Constants.FFT_SIZE;
        int sOR_size = Constants.SOR_SIZE;
        int sparse_size_M = Constants.SPARSE_SIZE_M;
        int sparse_size_nz = Constants.SPARSE_SIZE_NZ;
        int lU_size = Constants.LU_SIZE;
        int current_arg = 0;
        if (isLarge) {
            fFT_size = Constants.LG_FFT_SIZE;
            sOR_size = Constants.LG_SOR_SIZE;
            sparse_size_M = Constants.LG_SPARSE_SIZE_M;
            sparse_size_nz = Constants.LG_SPARSE_SIZE_NZ;
            lU_size = Constants.LG_LU_SIZE;
            current_arg++;
        }
        double[] res = new double[6];
        Random R = ThreadLocalRandom.current();

        res[1] = Kernel.measureFFT(fFT_size, min_time, R);
        res[2] = Kernel.measureSOR(sOR_size, min_time, R);
        res[3] = Kernel.measureMonteCarlo(min_time, R);
        res[4] = Kernel.measureSparseMatmult(sparse_size_M, sparse_size_nz, min_time, R);
        res[5] = Kernel.measureLU(lU_size, min_time, R);
        res[0] = (((res[1] + res[2] + res[3]) + res[4] + res[5])) / (5.0);
        output.writeLine() /* error */;
        output.writeLine("SciMark 2.0a") /* error */;
        output.writeLine() /* error */;
        output.writeLine("Composite Score: %.2f", res[0]) /* error */;
        output.write("FFT (%d): ", fFT_size) /* error */;
        if (res[1] == .0)
            output.writeLine(" ERROR, INVALID NUMERICAL RESULT!") /* error */;
        else
            output.writeLine("%.2f", res[1]) /* error */;
        output.writeLine("SOR (%dx%d):   %.2f", sOR_size, sOR_size, res[2]) /* error */;
        output.writeLine("Monte Carlo : %.2f", res[3]) /* error */;
        output.writeLine("Sparse matmult (N=%d, nz=%d): %.2f", sparse_size_M, sparse_size_nz, res[4]) /* error */;
        output.write("LU (%dx%d): ", lU_size, lU_size) /* error */;
        if (res[5] == .0)
            output.writeLine(" ERROR, INVALID NUMERICAL RESULT!") /* error */;
        else
            output.writeLine("%.2f", res[5]) /* error */;
        return Scimark2Result._new1(res[0], res[1], res[2], res[3], res[4], res[5], output.Output);
    }
}