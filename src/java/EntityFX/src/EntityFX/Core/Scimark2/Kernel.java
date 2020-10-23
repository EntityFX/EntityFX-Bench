package EntityFX.Core.Scimark2;

import java.util.Random;

public class Kernel {

    public static double measureFFT(int N, double mintime, Random R) throws Exception {
        double[] x = randomVector(2 * N, R);
        double[] oldx = newVectorCopy(x);
        long cycles = 1L;
        double elapsed = 0.0;
        while (true) {
            double start = System.currentTimeMillis() / 1000.0;
            for (int i = 0; i < cycles; i++) {
                FFT.transform(x);
                FFT.inverse(x);
            }
            elapsed = System.currentTimeMillis() / 1000.0 - start;
            if (elapsed >= mintime)
                break;
            cycles *= (2L);
        }
        double EPS = 1.0e-10;
        if ((FFT.test(x) / ((double) N)) > EPS)
            return .0;
        return FFT.num_flops(N) * cycles / elapsed * 1.0e-6;
    }

    public static double measureSOR(int N, double min_time, Random R) {
        double[][] G = randomMatrix(N, N, R);
        double elapsed = 0.0;
        int cycles = 1;
        while (true) {
            double start = System.currentTimeMillis() / 1000.0;
            SOR.execute(1.25, G, cycles);
            elapsed = System.currentTimeMillis() / 1000.0 - start;
            if (elapsed >= min_time) 
                break;
            cycles *= 2;
        }
        return SOR.num_flops(N, N, cycles) / elapsed * 1.0e-6;

    }

    public static double measureMonteCarlo(double min_time, Random R) {
        double elapsed = 0.0;
        int cycles = 1;
        while (true) {
            double start = System.currentTimeMillis() / 1000.0;
            MonteCarlo.integrate(cycles);
            elapsed = System.currentTimeMillis() / 1000.0 - start;
            if (elapsed >= min_time) 
                break;
            cycles *= 2;
        }
        return MonteCarlo.num_flops(cycles) / elapsed * 1.0e-6;
    }

    public static double measureSparseMatmult(int N, int nz, double min_time, Random R) {
        double[] x = randomVector(N, R);
        double[] y = new double[N];
        int nr = nz / N;
        int anz = nr * N;
        double[] val = randomVector(anz, R);
        int[] col = new int[anz];
        int[] row = new int[N + 1];
        row[0] = 0;
        for (int r = 0; r < N; r++) {
            int rowr = row[r];
            row[r + 1] = rowr + nr;
            int step = r / nr;
            if (step < 1) 
                step = 1;
            for (int i = 0; i < nr; i++) {
                col[rowr + i] = i * step;
            }
        }
        double elapsed = 0.0;
        int cycles = 1;
        while (true) {
            double start = System.currentTimeMillis() / 1000.0;
            SparseCompRow.matmult(y, val, row, col, x, cycles);
            elapsed = System.currentTimeMillis() / 1000.0 - start;
            if (elapsed >= min_time) 
                break;
            cycles *= 2;
        }
        return SparseCompRow.num_flops(N, nz, cycles) / elapsed * 1.0e-6;
  
    }

    public static double measureLU(int N, double min_time, Random R) {
        double[][] A = randomMatrix(N, N, R);
        double[][] lu = new double[N][N];
        for (int i = 0; i < N; i++) {
            lu[i] = new double[N];
        }
        int[] pivot = new int[N];
        double elapsed = 0.0;
        int cycles = 1;
        while (true) {
            double start = System.currentTimeMillis() / 1000.0;
            for (int i = 0; i < cycles; i++) {
                copyMatrix(lu, A);
                LU.factor(lu, pivot);
            }
            elapsed = System.currentTimeMillis() / 1000.0 - start;
            if (elapsed >= min_time)
                break;
            cycles *= 2;
        }
        double[] b = randomVector(N, R);
        double[] x = newVectorCopy(b);
        LU.solve(lu, pivot, x);
        double EPS = 1.0e-12;
        if ((normabs(b, matvec(A, x)) / ((double) N)) > EPS)
            return .0;
        return LU.num_flops(N) * cycles / elapsed * 1.0e-6;
    }

    private static double[] newVectorCopy(double[] x) {
        int N = x.length;
        double[] y = new double[N];
        for (int i = 0; i < N; i++) {
            y[i] = x[i];
        }
        return y;
    }

    private static void copyVector(double[] B, double[] A) {
        int N = A.length;
        for (int i = 0; i < N; i++) {
            B[i] = A[i];
        }
    }

    private static double normabs(double[] x, double[] y) {
        int N = x.length;
        double sum = .0;
        for (int i = 0; i < N; i++) {
            sum += Math.abs(x[i] - y[i]);
        }
        return sum;
    }

    private static void copyMatrix(double[][] B, double[][] A) {
        int M = A.length;
        int N = A[0].length;
        int remainder = N & 3;
        for (int i = 0; i < M; i++) {
            double[] bi = B[i];
            double[] ai = A[i];
            for (int j = 0; j < remainder; j++) {
                bi[j] = ai[j];
            }
            for (int j = remainder; j < N; j += 4) {
                bi[j] = ai[j];
                bi[j + 1] = ai[j + 1];
                bi[j + 2] = ai[j + 2];
                bi[j + 3] = ai[j + 3];
            }
        }
    }

    private static double[][] randomMatrix(int M, int N, Random R) {
        double[][] A = new double[M][N];
        for (int i = 0; i < N; i++) {
            A[i] = new double[N];
            for (int j = 0; j < N; j++) {
                A[i][j] = R.nextDouble();
            }
        }
        return A;
    }

    private static double[] randomVector(int N, Random R) {
        double[] A = new double[N];
        for (int i = 0; i < N; i++) {
            A[i] = R.nextDouble();
        }
        return A;
    }

    private static double[] matvec(double[][] A, double[] x) {
        int N = x.length;
        double[] y = new double[N];
        matvec(A, x, y);
        return y;
    }

    private static void matvec(double[][] A, double[] x, double[] y) {
        int M = A.length;
        int N = A[0].length;
        for (int i = 0; i < M; i++) {
            double sum = .0;
            double[] ai = A[i];
            for (int j = 0; j < N; j++) {
                sum += (ai[j] * x[j]);
            }
            y[i] = sum;
        }
    }

    public Kernel() {
    }
}