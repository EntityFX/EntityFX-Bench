var MemoryBenchmark = /** @class */ (function (_super) {
	__extends(MemoryBenchmark, _super);

	function MemoryBenchmark(writer, printToConsole) {
		printToConsole = printToConsole || true;
		var _this = _super.call(this, writer, printToConsole) || this;
		return _this;
	}

	MemoryBenchmark.prototype.BenchImplementation = function () {
		return this.BenchRandomMemory();
	};

	MemoryBenchmark.prototype.PopulateResult = function (benchResult, result) {
		benchResult.Points = result.Average * this.Ratio;
		benchResult.Result = result.Average;
		benchResult.Units = "MB/s";
		benchResult.Output = result.Output;
		return benchResult;
	};

	return MemoryBenchmark;
}(MemoryBenchmarkBase));

if (typeof module !== 'undefined' && module.exports) {
	module.exports = {
		MemoryBenchmark: MemoryBenchmark
	};
}