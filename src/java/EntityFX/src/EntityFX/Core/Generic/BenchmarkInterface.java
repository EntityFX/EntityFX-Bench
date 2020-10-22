package EntityFX.Core.Generic;

import java.io.IOException;

public interface BenchmarkInterface
{
    String getName();

    BenchResult bench() throws IOException;

    void warmup(Double aspect) throws IOException;
}