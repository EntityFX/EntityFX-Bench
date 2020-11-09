from entityfx import benchmark, benchmark_base, writer
from entityfx.arithemtics_benchmark import ArithemticsBenchmark
from entityfx.math_benchmark import MathBenchmark
from entityfx.call_benchmark import CallBenchmark
from entityfx.if_else_benchmark import IfElseBenchmark
from entityfx.string_manipulation import StringManipulation
from entityfx.memory_benchmark import MemoryBenchmark
from entityfx.random_memory_benchmark import RandomMemoryBenchmark
from entityfx.dhrystone_benchmark import DhrystoneBenchmark
from entityfx.whetstone_benchmark import WhetstoneBenchmark

from entityfx.linpack import Linpack

from entityfx.writer import Writer

import time

# l = Linpack(True)
# l.bench(2000)


writer = Writer("Output.log")

benchmark_base.BenchmarkBase.ITERRATIONS_RATIO = 0.01

def write_result(bench_result) -> None:
    writer.write_title("{0:<30}", bench_result["Name"])
    writer.write_value("{0:>13.2f} ms", bench_result["Elapsed"])
    writer.write_value("{0:>13.2f} pts", bench_result["Points"])
    writer.write_value("{0:>13.2f} {1}", bench_result["Result"], bench_result["Units"])
    writer.write_line()
    writer.write_value("Iterrations: {0:<15}, Ratio: {1:<15}", bench_result["Iterrations"], bench_result["Ratio"])
    writer.write_line()


bench_marks = [ 
    MemoryBenchmark(writer), 
    RandomMemoryBenchmark(writer), 
    ArithemticsBenchmark(writer), 
    MathBenchmark(writer), 
    CallBenchmark(writer), 
    IfElseBenchmark(writer), 
    StringManipulation(writer),  

    # Scimark2Benchmark(), 
    DhrystoneBenchmark(writer),
    WhetstoneBenchmark(writer), 
    # LinpackBenchmark(), 
    ]


total = 0
total_points = 0

result = list()

writer.write_header("Warmup")

for bench in bench_marks: 
    bench.warmup(.05)
    writer.write(".")

writer.write_line()
writer.write_header("Bench")

i = 1
for bench in bench_marks: 
    writer.write_header("[{0}] {1}", i, bench.name)
    r = bench.bench()
    total += r["Elapsed"]
    total_points += r["Points"]
    write_result(r)
    result.append(r)
    i += 1
       
