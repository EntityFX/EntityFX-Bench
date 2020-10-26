package EntityFX.Core.Whetstone;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class WhetstoneBenchmark extends BenchmarkBase<WhetstoneResult> {

    private Whetstone whetstone;

    public WhetstoneBenchmark(final Writer writer, final boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        whetstone = new Whetstone(printToConsole);
        Ratio = 1;
    }

    @Override
    public WhetstoneResult benchImplementation() throws IOException {
        return whetstone.bench(true);
    }

    /* error value type 'BenchResult' of method PopulateResult */
    @Override
    public BenchResult populateResult(final BenchResult benchResult, final WhetstoneResult dhrystoneResult) {
        benchResult.Result = dhrystoneResult.getMWIPS();
        benchResult.Points = dhrystoneResult.getMWIPS() * this.Ratio;
        benchResult.Units = "MWIPS";
        benchResult.Output = dhrystoneResult.getOutput();
        return benchResult;
    }

    @Override
    public void warmup(final Double aspect) {
    }
}
