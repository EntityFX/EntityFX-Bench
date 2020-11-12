
class Benchamrk:
    
    @property
    def name(self) -> str:
        return None
    
    @property
    def is_parallel(self) -> bool:
        return None
    
    def bench(self):
        return None
    
    def warmup(self, aspect : float=.05) -> None:
        pass