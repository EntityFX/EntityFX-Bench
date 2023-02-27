var FlopsBenchmark = /** @class */ (function (_super) {
	__extends(FlopsBenchmark, _super);

	function FlopsBenchmark(writer) {
		var _this = _super.call(this, writer) || this;
		_this.flops = new Flops(writer);
		_this.Ratio = 2;
		return _this;
	}

	FlopsBenchmark.prototype.BenchImplementation = function () {
		_super.prototype.BenchImplementation.call(this);
		return this.flops.bench(2000);
	};

	FlopsBenchmark.prototype.PopulateResult = function (benchResult, benchValue) {
		var avg = (benchValue.Mflops1 + benchValue.Mflops2 + benchValue.Mflops3 + benchValue.Mflops4) / 4.0
		benchResult.Result = avg;
		benchResult.Points = avg * this.Ratio;
		benchResult.Units = "MFLOPS";
		benchResult.Output = benchValue.Output;
		return benchResult;
	};

	FlopsBenchmark.prototype.Warmup = function () {
	};

	return FlopsBenchmark;
}(BenchmarkBase));

if (typeof module !== 'undefined' && module.exports) {
	module.exports = {
		FlopsBenchmark: FlopsBenchmark
	};
}