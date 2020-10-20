<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    abstract class HashBase extends BenchmarkBase {
        protected static $strs;

        public function __construct($writer, $printToConsole) {
            self::$strs = [
                "the quick brown fox jumps over the lazy dog", 
                "Some red wine", 
                "Candels & Ropes" ];
    

            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 2000000;
            $this->Ratio = 10;
        }

        public static function DoHash($i) {
            $sha = hash("sha1", self::$strs[$i % 3]);
            $sha256 = hash("sha256", self::$strs[($i + 1) % 3]);
            return $sha . $sha256;
        }
    }
}