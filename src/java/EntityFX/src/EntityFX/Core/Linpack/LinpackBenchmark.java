package EntityFX.Core.Linpack;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;
import EntityFX.Core.Generic.BenchResult;
import EntityFX.Core.Generic.BenchmarkBase;

public class LinpackBenchmark extends BenchmarkBase<LinpackResult> {

    private Linpack linpack;

    public LinpackBenchmark(final Writer writer, final boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        linpack = new Linpack(printToConsole);
        Ratio = 10;
    }

    @Override
    public LinpackResult benchImplementation() throws IOException {
        return linpack.run_benchmark(2000);
    }

    /* error value type 'BenchResult' of method PopulateResult */
    @Override
    public BenchResult populateResult(final BenchResult benchResult, final LinpackResult linpackResult) {
        benchResult.Result = linpackResult.MFLOPS;
        benchResult.Points = linpackResult.MFLOPS * this.Ratio;
        benchResult.Units = "MFLOPS";
        benchResult.Output = linpackResult.output;
        return benchResult;
    }

    @Override
    public void warmup(final Double aspect) {
    }
}
