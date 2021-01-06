package scimark2

import (
	"math"
)

var LU_ [][]float64

var pivot_ []int

func lu_num_flops(N int) float64 {
	// rougly 2/3*N^3

	Nd := float64(N)

	return (2.0 * Nd * Nd * Nd / 3.0)
}

func lu_new_copy(x []float64) []float64{
	N := len(x)
	T := make([]float64, N)
	for i := 0; i < N; i++ {
		T[i] = x[i]
	}
	return T
}

func lu_new_matrix_copy(A [][]float64) [][]float64 {
	M := len(A)
	N := len(A[0])

	T := make([][]float64, M)

	for i := 0; i < M; i++ {
		Ti := T[i]
		Ai := A[i]
		for j := 0; j < N; j++ {
			Ti[j] = Ai[j]
		}
	}

	return T
}

func lu_int_new_copy(x []int) []int {
	N := len(x)
	T := make([]int, N)
	for i := 0; i < N; i++ {
		T[i] = x[i]
	}
	return T
}

func lu_insert_copy(B [][]float64, A [][]float64) {
	M := len(A)
	N := len(A[0])

	remainder := N & 3       // N mod 4;

	for i := 0; i < M; i++ {
		Bi := B[i]
		Ai := A[i]
		for j := 0; j < remainder; j++ {
			Bi[j] = Ai[j]
		}
		for j := remainder; j < N; j += 4 {
			Bi[j] = Ai[j]
			Bi[j + 1] = Ai[j + 1]
			Bi[j + 2] = Ai[j + 2]
			Bi[j + 3] = Ai[j + 3]
		}
	}
}

func lu_getLU() [][]float64 {
	return lu_new_matrix_copy(LU_)
}

func lu_getPivot() []int{
	return lu_int_new_copy(pivot_)
}

func LU(A [][]float64) {
	M := len(A)
	//N := len(A[0])

	//if ( LU_ == null || LU_.length != M || LU_[0].length != N)
	LU_ = make([][]float64, M)

	lu_insert_copy(LU_, A)

	//if (pivot_.length != M)
	pivot_ = make([]int, M)

	lu_factor(LU_, pivot_)
}

func lu_solve(b []float64) []float64 {
	x := lu_new_copy(b)

	lu_solve_matrix(LU_, pivot_, x)
	return x
}

func lu_factor(A [][]float64, pivot []int) int {
	M := len(A)
	N := len(A[0])
	minMN := M

	if (M < N) {
		minMN = M
	} else {
		minMN = N
	}

	for j := 0; j < minMN; j++ {
		// find pivot in column j and  test for singularity.

		jp := j

		t := math.Abs(A[j][j])
		for i := j + 1; i < M; i++ {
			ab := math.Abs(A[i][j])
			if ab > t {
				jp = i
				t = ab
			}
		}

		pivot[j] = jp;

		// jp now has the index of maximum element 
		// of column j, below the diagonal

		if A[jp][j] == 0 {
			return 1       // factorization failed because of zero pivot
		}

		if jp != j {
			// swap rows j and jp
			tA := A[j]
			A[j] = A[jp]
			A[jp] = tA
		}

		if j < M - 1 {               // compute elements j+1:M of jth column
			// note A(j,j), was A(jp,p) previously which was
			// guarranteed not to be zero (Label #1)
			//
			recp := 1.0 / A[j][j]

			for k := j + 1; k < M; k++ {
				A[k][j] *= recp
			}
		}


		if j < minMN - 1 {
			// rank-1 update to trailing submatrix:   E = E - x*y;
			//
			// E is the region A(j+1:M, j+1:N)
			// x is the column vector A(j+1:M,j)
			// y is row vector A(j,j+1:N)


			for ii := j + 1; ii < M; ii++ {
				Aii := A[ii]
				Aj := A[j]
				AiiJ := Aii[j]
				for jj := j + 1; jj < N; jj++ {
					Aii[jj] -= AiiJ * Aj[jj]
				}
			}
		}
	}

	return 0
}

func lu_solve_matrix(LU [][]float64, pvt []int, b []float64) {
	M := len(LU)
	N := len(LU[0])
	ii := 0

	for i:= 0; i < M; i++ {
		ip := pvt[i]
		sum := b[ip]

		b[ip] = b[i]
		if ii == 0 {
			for j := ii; j < i; j++ {
				sum -= LU[i][j] * b[j]
			}
		} else {
			if sum == 0.0 {
				ii = i
			}
		}
		b[i] = sum
	}

	for i := N - 1; i >= 0; i-- {
		sum := b[i]
		for j := i + 1; j < N; j++ {
			sum -= LU[i][j] * b[j]
		}
		b[i] = sum / LU[i][i]
	}
}

