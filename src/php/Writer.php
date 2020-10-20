<?php

namespace EntityFX\NetBenchmark\Core
{
	class Writer
	{
		private $writer = "";
	
        public $UseConsole = true;
	
        public $FilePath = null; 
        
        public $Output;
	
		public function __construct($filePath = null)
		{
			if ($filePath != null)
			{
				$this->FilePath = $filePath;
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
				//var tmpColor = Console.ForegroundColor;
				//Console.ForegroundColor = color;
				//Console.Write(format, args);
				echo($color.$formatted."\033[0m");
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
