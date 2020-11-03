<?php

namespace EntityFX\NetBenchmark\Core
{
	class Writer
	{
		private $writer = "";

		private $useColor = true;
	
        public $UseConsole = true;
	
        public $FilePath = null; 
        
        public $Output;
	
		public function __construct($filePath = null)
		{
			if ($filePath != null)
			{
				$this->FilePath = $filePath;
			}

			if (substr(PHP_OS, 0, 3) === 'WIN') {
				$this->useColor = false;
			}
		}
	
		public function WriteLine($format, ...$args)
		{
			$this->WriteColor("\033[1;30m", $format, $args);
			$this->WriteNewLine();
		}
	
		public function WriteNewLine()
		{
			if ($this->UseConsole) {
				if ($this->FilePath) {
					file_put_contents($this->FilePath, "\n", FILE_APPEND);
				}
				printf("\n");
			}
			$this->Output .= "\n";
		}
	
		public function WriteHeader($format, ...$args)
		{
			$this->WriteColor("\033[1;36m", $format, $args);
			$this->WriteNewLine();
		}
	
		public function Write($format, ...$args)
		{
			$this->WriteColor("\033[1;30m", $format, $args);
		}
	
		public function WriteColor($color, $format, array $args)
		{
			$formatted = vsprintf($format, $args);
			if ($this->UseConsole)
			{
				if ($this->useColor) {
					echo($color.$formatted."\033[0m");
				} else {
					echo($formatted);
				}

				//Console.ForegroundColor = tmpColor;
				if ($this->FilePath) {
					file_put_contents($this->FilePath, $formatted, FILE_APPEND);
				}
			}
			$this->Output .= $formatted;
		}
	
		public function WriteValue($format, ...$args)
		{
			$this->WriteColor("\033[1;32m", $format, $args);
		}
	
		public function WriteTitle($format, ...$args)
		{
			$this->WriteColor("\033[1;37m", $format, $args);
		}
	}
}

namespace {
    if (!defined('PHP_INT_MIN')) {
        define('PHP_INT_MIN', ~PHP_INT_MAX);
    }

    if (!function_exists('intdiv')) {
        function intdiv($dividend, $divisor)
        {
            $dividend = (int) $dividend;
            $divisor = (int) $divisor;

            if ($divisor === 0) {
                throw new DivisionByZeroError('Division by zero');
            }

            if ($divisor === -1 && $dividend === PHP_INT_MIN) {
                throw new ArithmeticError('Division of PHP_INT_MIN by -1 is not an integer');
            }

            $dividend = ($dividend - $dividend % $divisor);

            return ((int) ($dividend / $divisor));
        }
    }

    if ((!class_exists('ArithmeticError') || !class_exists('DivisionByZeroError')) 
        && (!class_exists('Error') || (!in_array('Throwable', class_implements('Error')) && !in_array('Throwable', class_implements('Exception'))))) {
        class Error extends \Exception
        {
            public function __toString()
            {
                return $this->getMessage();
            }
        }
    }

    if (!class_exists('ArithmeticError')) {
        class ArithmeticError extends Error
        {
            public function __toString()
            {
                return $this->getMessage();
            }
        }
    }

    if (!class_exists('DivisionByZeroError')) {
        class DivisionByZeroError extends Error
        {
            public function __toString()
            {
                return $this->getMessage();
            }
        }
    }
}
