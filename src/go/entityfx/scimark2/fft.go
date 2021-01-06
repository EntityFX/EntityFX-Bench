package scimark2

import (
	"math/rand"
	"time"
	"math"
)

func fft_num_flops(N int) float64 {
	Nd := float64(N)
	logN := float64(fft_log2(N))

	return (5.0 * Nd - 2) * logN + 2 * (Nd + 1)
}

func fft_transform(data []float64) {
	fft_transform_internal(data, -1)
}

func fft_inverse(data []float64) {
	fft_transform_internal(data, +1)
	// Normalize
	nd := len(data)
	n := nd / 2
	norm := 1.0 / float64(n)
	for i := 0; i < nd; i++ {
		data[i] *= norm
	}
}

func fft_makeRandom(n int) []float64{
	rand.Seed(time.Now().Unix())
	nd := 2 * n
	data := make([]float64, nd)
	for i := 0; i < nd; i++ {
		data[i] = rand.Float64()
	}
	return data
}

func fft_log2(n int) int {
	log := 0
	for k := 1; k < n; k *= 2 {
		log++
	}
	if n != (1 << log) {
		return -1//, errors.New("FFT: Data length is not a power of 2!: " + string(n))
	}
	return log
}

func fft_transform_internal(data []float64, direction int) {
	if len(data) == 0 { return }
	n := len(data) / 2
	if n == 1 { return }         // Identity operation!
	logn := fft_log2(n)

	/* bit reverse the input data for decimation in time algorithm */
	fft_bitreverse(data);

	/* apply fft recursion */
	/* this loop executed log2(N) times */
	
	dual := 1			
	for bit := 0 ; bit < logn; bit++ {
		dual *= 2

		w_real := 1.0
		w_imag := 0.0

		theta := 2.0 * float64(direction) * math.Pi / (2.0 * float64(dual))
		s := math.Sin(theta)
		t := math.Sin(theta / 2.0)
		s2 := 2.0 * t * t

		/* a = 0 */
		for b := 0; b < n; b += 2 * dual {
			i := 2 * b
			j := 2 * (b + dual)

			wd_real := data[j]
			wd_imag := data[j + 1]

			data[j] = data[i] - wd_real
			data[j + 1] = data[i + 1] - wd_imag
			data[i] += wd_real
			data[i + 1] += wd_imag
		}

		/* a = 1 .. (dual-1) */
		for a := 1; a < dual; a++ {
			/* trignometric recurrence for w-> exp(i theta) w */
			{
				tmp_real := w_real - s * w_imag - s2 * w_real
				tmp_imag := w_imag + s * w_real - s2 * w_imag
				w_real = tmp_real;
				w_imag = tmp_imag;
			}
			for b := 0; b < n; b += 2 * dual {
				i := 2 * (b + a)
				j := 2 * (b + a + dual)

				z1_real := data[j]
				z1_imag := data[j + 1]

				wd_real := w_real * z1_real - w_imag * z1_imag
				wd_imag := w_real * z1_imag + w_imag * z1_real

				data[j] = data[i] - wd_real
				data[j + 1] = data[i + 1] - wd_imag
				data[i] += wd_real
				data[i + 1] += wd_imag
			}
		}
	}
}

func fft_bitreverse(data []float64) {
	/* This is the Goldrader bit-reversal algorithm */
	n := len(data)
	nm1 := n - 1
	i := 0
	j := 0
	for ; i < nm1; i++ {

		//int ii = 2*i;
		ii := i << 1

		//int jj = 2*j;
		jj := j << 1

		//int k = n / 2 ;
		k := n >> 1

		if i < j {
			tmp_real := data[ii]
			tmp_imag := data[ii + 1]
			data[ii] = data[jj]
			data[ii + 1] = data[jj + 1]
			data[jj] = tmp_real
			data[jj + 1] = tmp_imag
		}

		for k <= j {
			//j = j - k ;
			j -= k

			//k = k / 2 ; 
			k >>= 1
		}
		j += k
	}
}