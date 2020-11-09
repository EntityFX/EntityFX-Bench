
from entityfx.benchmark_base import BenchmarkBase
from entityfx.whetstone import Whetstone
from entityfx.writer import Writer


class WhetstoneBenchmark(BenchmarkBase):
    
    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self.__whetstone = Whetstone()
        Ratio = 1
    
    def benchImplementation(self):
        return self.__whetstone.bench()
    
    def populateResult(self, bench_result, dhrystone_result):
        bench_result["Points"] = dhrystone_result["MWIPS"]
        bench_result["Result"] = dhrystone_result["MWIPS"] * self.ratio
        bench_result["Units"] = "MWIPS"
        bench_result["Output"] = dhrystone_result["Output"]
        return bench_result
    
    def warmup(self, aspect : float=.05) -> None:
        pass
