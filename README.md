# Benchmarks Set

## Languages

* C#
* PHP
* JavaScript (Browser, NodeJS)
* Java
* Python
* Lua

## Benchmarks

* Dhrystone
* Whetstone
* Scimark 2
* Linpack
* Generic:
  * Loops
  * Conditions
  * Arithmetics
  * Math
  * Array speed
  * String manipulation
  * Hash algorithms
 
## Run benchmarks

### C#

Mono:

```sh
cd release/dotnet/net45/
mono EntityFX.NetBenchmark.exe
```

DotNet Core:

```sh
cd release/dotnet/netcoreapp3.1/
dotnet EntityFX.NetBenchmark.dll
```

### Java

```sh
cd /release/java
java -jar EntityFXBench.jar
```

### JavaScript

Web:

[http://laseroid.azurewebsites.net/js-bench/]

NodeJS: TODO

### PHP

```sh
cd src/php/
php -n -t 99999 -d memory_limit=2048M Main.php
```

### Python

```sh
cd src/python/
python3 main.py
```
## Build

TODO
