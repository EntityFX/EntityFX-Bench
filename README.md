# EntityFX Benchmarks Set

![](https://repository-images.githubusercontent.com/294349875/7e366c80-2123-11eb-9279-0c805e037106)

## Languages

* C#
* PHP
* JavaScript (Browser, NodeJS)
* Java
* Python
* Lua

## Benchmarks

* Dhrystone (http://www.roylongbottom.org.uk/#anchorSource)
* Whetstone (http://www.roylongbottom.org.uk/#anchorSource)
* Scimark 2 (Original sources: https://math.nist.gov/scimark2/download.html)
* Linpack (Based on: https://github.com/fommil/netlib-java/blob/master/perf/src/main/java/com/github/fommil/netlib/Linpack.java)
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

#### Web

http://laseroid.azurewebsites.net/js-bench/

#### NodeJS

You must have nodejs installed

```sh
cd src/js
node ./node_main.js
```

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

### Lua

```sh
cd src/lua/
lua main.lua
```

## Build

TODO

### Go
```sh
cd src/go/entityfx
go build
```

alternatively:
```sh
go get -u github.com/EntityFX/EntityFX-Bench/src/go/entityfx
```

