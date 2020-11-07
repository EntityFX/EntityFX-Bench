from entityfx.benchmark_base import BenchmarkBase
from entityfx.call_benchmark_base import CallBenchmarkBase
import time

class CallBenchmark(CallBenchmarkBase):
    
    def benchImplementation(self) -> float:
        return .0
    
    def bench(self) :
        self._beforeBench()
        start = time.time()
        call_time = self._doCallBench()
        result = self.populateResult(self._buildResult(start), call_time)
        result["Elapsed"] = time.time() - start
        self._doOutput(result)
        self._afterBench(result)
        return result