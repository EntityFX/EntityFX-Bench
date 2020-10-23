package EntityFX.Core.Scimark2;


public class SOR {

    public static double num_flops(int M, int N, int num_iterations) {
        double md = (double)M;
        double nd = (double)N;
        double num_iterD = (double)num_iterations;
        return (((md - (1.0))) * ((nd - (1.0))) * num_iterD) * 6.0;
    }

    public static void execute(double omega, double[][] G, int num_iterations) {
        int M = G.length;
        int N = G[0].length;
        double omega_over_four = omega * .25;
        double one_minus_omega = 1.0 - omega;
        int mm1 = M - 1;
        int nm1 = N - 1;
        for (int p = 0; p < num_iterations; p++) {
            for (int i = 1; i < mm1; i++) {
                double[] gi = G[i];
                double[] gim1 = G[i - 1];
                double[] gip1 = G[i + 1];
                for (int j = 1; j < nm1; j++) {
                    gi[j] = (omega_over_four * (((gim1[j] + gip1[j] + gi[j - 1]) + gi[j + 1]))) + (one_minus_omega * gi[j]);
                }
            }
        }
    }
    public SOR() {
    }
}
