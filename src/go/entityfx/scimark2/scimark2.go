package scimark2

import (
	"../utils"
)

type Scimark2Result struct {
	compositeScore float64
	sor float64
	fft float64
	monteCarlo float64
	sparseMathmult float64
	lu float64
}

func Bench(min_time float64, isLarge bool, output utils.WriterType) *Scimark2Result {
	fFT_size := FFT_SIZE
	sOR_size := SOR_SIZE
	sparse_size_M := SPARSE_SIZE_M
	sparse_size_nz := SPARSE_SIZE_nz
	lU_size := LU_SIZE
	current_arg := 0
	if isLarge {
		fFT_size = LG_FFT_SIZE
		sOR_size = LG_SOR_SIZE
		sparse_size_M = LG_SPARSE_SIZE_M
		sparse_size_nz = LG_SPARSE_SIZE_nz
		lU_size = LG_LU_SIZE
		current_arg++
	}

	res := [6]float64{}

	res[1] = measureFFT(fFT_size, min_time)
	res[2] = measureSOR(sOR_size, min_time)
	res[3] = measureMonteCarlo(min_time)
	res[4] = measureSparseMatmult(sparse_size_M, sparse_size_nz, min_time)
	res[5] = measureLU(lU_size, min_time)
	res[0] = (((res[1] + res[2] + res[3]) + res[4] + res[5])) / (5.0)

	output.WriteNewLine() 
	output.WriteLine("SciMark 2.0a") 
	output.WriteNewLine() 
	output.WriteLine("Composite Score: %.2f", res[0]) 
	output.Write("FFT (%d): ", fFT_size) 
	if (res[1] == .0) {
		output.WriteLine(" ERROR, INVALID NUMERICAL RESULT!")
	} else {
		output.WriteLine("%.2f", res[1]) 
	}
	output.WriteLine("SOR (%dx%d):   %.2f", sOR_size, sOR_size, res[2]) 
	output.WriteLine("Monte Carlo : %.2f", res[3]) 
	output.WriteLine("Sparse matmult (N=%d, nz=%d): %.2f", sparse_size_M, sparse_size_nz, res[4]) 
	output.Write("LU (%dx%d): ", lU_size, lU_size) 
	if (res[5] == .0) {
		output.WriteLine(" ERROR, INVALID NUMERICAL RESULT!") 
	} else {
		output.WriteLine("%.2f", res[5]) 
	}
	return &Scimark2Result{res[0], res[1], res[2], res[3], res[4], res[5]}
}