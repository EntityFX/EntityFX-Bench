<?php

namespace EntityFX\NetBenchmark\Core\Dhrystone
{
	class Record
	{
		/**
		 * @type Record
		 */
		public $PtrComp;
		public $Discr;
		public $EnumComp;
		public $IntComp;
		public $StringComp;
	}

	class Dhrystone2
    {
        const Ident_1 = 0;
        const Ident_2 = 1;
        const Ident_3 = 2;
        const Ident_4 = 3;
		const Ident_5 = 4;

		public const LOOPS = 20000000;
		
		/**
		 * @type CheckRecord
		 */
		private static $Check;

        private $IntGlob = 0;
        private $BoolGlob = FALSE;
        private $Char1Glob = '\0';
        private $Char2Glob = '\0';
        private $Array1Glob = [];
        private $Array2Glob = [[]];

        private $PtrGlbNext;
        private $PtrGlb;

		public function __construct(bool $printToConsole) {

		}

		static function structAssign(Record $destination, Record $source)
        {
            $destination->IntComp = $source->IntComp;
            $destination->PtrComp = $source->PtrComp;
            $destination->StringComp = $source->StringComp;
            $destination->EnumComp = $source->EnumComp;
            $destination->Discr = $source->Discr;
		}

        public function bench(int $loops)
        {
			$result = $this->proc0($loops);
			var_dump($result);
		}

        function proc0(int $loops)
        {
            $this->PtrGlbNext = new Record();
            $this->PtrGlb = new Record();
			$this->PtrGlb->PtrComp = $this->PtrGlbNext;
            $this->PtrGlb->Discr = self::Ident_1;
            $this->PtrGlb->EnumComp = self::Ident_3;
            $this->PtrGlb->IntComp = 40;
            $this->PtrGlb->StringComp = "DHRYSTONE PROGRAM, SOME STRING";
            $String1Loc = "DHRYSTONE PROGRAM, 1'ST STRING";
            $String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING";


            $this->IntGlob = 0;
            $this->BoolGlob = false;
            $this->Char1Glob = '\0';
            $this->Char2Glob = '\0';
            $this->Array1Glob = [];
			$this->Array2Glob = [[]];
			$length = count($this->Array2Glob);
            for ($i = 0; $i < $length; $i++)
            {
                $this->Array2Glob[$i] = [];
            }
            $this->Array2Glob[8][7] = 10;

            $EnumLoc = 0;
            $IntLoc1 = 0;
            $IntLoc2 = 0;
            $IntLoc3 = 0;

            $sw = microtime(true);

            for ($i = 0; $i < $loops; ++$i)
            {
                $this->proc5();
                $this->proc4();
                $IntLoc1 = 2;
                $IntLoc2 = 3;
                $IntLoc3 = 0;
                $String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING";
                $EnumLoc = self::Ident_2;
                $this->BoolGlob = !self::func2($String1Loc, $String2Loc);
                while ($IntLoc1 < $IntLoc2)
                {
                    $IntLoc3 = 5 * $IntLoc1 - $IntLoc2;
                    $IntLoc3 = self::proc7($IntLoc1, $IntLoc2);
                    $IntLoc1 += 1;
                }
                $this->proc8($this->Array1Glob, $this->Array2Glob, $IntLoc1, $IntLoc3);
                $PtrGlb = $this->proc1($this->PtrGlb);
                $CharIndex = 'A';

                for ($CharIndex = 'A'; $CharIndex <= $this->Char2Glob; ++$CharIndex)
                {
                    if ($EnumLoc == self::func1($CharIndex, 'C'))
                    {
                        $EnumLoc = $this->proc6(self::Ident_1);
                    }
                }


                $IntLoc2 = $IntLoc2 * $IntLoc1;
                $IntLoc1 = $IntLoc2 / $IntLoc3;
                $IntLoc2 = 7 * ($IntLoc2 - $IntLoc3) - $IntLoc1;
				$IntLoc1 = $this->proc2($IntLoc1);
            }

            $benchtime = (microtime(true) - $sw) * 1000;
            $loopsPerBenchtime = 0.0;
            if ($benchtime == 0.0)
            {
                $loopsPerBenchtime = 0.0;
            }
            else
            {
                $loopsPerBenchtime = ($loops / $benchtime);
            }
            $dhrystones = 1000 * $loopsPerBenchtime;

            $Check = [
                "EnumLoc" => $EnumLoc,
                "IntLoc1" => $IntLoc1,
                "IntLoc2" => $IntLoc2,
                "IntLoc3" => $IntLoc3,
                "String1Loc" => $String1Loc,
                "String2Loc" => $String2Loc
            ];

            return [
                "Dhrystones" => $dhrystones,
                "Output" => $output,
                "TimeUsed" => $benchtime,
                "VaxMips" => $dhrystones / 1757
			];
        }

        private function proc1(Record $PtrValPar)
        {
            $NextRecord = $this->PtrGlb->PtrComp;
            self::structAssign($this->PtrGlb->PtrComp, $this->PtrGlb);
            $PtrValPar->IntComp = 5;
            $NextRecord->IntComp = $PtrValPar->IntComp;
            $NextRecord->PtrComp = $PtrValPar->PtrComp;
            $NextRecord->PtrComp = $this->proc3($NextRecord->PtrComp);
            if ($NextRecord->Discr == self::Ident_1)
            {
                $NextRecord->IntComp = 6;
                $NextRecord->EnumComp =  $this->proc6($PtrValPar->EnumComp);
                $NextRecord->PtrComp = $this->PtrGlb->PtrComp;
                $NextRecord->IntComp = $this->proc7($NextRecord->IntComp, 10);
            }
            else
            {
                self::structAssign($PtrValPar, $PtrValPar->PtrComp);
            }

            return $PtrValPar;
        }

        private function proc2(int $IntParIO)
        {
            $IntLoc = $IntParIO + 10;
            $EnumLoc = self::Ident_2;
            while (true)
            {
                if ($this->Char1Glob == 'A')
                {
                    $IntLoc = $IntLoc - 1;
                    $IntParIO = $IntLoc - $this->IntGlob;
                    $EnumLoc = self::Ident_1;
                }
                if ($EnumLoc == self::Ident_1)
                    break;
            }
            return $IntParIO;
        }

        private function proc3(Record $PtrParOut)
        {
            if ($this->PtrGlb != null)
            {
                $PtrParOut = $this->PtrGlb->PtrComp;
            }
            else
            {
                $this->IntGlob = 100;
            }
			$this->PtrGlb->IntComp =  $this->proc7(10,  $this->IntGlob);
            return $PtrParOut;
        }

        function proc4()
        {
            $BoolLoc = $this->Char1Glob == 'A';
            $this->BoolGlob = $BoolLoc || $this->BoolGlob;
            $this->Char2Glob = 'B';
        }

        function proc5()
        {
            $this->Char1Glob = 'A';
            $this->BoolGlob = false;
        }

        function proc6(int $EnumParIn)
        {
            $EnumParOut = $EnumParIn;
            if (!self::func3($EnumParIn))
            {
                $EnumParOut = self::Ident_4;
            }


            switch ($EnumParIn)
            {
                case self::Ident_1:
                    $EnumParOut = self::Ident_1;
                    break;
                case self::Ident_2:
                    if ($this->IntGlob > 100)
						$EnumParOut = self::Ident_1;
                    else
						$EnumParOut = self::Ident_4;
                    break;
                case self::Ident_3:
                    $EnumParOut = self::Ident_2;
                    break;
                case self::Ident_4:
                    break;
                case self::Ident_5:
                    $EnumParOut = self::Ident_3;
                    break;
            }

            return $EnumParOut;
        }

        static function proc7($IntParI1, $IntParI2)
        {
            $IntLoc = $IntParI1 + 2;
            $IntParOut = $IntParI2 + $IntLoc;
            return $IntParOut;
        }

        function proc8(array $Array1Par,array $Array2Par, $IntParI1, $IntParI2)
        {
            $IntLoc = $IntParI1 + 5;
            $Array1Par[$IntLoc] = $IntParI2;
            $Array1Par[$IntLoc + 1] = $Array1Par[$IntLoc];
            $Array1Par[$IntLoc + 30] = $IntLoc;
            // for IntIndex in range(IntLoc, IntLoc+2):
            for ($IntIndex = $IntLoc; $IntIndex < $IntLoc + 2; ++$IntIndex)
            {
                $Array2Par[$IntLoc][$IntIndex] = $IntLoc;
            }
            $Array2Par[$IntLoc][$IntLoc - 1] = $Array2Par[$IntLoc][$IntLoc - 1] + 1;
            $Array2Par[$IntLoc + 20][$IntLoc] = $Array1Par[$IntLoc];
            $IntGlob = 5;
        }

		static function func1($CharPar1, $CharPar2)
        {
            $CharLoc1 = $CharPar1;
            $CharLoc2 = $CharLoc1;
            return $CharLoc2 != $CharPar2 ? self::Ident_1 : self::Ident_2;
        }

        static function func2(string $StrParI1, string $StrParI2)
        {
            $IntLoc = 1;
            $CharLoc = '\0';
            while ($IntLoc <= 1)
            {
                if (self::func1($StrParI1[$IntLoc], $StrParI2[$IntLoc + 1]) == self::Ident_1)
                {
                    $CharLoc = 'A';
                    $IntLoc = $IntLoc + 1;
                }
            }
            if ($CharLoc >= 'W' && $CharLoc <= 'Z')
            {
                $IntLoc = 7;
            }
            if ($CharLoc == 'X')
            {
                return true;
            }
            else
            {
                if (strcmp($StrParI1, $StrParI2) > 0)
                {
                    $IntLoc = $IntLoc + 7;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
		
		static function Func3(bool $EnumParIn)
        {
            $EnumLoc = $EnumParIn;
            return $EnumLoc == self::Ident_3;
        }
	}

	$d = new Dhrystone2(true);
	$d->bench(500000 /*Dhrystone2::LOOPS*/);
}