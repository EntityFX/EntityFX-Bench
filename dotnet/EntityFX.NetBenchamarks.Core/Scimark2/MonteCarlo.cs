namespace EntityFX.NetBenchmark.Core.Scimark2
{
    internal class MonteCarlo
    {
        const int SEED = 113;

        public static double num_flops(int Num_samples)
        {
            // 3 flops in x^2+y^2 and 1 flop in random routine

            return ((double)Num_samples) * 4.0;

        }



        public static double integrate(int Num_samples)
        {

            Random R = new Random(SEED);


            int under_curve = 0;
            for (int count = 0; count < Num_samples; count++)
            {
                double x = R.nextDouble();
                double y = R.nextDouble();

                if (x * x + y * y <= 1.0)
                    under_curve++;

            }

            return ((double)under_curve / Num_samples) * 4.0;
        }


    }
}