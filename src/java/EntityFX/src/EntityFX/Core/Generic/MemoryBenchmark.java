package EntityFX.Core.Generic;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;

public class MemoryBenchmark extends MemoryBenchmarkBase<MemoryBenchmarkResult> {
    public MemoryBenchmark(Writer writer, boolean printToConsole) throws FileNotFoundException {
        super(writer, printToConsole);
        // TODO Auto-generated constructor stub
    }

    @Override
    public MemoryBenchmarkResult benchImplementation() throws IOException {
        return BenchRandomMemory();
    }

    @Override
    public BenchResult populateResult(BenchResult benchResult, MemoryBenchmarkResult memoryBenchmarkResult) {
        benchResult.Points = memoryBenchmarkResult.Average * Ratio;
        benchResult.Result = memoryBenchmarkResult.Average;
        benchResult.Units = "MB/s";
        benchResult.Output = memoryBenchmarkResult.Output;
        return benchResult;
    }
}