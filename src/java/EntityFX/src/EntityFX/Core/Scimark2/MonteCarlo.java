package EntityFX.Core.Scimark2;

import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

public class MonteCarlo {

    private static final int sEED = 113;

    public static double num_flops(int num_samples) {
        return (((double)num_samples)) * 4.0;
    }

    public static double integrate(int num_samples) {
        Random R = ThreadLocalRandom.current();
        int under_curve = 0;
        for (int count = 0; count < num_samples; count++) {
            double x = R.nextDouble();
            double y = R.nextDouble();
            if (((x * x) + (y * y)) <= 1.0) 
                under_curve++;
        }
        return ((((double)under_curve) / ((double)num_samples))) * 4.0;
    }
    public MonteCarlo() {
    }
}
