package EntityFX.Core.Generic;

public abstract class BenchmarkBase {

    protected int Iterrations;

    public static double DebugAspectRatio = 0.1;

    public double Ratio = 1.0;

    public String Name = "";

    public BenchmarkBase() {
        super();
        this.Name = this.getClass().getSimpleName();
    }

    public void bench() {
        System.out.println("Bench");
    }
}