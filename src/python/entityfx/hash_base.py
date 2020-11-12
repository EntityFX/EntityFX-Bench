
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer
import hashlib

class HashBase(BenchmarkBase):
    
    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self._r = 0
        self._strs = ["the quick brown fox jumps over the lazy dog", "Some red wine", "Candels & Ropes"]
        self._array_of_bytes = list(map(lambda x: x.encode(), self._strs))
        self._iterrations = 2000000
        self.ratio = 10

    @staticmethod
    def _doHash(i : int, prepared_bytes):
        hashlib.sha1()
        sha1_hash = hashlib.sha1(prepared_bytes[i % 3]).digest()
        sha256_hash = hashlib.sha256(prepared_bytes[(i + 1) % 3]).digest()
        return sha1_hash + sha256_hash