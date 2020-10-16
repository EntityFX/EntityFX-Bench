<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    class ArithmeticsBenchmark  extends ArithmeticsBase {

        protected function BenchImplementation()
        {
            parent::$R = 0;
            for ($i = 0; $i < $this->Iterrations; $i++)
            {
                self::$R += self::DoArithmetics($i);
            }
            return self::$R;
        }
    }
}