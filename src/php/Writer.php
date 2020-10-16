<?php

namespace EntityFX\NetBenchmark\Core
{
	class Writer
	{
		private $writer = "";
	
        public $UseConsole = true;
        
        private $UseFile = true;
	
        public $FilePath = ""; 
        
        public $Output;
	
		public function __construct($filePath = null)
		{
			if ($filePath != null)
			{
				$this->FilePath = $filePath;
				$this->UseFile = true;
			}
		}
	
		public function WriteLine($format, ...$args)
		{
			$this->WriteColor("\033[1;30m", $format, $args);
			$this->WriteNewLine();
		}
	
		public function WriteNewLine()
		{
			if ($this->UseConsole) printf("\n");
			if ($this->UseFile)
			{
			}
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
			if ($this->UseConsole)
			{
				//var tmpColor = Console.ForegroundColor;
				//Console.ForegroundColor = color;
				//Console.Write(format, args);
				vprintf($color.$format."\033[0m", $args);
				//Console.ForegroundColor = tmpColor;
	
			}
			if ($this->UseFile)
			{
	
			}
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
