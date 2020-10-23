package EntityFX.Core.Scimark2;

public class LU {

    public static double num_flops(int N) {
        double nd = (double) N;
        return (((2.0 * nd * nd) * nd) / 3.0);
    }

    protected static double[] new_copy(double[] x) {
        int N = x.length;
        double[] T = new double[N];
        for (int i = 0; i < N; i++) {
            T[i] = x[i];
        }
        return T;
    }

    protected static double[][] new_copy(double[][] A) {
        int M = A.length;
        int N = A[0].length;
        double[][] T = new double[M][N];
        for (int i = 0; i < M; i++) {
            double[] ti = T[i];
            double[] ai = A[i];
            for (int j = 0; j < N; j++) {
                ti[j] = ai[j];
            }
        }
        return T;
    }

    public static int[] new_copy(int[] x) {
        int N = x.length;
        int[] T = new int[N];
        for (int i = 0; i < N; i++) {
            T[i] = x[i];
        }
        return T;
    }

    protected static void insert_copy(double[][] B, double[][] A) {
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

    public double[][] getLU() {
        return new_copy(lU_);
    }

    public int[] getPivot() {
        return new_copy(pivot_);
    }

    public LU(double[][] A) {
        int M = A.length;
        int N = A[0].length;
        lU_ = new double[M][N];
        insert_copy(lU_, A);
        pivot_ = new int[M];
        factor(lU_, pivot_);
    }

    public double[] solve(double[] b) {
        double[] x = new_copy(b);
        solve(lU_, pivot_, x);
        return x;
    }

    public static int factor(double[][] A, int[] pivot) {
        int N = A.length;
        int M = A[0].length;
        int minMN = Math.min(M, N);
        for (int j = 0; j < minMN; j++) {
            int jp = j;
            double t = Math.abs(A[j][j]);
            for (int i = j + 1; i < M; i++) {
                double ab = Math.abs(A[i][j]);
                if (ab > t) {
                    jp = i;
                    t = ab;
                }
            }
            pivot[j] = jp;
            if (A[jp][j] == 0)
                return 1;
            if (jp != j) {
                double[] tA = A[j];
                A[j] = A[jp];
                A[jp] = tA;
            }
            if (j < (M - 1)) {
                double recp = 1.0 / A[j][j];
                for (int k = j + 1; k < M; k++) {
                    A[k][j] *= recp;
                }
            }
            if (j < (minMN - 1)) {
                for (int ii = j + 1; ii < M; ii++) {
                    double[] aii = A[ii];
                    double[] aj = A[j];
                    double aiiJ = aii[j];
                    for (int jj = j + 1; jj < N; jj++) {
                        aii[jj] -= (aiiJ * aj[jj]);
                    }
                }
            }
        }
        return 0;
    }

    public static void solve(double[][] lU, int[] pvt, double[] b) {
        int M = lU.length;
        int N = lU[0].length;
        int ii = 0;
        for (int i = 0; i < M; i++) {
            int ip = pvt[i];
            double sum = b[ip];
            b[ip] = b[i];
            if (ii == 0) {
                for (int j = ii; j < i; j++) {
                    sum -= (lU[i][j] * b[j]);
                }
            } else if (sum == .0)
                ii = i;
            b[i] = sum;
        }
        for (int i = N - 1; i >= 0; i--) {
            double sum = b[i];
            for (int j = i + 1; j < N; j++) {
                sum -= (lU[i][j] * b[j]);
            }
            b[i] = sum / lU[i][i];
        }
    }

    private double[][] lU_;

    private int[] pivot_;

    public LU() {
    }
}
