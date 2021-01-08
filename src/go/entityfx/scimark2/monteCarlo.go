package scimark2

import (
	"time"
	"math/rand"
)

const SEED = 113

func mc_num_flops(num_samples int) float64 {
	return float64(num_samples) * 4.0
}

func mc_integrate(num_samples int) float64 {
	rand.Seed(time.Now().Unix())
	under_curve := 0
	for count := 0; count < num_samples; count++ {
		x := rand.Float64()
		y := rand.Float64()
		if (x * x) + (y * y) <= 1.0 {
			under_curve++
		}
	}
	return ( float64(under_curve) / float64(num_samples)) * 4.0
}