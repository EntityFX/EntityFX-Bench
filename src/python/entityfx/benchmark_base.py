
import time, math
from entityfx.benchmark import Benchamrk
from entityfx.writer import Writer

class BenchmarkBase(Benchamrk):
    
    def __init__(self, writer : Writer=None, print_to_console : bool=True) -> None:
        self._iterrations = 0
        self._print_to_console = print_to_console
        self._output = writer is None if Writer() else writer
        self.ratio = 1.0
        self.__isparallel = False
    
    ITERRATIONS_RATIO = 1.0
    
    @property
    def is_parallel(self) -> bool:
        return self.__isparallel
    @is_parallel.setter
    def is_parallel(self, value) -> bool:
        self.__isparallel = value
        return self.__isparallel
    
    @property
    def name(self) -> str:
        return type(self)

    def bench(self):
        self._beforeBench()
        start = time.time()
        res = self.benchImplementation()
        result = self.populateResult(self._buildResult(start), res)
        self._doOutput(result)
        self._afterBench(result)
        return result
    
    def _doOutput(self, result : dict) -> None:
        if (result['Output'] is None): 
            return
        f = open(f"{self.name}.log", "a")
        f.write(result['Output'])
        f.close()

    def benchImplementation(self) :
        return None
    
    def _beforeBench(self) -> None:
        pass
    
    def _afterBench(self, result) -> None:
        pass
    
    def warmup(self, aspect : float=.05) -> None:
        self._iterrations = (math.floor(round((self._iterrations) * BenchmarkBase.ITERRATIONS_RATIO, 0)))
        tmp = self._iterrations
        self._iterrations = (math.floor(round((self._iterrations) * aspect, 0)))
        self.bench()
        self._iterrations = tmp
    
    # def _benchInParallel(self, build_func : Func, bench_func :  ERROR(type=Func), set_bench_result_func : Action) -> list:
    #     pass
    
    def populateResult(self, bench_result : dict, dhrystone_result) -> dict :
        return bench_result
    
    def _buildResult(self, start : float) -> dict :
        elapsed_seconds = time.time() - start
        elapsed = elapsed_seconds * 1000.0
        return {
            "Name" : self.name,
            "Elapsed" : elapsed,
            "Points" : self._iterrations / elapsed * self.ratio,
            "Result" : self._iterrations / elapsed_seconds,
            "Units" : "Iter/s",
            "Iterrations" : self._iterrations,
            "Ratio" : self.ratio,
            "Output" : None
        }
    
    def _buildParallelResult(self, start, results : list) :
        pass