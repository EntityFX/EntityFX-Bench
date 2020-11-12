
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer
import math

class MathBase(BenchmarkBase):
    
    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self._iterrations = 200000000
        self.ratio = .5
    
    @staticmethod
    def _doMath(i : int, li : float) -> float:
        rev = 1.0 / (((i) + 1.0))
        return (math.fabs(i) * math.acos(rev) * math.asin(rev) * math.atan(rev) + math.floor(li) + math.exp(rev) * math.cos(i) * math.sin(i) * math.pi) + math.sqrt(i)