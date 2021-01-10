package scimark2

func sor_num_flops(M int, N int, num_iterations int) float64 {
	md := float64(M)
	nd := float64(N)
	num_iterD := float64(num_iterations)
	return (md - 1.0) * (nd - 1.0) * num_iterD * 6.0
}

func sor_execute(omega float64, G [][]float64, num_iterations int) {
	M := len(G)
	N := len(G[0])
	omega_over_four := omega * .25
	var one_minus_omega float64 = 1.0 - omega
	mm1 := M - 1
	nm1 := N - 1
	for p := 0; p < num_iterations; p++ {
		for i := 1; i < mm1; i++ {
			gi := G[i]
			gim1 := G[i - 1]
			gip1 := G[i + 1]
			for j := 1; j < nm1; j++ {
				gi[j] = (omega_over_four * (((gim1[j] + gip1[j] + gi[j - 1]) + gi[j + 1]))) + (one_minus_omega * gi[j])
			}
		}
	}
}