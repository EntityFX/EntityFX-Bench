
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer
import math

class ArithmeticsBase(BenchmarkBase):
    
    def __init__(self, writer : Writer=None, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self._r = 0
        self._iterrations = 300000000
        self.ratio = .03
    
    @staticmethod
    def _doArithmetics(i : int) -> float:
        return ((((((math.floor(i / 10))) * ((math.floor(i / 100))) * ((math.floor(i / 100)))) * ((math.floor(i / 100))) * 1.11) + ((((math.floor(i / 100))) * ((math.floor(i / 1000))) * ((math.floor(i / 1000)))) * 2.22)) - ((i) * ((math.floor(i / 10000))) * 3.33)) + ((i) * 5.33)
    
    @staticmethod
    def _doArithmetics0(i : int) -> float:
        return ((((((math.floor(i / (10)))) * ((math.floor(i / (100)))) * ((math.floor(i / (100))))) * ((math.floor(i / (100)))) * 1.11) + ((((math.floor(i / (100)))) * ((math.floor(i / (1000)))) * ((math.floor(i / (1000))))) * 2.22)) - ((i) * ((math.floor(i / (10000)))) * 3.33)) + ((i) * 5.33)