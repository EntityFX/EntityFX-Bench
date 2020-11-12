from entityfx import benchmark, benchmark_base, writer
from entityfx.arithemtics_benchmark import ArithemticsBenchmark
from entityfx.parallel_arithemtics_benchmark import ParallelArithemticsBenchmark
from entityfx.math_benchmark import MathBenchmark
from entityfx.parallel_math_benchmark import ParallelMathBenchmark
from entityfx.call_benchmark import CallBenchmark
from entityfx.parallel_call_benchmark import ParallelCallBenchmark
from entityfx.if_else_benchmark import IfElseBenchmark
from entityfx.parallel_if_else_benchmark import ParallelIfElseBenchmark
from entityfx.string_manipulation import StringManipulation
from entityfx.parallel_string_manipulation import ParallelStringManipulation
from entityfx.memory_benchmark import MemoryBenchmark
from entityfx.parallel_memory_benchmark import ParallelMemoryBenchmark
from entityfx.random_memory_benchmark import RandomMemoryBenchmark
from entityfx.parallel_random_memory_benchmark import ParallelRandomMemoryBenchmark
from entityfx.dhrystone_benchmark import DhrystoneBenchmark
from entityfx.parallel_dhrystone_benchmark import ParallelDhrystoneBenchmark
from entityfx.whetstone_benchmark import WhetstoneBenchmark
from entityfx.parallel_whetstone_benchmark import ParallelWhetstoneBenchmark
from entityfx.scimark2_benchmark import Scimark2Benchmark
from entityfx.parallel_scimark2_benchmark import ParallelScimark2Benchmark
from entityfx.linpack_benchmark import LinpackBenchmark
from entityfx.parallel_linpack_benchmark import ParallelLinpackBenchmark
from entityfx.hash_benchmark import HashBenchmark
from entityfx.parallel_hash_benchmark import ParallelHashBenchmark
from entityfx.writer import Writer

import time
import platform
import multiprocessing


writer = Writer("Output.log")

benchmark_base.BenchmarkBase.ITERRATIONS_RATIO = 0.01


def write_result(bench_result) -> None:
    writer.write_title("{0:<30}", bench_result["Name"])
    writer.write_value("{0:>13.2f} ms", bench_result["Elapsed"])
    writer.write_value("{0:>13.2f} pts", bench_result["Points"])
    writer.write_value(
        "{0:>13.2f} {1}", bench_result["Result"], bench_result["Units"])
    writer.write_line()
    writer.write_value("Iterrations: {0:<15}, Ratio: {1:<15}",
                    bench_result["Iterrations"], bench_result["Ratio"])
    writer.write_line()


bench_marks = [
    ArithemticsBenchmark(writer),
    ParallelArithemticsBenchmark(writer),

    MathBenchmark(writer),
    ParallelMathBenchmark(writer),

    CallBenchmark(writer),
    ParallelCallBenchmark(writer),

    IfElseBenchmark(writer),
    ParallelIfElseBenchmark(writer),

    StringManipulation(writer),
    ParallelStringManipulation(writer),

    MemoryBenchmark(writer),
    ParallelMemoryBenchmark(writer),

    RandomMemoryBenchmark(writer),
    ParallelRandomMemoryBenchmark(writer),

    Scimark2Benchmark(writer),
    ParallelScimark2Benchmark(writer),

    DhrystoneBenchmark(writer),
    ParallelDhrystoneBenchmark(writer),

    WhetstoneBenchmark(writer),
    ParallelWhetstoneBenchmark(writer),

    LinpackBenchmark(writer),
    ParallelLinpackBenchmark(writer),

    HashBenchmark(writer),
    ParallelHashBenchmark(writer)
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

writer.write_line()
writer.write_title("{0:<30}", "Total:")
writer.write_value("{0:13.2f} ms", total)
writer.write_value("{0:13.2f} pts", total_points)
writer.write_line()

header_common = "Operating System,Runtime,Threads Count,Memory Used"
header_totals = ",Total Points,Total Time (ms)"

isNotParallel = lambda r: not r["IsParallel"]


writer.write_line()
writer.write_header("Single-thread results")
writer.write_title(header_common)

for r in filter(isNotParallel, result):
    writer.write_title("," + r["Name"])

writer.write_title(header_totals)
writer.write_line()
writer.write_title("{0},{1} {2},{3},{4}", platform.platform(), platform.python_implementation(), platform.python_version(), multiprocessing.cpu_count(), 0)

for r in filter(isNotParallel, result):
    writer.write_value(",{0:1.2f}", r["Points"])

writer.write_title(",{0:1.2f},{1:1.2f}", total_points, total)
writer.write_line()


writer.write_line()
writer.write_header("Single-thread Units results")
writer.write_title(header_common)

for r in filter(isNotParallel, result):
    writer.write_title("," + r["Name"])

writer.write_title(header_totals)
writer.write_line()
writer.write_title("{0},{1} {2},{3},{4}", platform.platform(), platform.python_implementation(), platform.python_version(), multiprocessing.cpu_count(), 0)

for r in filter(isNotParallel, result):
    writer.write_value(",{0:1.2f}", r["Result"])

writer.write_title(",{0:1.2f},{1:1.2f}", total_points, total)
writer.write_line()

writer.write_line()
writer.write_header("All results")
writer.write_title(header_common)

for r in result:
    writer.write_title("," + r["Name"])

writer.write_title(header_totals)
writer.write_line()
writer.write_title("{0},{1} {2},{3},{4}", platform.platform(), platform.python_implementation(), platform.python_version(), multiprocessing.cpu_count(), 0)

for r in result:
    writer.write_value(",{0:1.2f}", r["Points"])

writer.write_title(",{0:1.2f},{1:1.2f}", total_points, total)
writer.write_line()


writer.write_line()
writer.write_header("All Units results")
writer.write_title(header_common)

for r in result:
    writer.write_title("," + r["Name"])

writer.write_title(header_totals)
writer.write_line()
writer.write_title("{0},{1} {2},{3},{4}", platform.platform(), platform.python_implementation(), platform.python_version(), multiprocessing.cpu_count(), 0)

for r in result:
    writer.write_value(",{0:1.2f}", r["Result"])

writer.write_title(",{0:1.2f},{1:1.2f}", total_points, total)
writer.write_line()
writer.write_line()
