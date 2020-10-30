var LinpackBenchmark = /** @class */ (function (_super) {
	__extends(LinpackBenchmark, _super);
	
	function LinpackBenchmark(writer) {
		var _this = _super.call(this, writer) || this;
        _this.dhrystone = new Linpack(writer);
		_this.Ratio = 10;
        return _this;
	}
	
	LinpackBenchmark.prototype.BenchImplementation = function() {
		_super.prototype.BenchImplementation.call(this);
		return this.dhrystone.bench(2000);
	};
	
	LinpackBenchmark.prototype.PopulateResult = function(benchResult, benchValue) {
		benchResult.Result = benchValue.Mflops;
		benchResult.Points = benchValue.Mflops * this.Ratio;
		benchResult.Units = "MFLOPS";
		benchResult.Output = benchValue.Output;
		return benchResult;
	};
	
	LinpackBenchmark.prototype.Warmup = function() {
	};
	
    return LinpackBenchmark;
}(BenchmarkBase));