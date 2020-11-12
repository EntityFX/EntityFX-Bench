from entityfx.linpack import Linpack
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer

class LinpackBenchmark(BenchmarkBase):
    
    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self.__linpack = Linpack(True)
        self.ratio = 10
    
    def benchImplementation(self):
        return self.__linpack.bench(1000)
    
    def populateResult(self, bench_result, linpack_result):
        bench_result["Points"] = linpack_result["MFLOPS"] * self.ratio
        bench_result["Result"] = linpack_result["MFLOPS"]
        bench_result["Units"] = "MFLOPS"
        bench_result["Output"] = linpack_result["Output"]
        return bench_result
    
    def warmup(self, aspect : float=.05) -> None:
        pass