from entityfx.call_benchmark_base import CallBenchmarkBase
from entityfx.writer import Writer
from entityfx.benchmark_base import BenchmarkBase
import time
import multiprocessing
try:
    mp = multiprocessing.get_context('fork')
except (AttributeError, ValueError):
    mp = multiprocessing

class ParallelCallBenchmark(CallBenchmarkBase):

    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self._iterrations = 2000000000
        self.ratio = .01
        self.is_parallel = True
    
    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : 0, lambda a: self.benchFunc(a), lambda a, r : None)

    def benchFunc(self, a):
        return self._doCallBench()

    @staticmethod
    def _parallelContextCallFunc(x):
        ctx = BenchmarkBase._parallelContext[x["idx"]]
        s = ctx["self"]
        start = time.time()
        r = ctx["benchFunc"]()
        result = s._buildResult(start)
        ctx["setBenchResultFunc"](r, result)
        result["Elapsed"] = r
        return result


    def bench_in_parallel(self, buildFunc, benchFunc, setBenchResultFunc):
        cpu_count = mp.cpu_count()
        BenchmarkBase._parallelContext = [{
            "benchFunc" : self._doCallBench,
            "setBenchResultFunc" : setBenchResultFunc,
            "self" : self
        }] * cpu_count

        benchs = list(map(lambda idx: {
            "idx" : idx,
            "buildData" : buildFunc()
        }, range(0, cpu_count)))
 
        p = mp.Pool(cpu_count)
        results = p.map(ParallelCallBenchmark._parallelContextCallFunc, benchs)
        p.close()
        return results