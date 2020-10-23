package EntityFX.Core.Scimark2;


public class SparseCompRow {

    public static double num_flops(int N, int nz, int num_iterations) {
        int actual_nz = ((nz / N)) * N;
        return (((double)actual_nz)) * 2.0 * (((double)num_iterations));
    }

    public static void matmult(double[] y, double[] val, int[] row, int[] col, double[] x, int nUM_ITERATIONS) {
        int M = row.length - 1;
        for (int reps = 0; reps < nUM_ITERATIONS; reps++) {
            for (int r = 0; r < M; r++) {
                double sum = .0;
                int rowR = row[r];
                int rowRp1 = row[r + 1];
                for (int i = rowR; i < rowRp1; i++) {
                    sum += (x[col[i]] * val[i]);
                }
                y[r] = sum;
            }
        }
    }
    public SparseCompRow() {
    }
}
