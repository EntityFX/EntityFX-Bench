<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    class HashBenchmark extends HashBase {

        protected function BenchImplementation() {
            $result = "";

            for ($i = 0; $i < $this->Iterrations; $i++)
            {
                $result = self::DoHash($i);
            }
            return $result;
        }
    }
}