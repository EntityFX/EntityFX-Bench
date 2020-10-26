package EntityFX.Core.Scimark2;

import java.io.FileNotFoundException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class Scimark2Benchmark extends BenchmarkBase<Scimark2Result> {

    /* error value type 'Scimark2.Scimark2' of field scimark2 */
    private Scimark2 scimark2 = new Scimark2(output, printToConsole);

    public Scimark2Benchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        Ratio = 10;
    }

    @Override
    public Scimark2Result benchImplementation() throws Exception {
        return scimark2.bench(Constants.RESOLUTION_DEFAULT, false);
    }

    /* error value type 'BenchResult' of method PopulateResult */
    @Override
    public BenchResult populateResult(BenchResult benchResult,
            Scimark2Result dhrystoneResult) {
        benchResult.Result = dhrystoneResult.getCompositeScore();
        benchResult.Points = dhrystoneResult.getCompositeScore() * Ratio;
        benchResult.Units = "CompositeScore";
        benchResult.Output = dhrystoneResult.getOutput();
        return benchResult;
    }

    @Override
    public void warmup(Double aspect) {
    }
}
