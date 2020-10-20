<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    class MathBenchmark extends MathBase {

        protected function BenchImplementation()
        {
            $R = 0;
            $li = 0.0;

            for ($i = 0; $i < $this->Iterrations; $i++)
            {
                $R += self::DoMath($i);
            }
            return $R;
        }
    }
}