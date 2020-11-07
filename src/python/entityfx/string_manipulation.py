from entityfx.string_manipulation_base import StringManipulationBase

class StringManipulation(StringManipulationBase):
    
    def benchImplementation(self) -> str:
        str0_ = "the quick brown fox jumps over the lazy dog"
        str1 = ""
        i = 0
        while i < self._iterrations: 
            str1 = StringManipulationBase._doStringManipilation(str0_)
            i += 1
        return str1
