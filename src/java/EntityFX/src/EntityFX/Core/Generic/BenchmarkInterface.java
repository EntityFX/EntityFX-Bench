package EntityFX.Core.Generic;

import java.io.IOException;

public interface BenchmarkInterface
{
    String getName();

    BenchResult bench() throws IOException, Exception;

    void warmup(Double aspect) throws IOException, Exception;
    
}