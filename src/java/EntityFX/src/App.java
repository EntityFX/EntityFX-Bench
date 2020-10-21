import EntityFX.Core.Generic.ArithmeticsBase;
import EntityFX.Core.Generic.BenchmarkBase;

public class App {
    public static void main(String[] args) throws Exception {
        BenchmarkBase benchmarkBase = new ArithmeticsBase();
        benchmarkBase.bench();
    }
}
