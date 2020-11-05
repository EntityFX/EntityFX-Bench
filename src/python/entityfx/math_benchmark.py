from entityfx.benchmark_base import BenchmarkBase
from entityfx.math_base import MathBase


class MathBenchmark(MathBase):
    
    def benchImplementation(self) -> float:
        r = 0
        li = 0
        i = 0
        while i < self._iterrations: 
            r += MathBase._doMath(i, li)
            li = (i); i += 1
        return r