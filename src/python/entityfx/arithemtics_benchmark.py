
from entityfx.arithmetics_base import ArithmeticsBase

class ArithemticsBenchmark(ArithmeticsBase):
    
    def benchImplementation(self) -> float:
        self._r = (0)
        i = 0
        while i < self._iterrations: 
            self._r += ArithmeticsBase._doArithmetics0(i)
            i += 1
        return self._r