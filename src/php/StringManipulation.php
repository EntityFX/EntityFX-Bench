<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    class StringManipulation  extends StringManipulationBase {

        protected function BenchImplementation()
        {
            $str = "the quick brown fox jumps over the lazy dog";
            $str1 = "";
            for ($i = 0; $i < $this->Iterrations; $i++)
            {
                $str1 = self::DoStringManipilation($str);
            }
            return $str1;
        }
    }
}