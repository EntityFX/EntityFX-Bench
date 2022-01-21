
import time
import math
from entityfx.benchmark import Benchamrk
from entityfx.writer import Writer
import threading
from concurrent.futures.thread import ThreadPoolExecutor
from concurrent.futures import ALL_COMPLETED, thread, wait
import multiprocessing
try:
    mp = multiprocessing.get_context('fork')
except (AttributeError, ValueError):
    mp = multiprocessing

class BenchmarkBase(Benchamrk):

    def __init__(self, writer: Writer = None, print_to_console: bool = True, is_enabled : bool = True) -> None:
        self._iterrations = 0
        self._print_to_console = print_to_console
        if Writer is None:
            self._output = Writer()
        else:
            self._output = writer

        self.ratio = 1.0
        self.__isparallel = False
        self.is_enabled = is_enabled

    ITERRATIONS_RATIO = 1.0

    @property
    def is_parallel(self) -> bool:
        return self.__isparallel

    @is_parallel.setter
    def is_parallel(self, value) -> bool:
        self.__isparallel = value
        return self.__isparallel

    def _use_console(self, value: bool) -> bool:
        self._print_to_console = value
        self._output.use_console = value

    @property
    def name(self) -> str:
        return self.__class__.__name__

    def bench(self):
        if (not self.is_enabled) :
            return self._buildResult(None)
        self._beforeBench()
        start = time.time()
        res = self.benchImplementation()
        result = self.populateResult(self._buildResult(start), res)
        self._doOutput(result)
        self._afterBench(result)
        return result

    def _doOutput(self, result: dict) -> None:
        if (result['Output'] is None or result['Output'] == ""):
            return
        f = open("{name}.log".format(name=self.name), "a")
        f.write(result['Output'])
        f.close()

    def benchImplementation(self):
        return None

    def _beforeBench(self) -> None:
        pass

    def _afterBench(self, result) -> None:
        pass

    def warmup(self, aspect: float = .05) -> None:
        if (not self.is_enabled) :
            return
        self._iterrations = (math.floor(
            round((self._iterrations) * BenchmarkBase.ITERRATIONS_RATIO, 0)))
        tmp = self._iterrations
        self._iterrations = (math.floor(
            round((self._iterrations) * aspect, 0)))
        self.bench()
        self._iterrations = tmp

    @staticmethod
    def _parallelContextFunc(x):
        ctx = BenchmarkBase._parallelContext[x["idx"]]
        s = ctx["self"]
        start = time.time()
        r = ctx["benchFunc"](x["buildData"])
        result = s._buildResult(start)
        ctx["setBenchResultFunc"](r, result)
        return result


    def bench_in_parallel(self, buildFunc, benchFunc, setBenchResultFunc):
        self._use_console(False)
        cpu_count = mp.cpu_count()
        BenchmarkBase._parallelContext = [{
            "benchFunc" : benchFunc,
            "setBenchResultFunc" : setBenchResultFunc,
            "self" : self
        }] * cpu_count

        benchs = list(map(lambda idx: {
            "idx" : idx,
            "buildData" : buildFunc()
        }, range(0, cpu_count)))
 
        p = mp.Pool(cpu_count)
        results = p.map(BenchmarkBase._parallelContextFunc, benchs)
        p.close()
        self._use_console(True)

        return results

    def populateResult(self, bench_result: dict, dhrystone_result) -> dict:
        if type(dhrystone_result) is list:
            self._buildParallelResult(bench_result, dhrystone_result)
        return bench_result

    def _buildResult(self, start: float) -> dict:
        elapsed_seconds = 0 if start == None else time.time() - start
        elapsed = elapsed_seconds * 1000.0
        return {
            "Name": self.name,
            "Elapsed": elapsed,
            "Points": 0 if start == None else self._iterrations / elapsed * self.ratio,
            "Result": 0 if start == None else self._iterrations / elapsed_seconds,
            "Units": "Iter/s",
            "Iterrations": self._iterrations,
            "Ratio": self.ratio,
            "IsParallel": self.is_parallel,
            "Output": None
        }

    def _buildParallelResult(self, rootResult, results: list):
        rootResult["Points"] = 0 if not self.is_enabled else sum(map(lambda x : x["Points"], results))
        rootResult["Result"] = 0 if not self.is_enabled else sum(map(lambda x : x["Result"], results))
        rootResult["IsParallel"] = self.is_parallel
        rootResult["Iterrations"] = self._iterrations
        rootResult["Ratio"] = self.ratio
        return rootResult
