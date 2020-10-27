package EntityFX.Core.Generic;

import EntityFX.Core.Writer;

public abstract class BenchmarkBaseBase {

    protected int Iterrations;

    protected boolean printToConsole = true;

    protected boolean isParallel = false;

    public static double IterrationsRatio = 1;

    protected double Ratio = 1.0;

    protected String Name = "";

    protected Writer output;
}