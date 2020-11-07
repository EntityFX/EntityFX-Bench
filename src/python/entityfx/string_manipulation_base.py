
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer
import math, time

class StringManipulationBase(BenchmarkBase):
    
    def __init__(self, writer : Writer=None, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self._iterrations = 5000000
        self.ratio = (10)
    
    @staticmethod
    def _doStringManipilation(str0_ : str) -> str:
        return ("/".join(str0_.split(' ')).replace("/", "_").upper() + "AAA").lower().replace("aaa", ".")