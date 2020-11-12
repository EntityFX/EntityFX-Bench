
from entityfx.hash_base import HashBase

class HashBenchmark(HashBase):
    
    def benchImplementation(self) -> bytearray:
        result = None

        for i in range(0, self._iterrations):
            result = HashBase._doHash(i, self._array_of_bytes )
        return result