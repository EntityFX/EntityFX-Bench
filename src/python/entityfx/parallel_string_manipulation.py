from entityfx.string_manipulation_base import StringManipulationBase
from entityfx.writer import Writer

class ParallelStringManipulation(StringManipulationBase):

    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self.is_parallel = True
    
    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : 0, lambda a: self.benchFunc(a), lambda a, r : None)

    def benchFunc(self, a):
        str0_ = "the quick brown fox jumps over the lazy dog"
        str1 = ""
        i = 0
        while i < self._iterrations: 
            str1 = StringManipulationBase._doStringManipilation(str0_)
            i += 1
        return str1