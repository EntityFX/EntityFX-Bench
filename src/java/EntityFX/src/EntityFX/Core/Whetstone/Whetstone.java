package EntityFX.Core.Whetstone;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;

public class Whetstone {

    private double[] loop_time = new double[9];

    private float[] loop_mops = new float[9];

    private float[] loop_mflops = new float[9];

    private double timeUsed;

    private double mwips;

    private String[] headings = new String[9];

    private float check;

    private float[] results = new float[9];

    private Writer output;

    public Whetstone(boolean printToConsole) throws FileNotFoundException {
        output = new Writer(null);
        output.UseConsole = printToConsole;
    }

    public WhetstoneResult bench(boolean getinput) throws IOException {
        int count = 10;
        int calibrate = 1;
        long xtra = 1L;
        int endit;
        int section;
        long x100 = 100L;
        int duration = 100;
        String[] general = new String[8];
        this.writeLine("{0} Precision Java Whetstone Benchmark\n", "Double");
        if (!getinput)
            this.writeLine("No run time input data\n");
        else
            this.writeLine("With run time input data\n");
        this.writeLine("Calibrate");
        do {
            timeUsed = 0.0;
            this.whetstones(xtra, x100, calibrate);
            this.writeLine("%11.2f Seconds %10d   Passes (x 100)", timeUsed, xtra);
            calibrate = calibrate + 1;
            count = count - 1;
            if (timeUsed > 2.0)
                count = 0;
            else
                xtra = xtra * (5L);
        } while (count > 0);
        if (timeUsed > 0)
            xtra = (long) ((((double) ((float) ((((long) duration) * xtra)))) / timeUsed));
        if (xtra < (1L))
            xtra = 1L;
        calibrate = 0;
        this.writeLine("\nUse %d  passes (x 100)", xtra);
        this.writeLine("\n          %s Precision C# Whetstone Benchmark", "Double");
        this.writeLine("\n                  %s", "");
        this.writeLine("\nLoop content                  Result              MFLOPS " + "     MOPS   Seconds\n");
        timeUsed = 0.0;
        this.whetstones(xtra, x100, calibrate);
        output.write("MWIPS            ");
        if (timeUsed > 0)
            mwips = (double) ((((float) ((xtra))) * ((float) ((x100)))) / (((10.0F) * ((float) timeUsed))));
        else
            mwips = 0.0;
        output.writeLine("%39.3f%19.3f", mwips, timeUsed);
        if (check == 0)
            this.writeLine("Wrong answer  ");
        return WhetstoneResult._new1(output.toString(), mwips, timeUsed);
    }

    private void whetstones(long xtra, long x100, int calibrate) throws IOException {
        long n1;
        long n2;
        long n3;
        long n4;
        long n5;
        long n6;
        long n7;
        long n8;
        long i;
        long ix;
        long n1mult;
        float x;
        float y;
        float z;
        long j;
        long k;
        long l;
        float[] e1 = new float[4];
        double timea;
        double timeb;
        float t = .49999975f;
        float t0 = t;
        float t1 = .50000025f;
        float t2 = 2.0F;
        check = 0.0F;
        n1 = (12L) * x100;
        n2 = (14L) * x100;
        n3 = (345L) * x100;
        n4 = (210L) * x100;
        n5 = (32L) * x100;
        n6 = (899L) * x100;
        n7 = (616L) * x100;
        n8 = (93L) * x100;
        n1mult = 10L;
        e1[0] = 1.0F;
        e1[1] = -1.0F;
        e1[2] = -1.0F;
        e1[3] = -1.0F;
        long start = System.currentTimeMillis();

        timea = (double) start / 1000;
        for (ix = 0L; ix < xtra; ix++) {
            for (i = 0L; i < (n1 * n1mult); i++) {
                e1[0] = (((e1[0] + e1[1] + e1[2]) - e1[3])) * t;
                e1[1] = ((((e1[0] + e1[1]) - e1[2]) + e1[3])) * t;
                e1[2] = (((e1[0] - e1[1]) + e1[2] + e1[3])) * t;
                e1[3] = ((((-e1[0]) + e1[1] + e1[2]) + e1[3])) * t;
            }
            t = (1.0F) - t;
        }
        t = t0;
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea)) / ((double) n1mult);
        this.pout("N1 floating point", ((float) ((n1 * (16L)))) * ((float) ((xtra))), 1, e1[3], timeb, calibrate, 1);
        timea = (double) start / 1000;
        {
            for (ix = 0L; ix < xtra; ix++) {
                for (i = 0L; i < n2; i++) {
                    pa(e1, t, t2);
                }
                t = (1.0F) - t;
            }
            t = t0;
        }
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea));
        this.pout("N2 floating point", ((float) ((n2 * (96L)))) * ((float) ((xtra))), 1, e1[3], timeb, calibrate, 2);
        j = 1L;
        timea = (double) start / 1000;
        {
            for (ix = 0L; ix < xtra; ix++) {
                for (i = 0L; i < n3; i++) {
                    if (j == (1L))
                        j = 2L;
                    else
                        j = 3L;
                    if (j > (2L))
                        j = 0L;
                    else
                        j = 1L;
                    if (j < (1L))
                        j = 1L;
                    else
                        j = 0L;
                }
            }
        }
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea));
        this.pout("N3 if then else  ", ((float) ((n3 * (3L)))) * ((float) ((xtra))), 2, (float) ((j)), timeb, calibrate,
                3);
        j = 1L;
        k = 2L;
        l = 3L;
        timea = (double) start / 1000;
        {
            for (ix = 0L; ix < xtra; ix++) {
                for (i = 0L; i < n4; i++) {
                    j = j * ((k - j)) * ((l - k));
                    k = (l * k) - (((l - j)) * k);
                    l = ((l - k)) * ((k + j));
                    e1[(int) (l - (2L))] = (float) (j + k + l);
                    e1[(int) (k - (2L))] = (float) (j * k * l);
                }
            }
        }
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea));
        x = e1[0] + e1[1];
        this.pout("N4 fixed point   ", ((float) ((n4 * (15L)))) * ((float) ((xtra))), 2, x, timeb, calibrate, 4);
        x = .5f;
        y = .5f;
        timea = (double) start / 1000;
        {
            for (ix = 0L; ix < xtra; ix++) {
                for (i = 1L; i < n5; i++) {
                    x = (float) ((((double) t) * Math.atan((((double) t2) * Math.sin((double) x) * Math.cos((double) x))
                            / (((Math.cos((double) (x + y)) + Math.cos((double) (x - y))) - 1.0)))));
                    y = (float) ((((double) t) * Math.atan((((double) t2) * Math.sin((double) y) * Math.cos((double) y))
                            / (((Math.cos((double) (x + y)) + Math.cos((double) (x - y))) - 1.0)))));
                }
                t = (1.0F) - t;
            }
            t = t0;
        }
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea));
        this.pout("N5 sin,cos etc.  ", ((float) ((n5 * (26L)))) * ((float) ((xtra))), 2, y, timeb, calibrate, 5);
        x = 1.0F;
        y = 1.0F;
        z = 1.0F;
        timea = (double) start / 1000;
        {
            for (ix = 0L; ix < xtra; ix++) {
                for (i = 0L; i < n6; i++) {
                    Float wrapx2 = x;
                    Float wrapy3 = y;
                    Float wrapz4 = z;
                    p3(wrapx2, wrapy3, wrapz4, t, t1, t2);
                    x = (wrapx2 != null ? wrapx2 : 0);
                    y = (wrapy3 != null ? wrapy3 : 0);
                    z = (wrapz4 != null ? wrapz4 : 0);
                }
            }
        }
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea));
        this.pout("N6 floating point", ((float) ((n6 * (6L)))) * ((float) ((xtra))), 1, z, timeb, calibrate, 6);
        j = 0L;
        k = 1L;
        l = 2L;
        e1[0] = 1.0F;
        e1[1] = 2.0F;
        e1[2] = 3.0F;
        timea = (double) start / 1000;
        {
            for (ix = 0L; ix < xtra; ix++) {
                for (i = 0L; i < n7; i++) {
                    po(e1, j, k, l);
                }
            }
        }
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea));
        this.pout("N7 assignments   ", ((float) ((n7 * (3L)))) * ((float) ((xtra))), 2, e1[2], timeb, calibrate, 7);
        x = .75f;
        timea = (double) start / 1000;
        {
            for (ix = 0L; ix < xtra; ix++) {
                for (i = 0L; i < n8; i++) {
                    x = (float) Math.sqrt(Math.exp(Math.log((double) x) / ((double) t1)));
                }
            }
        }
        timeb = ((((double) System.currentTimeMillis() / 1000) - timea));
        this.pout("N8 exp,sqrt etc. ", ((float) ((n8 * (4L)))) * ((float) ((xtra))), 2, x, timeb, calibrate, 8);
    }

    private static void pa(float[] e, float t, float t2) {
        long j;
        for (j = 0L; j < (6L); j++) {
            e[0] = (((e[0] + e[1] + e[2]) - e[3])) * t;
            e[1] = ((((e[0] + e[1]) - e[2]) + e[3])) * t;
            e[2] = (((e[0] - e[1]) + e[2] + e[3])) * t;
            e[3] = ((((-e[0]) + e[1] + e[2]) + e[3])) / t2;
        }
        return;
    }

    private static void po(float[] e1, long j, long k, long l) {
        e1[(int) j] = e1[(int) k];
        e1[(int) k] = e1[(int) l];
        e1[(int) l] = e1[(int) j];
        return;
    }

    private static void p3(Float x, Float y, Float z, float t, float t1, float t2) {
        x = y;
        y = z;
        x = t * ((x + y));
        y = t1 * ((x + y));
        z = ((x + y)) / t2;
        return;
    }

    private void pout(String title, float ops, int type, float checknum, double time, int calibrate, int section)
            throws IOException {
        float mops;
        float mflops;
        check = check + checknum;
        loop_time[section] = time;
        headings[section] = title;
        timeUsed = timeUsed + time;
        if (calibrate == 1)
            results[section] = checknum;
        if (calibrate == 0) {
            this.write("%-18s %24.17f    ", headings[section], results[section]);
            if (type == 1) {
                if (time > 0)
                    mflops = ops / (((1000000.0F) * ((float) time)));
                else
                    mflops = 0.0F;
                loop_mops[section] = 99999.0F;
                loop_mflops[section] = mflops;
                this.writeLine("%9.3f           %9.3f", loop_mflops[section], loop_time[section]);
            } else {
                if (time > 0)
                    mops = ops / (((1000000.0F) * ((float) time)));
                else
                    mops = 0.0F;
                loop_mops[section] = mops;
                loop_mflops[section] = 0.0F;
                this.writeLine("           %9.3f%9.3f", loop_mops[section], loop_time[section]);
            }
        }
        return;
    }

    private void writeLine(String text, Object... args) throws IOException {
        output.writeLine(text, args);
    }

    private void write(String text, Object... args) throws IOException {
        output.write(text, args);
    }
}
