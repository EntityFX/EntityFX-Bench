package EntityFX.Core.Dhrystone;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class DhrystoneBenchmark extends BenchmarkBase<DhrystoneResult> {

    private Dhrystone2 dhrystone = new Dhrystone2(true);

    public DhrystoneBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Ratio = 4;
    }

    @Override
    public DhrystoneResult benchImplementation() throws IOException {
        return dhrystone.Bench(Dhrystone2.LOOPS);
    }

    @Override
    public BenchResult populateResult(BenchResult benchResult, DhrystoneResult dhrystoneResult) {
        benchResult.Result = dhrystoneResult.VaxMips;
        benchResult.Points = dhrystoneResult.VaxMips * Ratio;
        benchResult.Units = "DMIPS";
        benchResult.Output = dhrystoneResult.Output;
        return benchResult;
    }

    @Override
    public void warmup(Double aspect) {
    }
}
