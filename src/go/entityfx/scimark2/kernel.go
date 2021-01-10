package scimark2

import (
	"math"
	"math/rand"
	"time"
	"../utils"
)

func measureFFT(N int, mintime float64) float64 {
	x := randomVector(2 * N)
	//oldx := newVectorCopy(x)
	var cycles int64 = 1
	elapsed := 0.0
	for true {
		start := (float64(utils.MakeTimestamp()) / 1000.0) 
		var i int64 = 0
		for ; i < cycles; i++ {
			fft_transform(x)
			fft_inverse(x)
		}
		elapsed = (float64(utils.MakeTimestamp()) / 1000.0) - start
		if elapsed >= mintime {
			break
		}
		cycles *= 2
	}
	var EPS float64 = 1.0e-10
	if fft_test(x) / float64(N) > EPS {
		return .0
	}
	return fft_num_flops(N) * float64(cycles) / elapsed * 1.0e-6
}

func measureSOR(N int, min_time float64) float64 {
	G := randomMatrix(N, N)
	elapsed := 0.0
	cycles := 1
	for true {
		start := (float64(utils.MakeTimestamp()) / 1000.0) 
		sor_execute(1.25, G, cycles)
		elapsed = (float64(utils.MakeTimestamp()) / 1000.0) - start
		if elapsed >= min_time {
			break
		}
		cycles *= 2
	}
	return sor_num_flops(N, N, cycles) / elapsed * 1.0e-6
}

func measureMonteCarlo(min_time float64) float64 {
	elapsed := 0.0
	cycles := 1
	for (true) {
		start := (float64(utils.MakeTimestamp()) / 1000.0) 
		mc_integrate(cycles);
		elapsed = (float64(utils.MakeTimestamp()) / 1000.0) - start
		if elapsed >= min_time {
			break
		}
		cycles *= 2
	}
	return mc_num_flops(cycles) / elapsed * 1.0e-6
}

func measureSparseMatmult(N int, nz int, min_time float64) float64 {
	x := randomVector(N)
	y := make([]float64, N)
	nr := nz / N
	anz := nr * N
	val := randomVector(anz)
	col := make([]int, anz)
	row := make([]int, N + 1)
	row[0] = 0
	for r := 0; r < N; r++ {
		rowr := row[r]
		row[r + 1] = rowr + nr
		step := r / nr
		if step < 1 {
			step = 1
		}
		for i := 0; i < nr; i++ {
			col[rowr + i] = i * step
		}
	}
	elapsed := 0.0
	cycles := 1
	for true {
		start := (float64(utils.MakeTimestamp()) / 1000.0) 
		sparse_cr_matmult(y, val, row, col, x, cycles)
		elapsed = (float64(utils.MakeTimestamp()) / 1000.0) - start
		if elapsed >= min_time {
			break
		}
		cycles *= 2
	}
	return sparse_cr_num_flops(N, nz, cycles) / elapsed * 1.0e-6
}

func measureLU(N int, min_time float64) float64 {
	A := randomMatrix(N, N)
	lu := make([][]float64, N)
	for i := 0; i < N; i++ {
		lu[i] = make([]float64, N)
	}
	pivot := make([]int, N)
	elapsed := 0.0
	cycles := 1
	for true {
		start := (float64(utils.MakeTimestamp()) / 1000.0) 
		for i := 0; i < cycles; i++ {
			copyMatrix(lu, A)
			lu_factor(lu, pivot)
		}
		elapsed = (float64(utils.MakeTimestamp()) / 1000.0) - start
		if elapsed >= min_time {
			break
		}
		cycles *= 2
	}
	b := randomVector(N)
	x := newVectorCopy(b)
	lu_solve_matrix(lu, pivot, x)
	EPS := 1.0e-12
	if (normabs(b, matvec(A, x)) / float64(N)) > EPS {
		return .0
	}
	return lu_num_flops(N) * float64(cycles) / elapsed * 1.0e-6
}

func newVectorCopy(x []float64) []float64{
	N := len(x)
	y := make([]float64, N)
	for i := 0; i < N; i++ {
		y[i] = x[i]
	}
	return y
}

func copyVector(B []float64, A []float64) {
	N := len(A)
	for i := 0; i < N; i++ {
		B[i] = A[i]
	}
}

func normabs(x []float64, y []float64) float64 {
	N := len(x)
	sum := .0
	for i := 0; i < N; i++ {
		sum += math.Abs(x[i] - y[i])
	}
	return sum;
}

func copyMatrix(B [][]float64, A [][]float64) {
	M := len(A)
	N := len(A[0])
	remainder := N & 3
	for i := 0; i < M; i++ {
		bi := B[i]
		ai := A[i]
		for j := 0; j < remainder; j++ {
			bi[j] = ai[j]
		}
		for j := remainder; j < N; j += 4 {
			bi[j] = ai[j]
			bi[j + 1] = ai[j + 1]
			bi[j + 2] = ai[j + 2]
			bi[j + 3] = ai[j + 3]
		}
	}
}

func randomMatrix(M int, N int) [][]float64 {
	A := make([][]float64, M)
	for i := 0; i < N; i++ {
		A[i] = make([]float64, N)
		for j := 0; j < N; j++ {
			A[i][j] = rand.Float64()
		}
	}
	return A
}

func randomVector(N int) []float64{
	rand.Seed(time.Now().Unix())
	A := make([]float64, N)
	for i := 0; i < N; i++ {
		A[i] = rand.Float64()
	}
	return A
}

func matvec(A [][]float64, x []float64) []float64 {
	N := len(x)
	y := make([]float64, N)
	matvec_n(A, x, y)
	return y
}

func matvec_n(A [][]float64, x []float64, y []float64) {
	M := len(A)
	N := len(A[0])
	for i := 0; i < M; i++ {
		sum := .0;
		ai := A[i]
		for j := 0; j < N; j++ {
			sum += (ai[j] * x[j])
		}
		y[i] = sum
	}
}
