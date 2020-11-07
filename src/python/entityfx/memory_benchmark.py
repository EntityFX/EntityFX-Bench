
from entityfx.memory_benchmark_base import MemoryBenchmarkBase

class MemoryBenchmark(MemoryBenchmarkBase):
    
    def benchImplementation(self) :
        return self.benchRandomMemory()
    
    def populateResult(self, bench_result, result) :
        bench_result["Points"] = result["Average"] * self.ratio
        bench_result["Result"] = result["Average"]
        bench_result["Units"] = "MB/s"
        bench_result["Output"] = result["Output"]
        return bench_result