from entityfx.scimark2 import Scimark2
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer

class Scimark2Benchmark(BenchmarkBase):
    
    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self.__scimark2 = Scimark2()
        self.ratio = 10
    
    def benchImplementation(self):
        return self.__scimark2.bench()
    
    def populateResult(self, bench_result, scimark2_result):
        bench_result["Points"] = scimark2_result["CompositeScore"] * self.ratio
        bench_result["Result"] = scimark2_result["CompositeScore"]
        bench_result["Units"]  = "CompositeScore"
        bench_result["Output"] = scimark2_result["Output"]
        return bench_result
    
    def warmup(self, aspect : float=.05) -> None:
        pass