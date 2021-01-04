package linpack

import (
	"../utils"
)

type LinpackResult struct {
	Norma              float64
	Residual           float64
	NormalisedResidual float64
	Epsilon            float64
	Time               float64
	MFLOPS             float64
	output             string
}

func abs(d float64) float64 {
	if d >= 0 {
		return d
	} else {
		return -d
	}
}

var second_orig float64 = -1

func second() float64 {
	if second_orig == -1 {
		second_orig = float64(utils.MakeTimestamp())
	}
	return (float64(utils.MakeTimestamp()) - second_orig) / 1000.0
}

func RunBenchmark(array_size int, output utils.WriterType) *LinpackResult {
	output.WriteLine("Running Linpack %dx%d in Go", array_size, array_size)
	var mflops_result float64 = 0.0
	var residn_result float64 = 0.0
	var time_result float64 = 0.0
	var eps_result float64 = 0.0

	var a [][]float64 = make([][]float64, array_size)
	for ar := 0; ar < array_size; ar++ {
		a[ar] = make([]float64, array_size)
	}

	var b []float64 = make([]float64, array_size)
	var x []float64 = make([]float64, array_size)
	var ops, total, norma, normx float64 = 0.0, 0.0, 0.0, 0.0
	var resid, time float64
	var n, i, lda = 0, 0, 0
	var ipvt []int = make([]int, array_size)

	lda = array_size
	n = array_size
	nf := float64(n)

	ops = ((2.0e0*nf)*nf*nf)/3.0 + 2.0*(nf*nf)

	norma = matgen(a, lda, n, b)
	time = second()
	dgefa(a, lda, n, ipvt)
	dgesl(a, lda, n, ipvt, b, 0)
	total = second() - time

	for i = 0; i < n; i++ {
		x[i] = b[i]
	}
	norma = matgen(a, lda, n, b)
	for i = 0; i < n; i++ {
		b[i] = -b[i]
	}
	dmxpy(n, b, n, lda, x, a)
	resid = 0.0
	normx = 0.0
	for i = 0; i < n; i++ {
		if resid > abs(b[i]) {
		} else {
			resid = abs(b[i])
		}
		if normx > abs(x[i]) {
		} else {
			normx = abs(x[i])
		}
	}

	eps_result = epslon(1.0)

	residn_result = resid / (float64(n) * norma * normx * eps_result)
	residn_result += 0.005 // for rounding
	residn_result = residn_result * 100
	residn_result /= 100

	time_result = total
	time_result += 0.005 // for rounding
	time_result = time_result * 100
	time_result /= 100

	mflops_result = ops / (1.0e6 * total)
	mflops_result += 0.0005 // for rounding
	mflops_result = mflops_result * 1000
	mflops_result /= 1000

	output.WriteLine("Norma is %v", norma)
	output.WriteLine("Residual is %v", resid)
	output.WriteLine("Normalised residual is %v", residn_result)
	output.WriteLine("Machine result.Eepsilon is %v", eps_result)
	output.WriteLine("x[0]-1 is %v", (x[0] - 1))
	output.WriteLine("x[n-1]-1 is %v", (x[n-1] - 1))
	output.WriteLine("Time is %f", time_result)
	output.WriteLine("MFLOPS: %f", mflops_result)

	result := LinpackResult{norma, resid, residn_result, eps_result, time_result, mflops_result, ""}
	return &result
}

func matgen(a [][]float64, lda int, n int, b []float64) float64 {
	var norma float64
	var i, j = 0, 0
	var iseed []int = []int{1, 2, 3, 1325}

	/* Magic numbers from original Linpack source */

	norma = 0.0
	/*
	 * Next two for() statements switched. Solver wants matrix in column order.
	 * --dmd 3/3/97
	 */
	for i = 0; i < n; i++ {
		for j = 0; j < n; j++ {
			a[j][i] = lran(iseed) - 0.5
			if a[j][i] > norma {
				norma = a[j][i]
			}
		}
	}
	for i = 0; i < n; i++ {
		b[i] = 0.0
	}
	for j = 0; j < n; j++ {
		for i = 0; i < n; i++ {
			b[i] += a[j][i]
		}
	}

	return norma
}

func lran(seed []int) float64 {
	var m1, m2, m3, m4, ipw2 = 0, 0, 0, 0, 0
	var it1, it2, it3, it4 = 0, 0, 0, 0
	var r, result float64

	m1 = 494
	m2 = 322
	m3 = 2508
	m4 = 2549
	ipw2 = 4096

	r = 1.0 / float64(ipw2)

	it4 = seed[3] * m4
	it3 = it4 / ipw2
	it4 = it4 - ipw2*it3
	it3 = it3 + seed[2]*m4 + seed[3]*m3
	it2 = it3 / ipw2
	it3 = it3 - ipw2*it2
	it2 = it2 + seed[1]*m4 + seed[2]*m3 + seed[3]*m2
	it1 = it2 / ipw2
	it2 = it2 - ipw2*it1
	it1 = it1 + seed[0]*m4 + seed[1]*m3 + seed[2]*m2 + seed[3]*m1
	it1 = it1 % ipw2

	seed[0] = it1
	seed[1] = it2
	seed[2] = it3
	seed[3] = it4

	result = r * (float64(it1) + r*(float64(it2)+r*(float64(it3)+r*float64(it4))))

	return result
}

func dgefa(a [][]float64, lda int, n int, ipvt []int) int {
	var col_k, col_j []float64
	var t float64
	var j, k, kp1, l, nm1 = 0, 0, 0, 0, 0
	var info = 0

	// gaussian elimination with partial pivoting

	nm1 = n - 1

	if nm1 >= 0 {
		for k = 0; k < nm1; k++ {
			col_k = a[k]
			kp1 = k + 1

			// find l = pivot index

			l = idamax(n-k, col_k, k, 1) + k
			ipvt[k] = l

			// zero pivot implies this column already triangularized

			if col_k[l] != 0 {

				// interchange if necessary

				if l != k {
					t = col_k[l]
					col_k[l] = col_k[k]
					col_k[k] = t
				}

				// compute multipliers

				t = -1.0 / col_k[k]
				dscal(n-(kp1), t, col_k, kp1, 1)

				// row elimination with column indexing

				for j = kp1; j < n; j++ {
					col_j = a[j]
					t = col_j[l]
					if l != k {
						col_j[l] = col_j[k]
						col_j[k] = t
					}
					daxpy(n-(kp1), t, col_k, kp1, 1, col_j, kp1, 1)
				}
			} else {
				info = k
			}
		}
	}
	ipvt[n-1] = n - 1
	if a[(n - 1)][(n-1)] == 0 {
		info = n - 1
	}

	return info
}

func dgesl(a [][]float64, lda int, n int, ipvt []int, b []float64, job int) {
	var t float64
	var k, kb, l, nm1, kp1 int = 0, 0, 0, 0, 0

	nm1 = n - 1

	if job == 0 {

		// job = 0 , solve a * x = b. first solve l*y = b

		if nm1 >= 1 {
			for k = 0; k < nm1; k++ {
				l = ipvt[k]
				t = b[l]
				if l != k {
					b[l] = b[k]
					b[k] = t
				}
				kp1 = k + 1
				daxpy(n-(kp1), t, a[k], kp1, 1, b, kp1, 1)
			}
		}

		// now solve u*x = y

		for kb = 0; kb < n; kb++ {
			k = n - (kb + 1)
			b[k] /= a[k][k]
			t = -b[k]
			daxpy(k, t, a[k], 0, 1, b, 0, 1)
		}
	} else {

		// job = nonzero, solve trans(a) * x = b. first solve trans(u)*y = b

		for k = 0; k < n; k++ {
			t = ddot(k, a[k], 0, 1, b, 0, 1)
			b[k] = (b[k] - t) / a[k][k]
		}

		// now solve trans(l)*x = y

		if nm1 >= 1 {
			for kb = 1; kb < nm1; kb++ {
				k = n - (kb + 1)
				kp1 = k + 1
				b[k] += ddot(n-(kp1), a[k], kp1, 1, b, kp1, 1)
				l = ipvt[k]
				if l != k {
					t = b[l]
					b[l] = b[k]
					b[k] = t
				}
			}
		}
	}
}

func daxpy(n int, da float64, dx []float64, dx_off int, incx int, dy []float64, dy_off int, incy int) {
	var ix, iy int

	if (n > 0) && (da != 0) {
		if incx != 1 || incy != 1 {
			// code for unequal increments or equal increments not equal to 1

			ix = 0
			iy = 0
			if incx < 0 {
				ix = (-n + 1) * incx
			}
			if incy < 0 {
				iy = (-n + 1) * incy
			}
			for i := 0; i < n; i++ {
				dy[iy+dy_off] += da * dx[ix+dx_off]
				ix += incx
				iy += incy
			}
			return
		} else {
			// code for both increments equal to 1

			for i := 0; i < n; i++ {
				dy[i+dy_off] += da * dx[i+dx_off]
			}
		}
	}
}

func ddot(n int, dx []float64, dx_off int, incx int, dy []float64, dy_off int, incy int) float64 {
	var dtemp float64 = 0.0
	var ix, iy int
	if n > 0 {
		if incx != 1 || incy != 1 {
			// code for unequal increments or equal increments not equal to 1

			ix = 0
			iy = 0
			if incx < 0 {
				ix = (-n + 1) * incx
			}
			if incy < 0 {
				iy = (-n + 1) * incy
			}
			for i := 0; i < n; i++ {
				dtemp += dx[ix+dx_off] * dy[iy+dy_off]
				ix += incx
				iy += incy
			}
		} else {
			// code for both increments equal to 1

			for i := 0; i < n; i++ {
				dtemp += dx[i+dx_off] * dy[i+dy_off]
			}
		}
	}
	return (dtemp)
}

func dscal(n int, da float64, dx []float64, dx_off int, incx int) {
	var nincx int

	if n > 0 {
		if incx != 1 {
			// code for increment not equal to 1

			nincx = n * incx
			for i := 0; i < nincx; i += incx {
				dx[i+dx_off] *= da
			}
		} else {
			// code for increment equal to 1

			for i := 0; i < n; i++ {
				dx[i+dx_off] *= da
			}
		}
	}
}

func idamax(n int, dx []float64, dx_off int, incx int) int {
	var dmax, dtemp float64
	var ix, itemp int

	if n < 1 {
		itemp = -1
	} else if n == 1 {
		itemp = 0
	} else if incx != 1 {

		// code for increment not equal to 1

		dmax = abs(dx[0+dx_off])
		ix = 1 + incx
		for i := 1; i < n; i++ {
			dtemp = abs(dx[ix+dx_off])
			if dtemp > dmax {
				itemp = i
				dmax = dtemp
			}
			ix += incx
		}
	} else {
		// code for increment equal to 1

		itemp = 0
		dmax = abs(dx[0+dx_off])
		for i := 1; i < n; i++ {
			dtemp = abs(dx[i+dx_off])
			if dtemp > dmax {
				itemp = i
				dmax = dtemp
			}
		}
	}
	return (itemp)
}

func epslon(x float64) float64 {
	var a, b, c, eps float64

	a = 4.0e0 / 3.0e0
	eps = 0
	for eps == 0 {
		b = a - 1.0
		c = b + b + b
		eps = abs(c - 1.0)
	}
	return eps * abs(x)
}

func dmxpy(n1 int, y []float64, n2 int, ldm int, x []float64, m [][]float64) {
	// cleanup odd vector
	for j := 0; j < n2; j++ {
		for i := 0; i < n1; i++ {
			y[i] += x[j] * m[j][i]
		}
	}
}
