from entityfx import benchmark, benchmark_base, writer
from entityfx.arithemtics_benchmark import ArithemticsBenchmark

import time

benchmark_base.BenchmarkBase.ITERRATIONS_RATIO = 0.01
w = writer.Writer(None)
b = ArithemticsBenchmark(w)
b.warmup()
print(b.bench())