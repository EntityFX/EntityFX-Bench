<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {
    require_once("Constants.php");
    require_once("FFT.php");
    require_once("SOR.php");
    require_once("MonteCarlo.php");
    require_once("SparseCompRow.php");
    require_once("LU.php");    
    require_once("Kernel.php");

    use EntityFX\NetBenchmark\Core\Writer;

    class Scimark2 {

        private $output;

        public function __construct($printToConsole = true)
        {
            $output = new Writer();
            $output->UseConsole = $printToConsole;
        }

        public function Bench($min_time = Constants::RESOLUTION_DEFAULT, $isLarge = false) {
            // default to the (small) cache-contained version

            $FFT_size = Constants::FFT_SIZE;
            $SOR_size = Constants::SOR_SIZE;
            $Sparse_size_M = Constants::SPARSE_SIZE_M;
            $Sparse_size_nz = Constants::SPARSE_SIZE_nz;
            $LU_size = Constants::LU_SIZE;

            // look for runtime options

            $current_arg = 0;
            if ($isLarge)
            {
                $FFT_size = Constants::LG_FFT_SIZE;
                $SOR_size = Constants::LG_SOR_SIZE;
                $Sparse_size_M = Constants::LG_SPARSE_SIZE_M;
                $Sparse_size_nz = Constants::LG_SPARSE_SIZE_nz;
                $LU_size = Constants::LG_LU_SIZE;

                $current_arg++;
            }


            // run the benchmark

            $res = [];

            $res[1] = kernel::measureFFT($FFT_size, $min_time);
            $res[2] = kernel::measureSOR($SOR_size, $min_time);
            $res[3] = kernel::measureMonteCarlo($min_time);
            $res[4] = kernel::measureSparseMatmult($Sparse_size_M,
                        $Sparse_size_nz, $min_time);
            $res[5] = kernel::measureLU($LU_size, $min_time);


            $res[0] = ($res[1] + $res[2] + $res[3] + $res[4] + $res[5]) / 5;


            // print out results

            $output->WriteNewLine();
            $output->WriteLine("SciMark 2.0a");
            $output->WriteLine();
            $output->WriteLine("Composite Score: %2f", $res[0]);
            $output->Write("FFT ({%d}): ", $FFT_size);
            if ($res[1] == 0.0)
                $output->WriteLine(" ERROR, INVALID NUMERICAL RESULT!");

            else
                $output->WriteLine("%2f", res[1]);

            $output->WriteLine("SOR (%dx%d):   %2f", $SOR_size, $SOR_size, $res[2]);
            $output->WriteLine("Monte Carlo : %2f", $res[3]);
            $output->WriteLine("Sparse matmult (N=%d, nz=%d): %2f", $Sparse_size_M, $Sparse_size_nz, $res[4]);
            $output->Write("LU (%dx%d): ", $LU_size, $LU_size);
            if ($res[5] == 0.0)
                $output->WriteLine(" ERROR, INVALID NUMERICAL RESULT!");

            else
                $output->WriteLine("%2f", $res[5]);

            return [
                "CompositeScore" => $res[0],
                "FFT" => $res[1],
                "SOR" => $res[2],
                "MonteCarlo" => $res[3],
                "SparseMathmult" => $res[4],
                "LU" => $res[5],
                "Output" => ""
            ];
        }
    }
}