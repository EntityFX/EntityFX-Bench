package whetstone

import (
	"math"

	"github.com/EntityFX/EntityFX-Bench/utils"
	//"strings"
)

type WhetstoneResult struct {
	MWIPS    float64
	TimeUsed float64
	Output   string
}

type Whetstone struct {
	loop_time   [9]float64
	loop_mops   [9]float64
	loop_mflops [9]float64
	results     [9]float64
	headings    [9]string
	timeUsed    float64
	mwips       float64
	check       float64
}

func Bench(getinput bool, output utils.WriterType) *WhetstoneResult {
	count := 10
	calibrate := 1
	var xtra int64 = 1
	//var endit int
	//var section int
	var x100 int64 = 100
	var duration int64 = 100
	whetstone := &Whetstone{}
	//general := [8]string{}
	output.WriteLine("%s Precision Go Whetstone Benchmark\n", "Double")
	if !getinput {
		output.WriteLine("No run time input data\n")
	} else {
		output.WriteLine("With run time input data\n")
	}
	output.WriteLine("Calibrate")
	for ok := true; ok; ok = count > 0 {
		whetstone.timeUsed = 0.0
		whetstones(whetstone, xtra, x100, calibrate, output)
		output.WriteLine("%11.2f Seconds %10d   Passes (x 100)", whetstone.timeUsed, xtra)
		calibrate = calibrate + 1
		count = count - 1
		if whetstone.timeUsed > 2.0 {
			count = 0
		} else {
			xtra = xtra * 5
		}
	}
	if whetstone.timeUsed > 0 {
		xtra = int64(float64(duration*xtra) / whetstone.timeUsed)
	}
	if xtra < 1 {
		xtra = 1
	}
	calibrate = 0
	output.WriteLine("\nUse %d  passes (x 100)", xtra)
	output.WriteLine("\n          %s Precision Go Whetstone Benchmark", "Double")
	output.WriteLine("\n                  %s", "")
	output.WriteLine("\nLoop content                  Result              MFLOPS " + "     MOPS   Seconds\n")
	whetstone.timeUsed = 0.0
	whetstones(whetstone, xtra, x100, calibrate, output)
	output.Write("MWIPS            ")
	if whetstone.timeUsed > 0 {
		whetstone.mwips = float64(xtra*x100) / (10.0 * whetstone.timeUsed)
	} else {
		whetstone.mwips = 0.0
	}
	output.WriteLine("%39.3f%20.3f", whetstone.mwips, whetstone.timeUsed)
	if whetstone.check == 0 {
		output.WriteLine("Wrong answer  ")
	}
	return &WhetstoneResult{whetstone.mwips, whetstone.timeUsed, output.GetOutput()}
}

func whetstones(whetstone *Whetstone, xtra int64, x100 int64, calibrate int, output utils.WriterType) {
	var i, ix, n1mult int64 = 0, 0, 10
	var x, y, z float64 = 0.0, 0.0, 0.0
	var j, k, l int64 = 0, 0, 0
	e1 := [4]float64{1.0, -1.0, -1.0, -1.0}
	var timea, timeb float64 = 0.0, 0.0
	var t float64 = .49999975
	t0 := t
	var t1, t2 float64 = .50000025, 2.0
	whetstone.check = 0.0
	var n1, n2, n3, n4, n5, n6, n7, n8 int64 = 12 * x100, 14 * x100, 345 * x100, 210 * x100, 32 * x100, 899 * x100, 616 * x100, 93 * x100

	start := float64(utils.MakeTimestamp())

	timea = start / 1000.0
	for ix = 0; ix < xtra; ix++ {
		for i = 0; i < (n1 * n1mult); i++ {
			e1[0] = ((e1[0] + e1[1] + e1[2]) - e1[3]) * t
			e1[1] = (((e1[0] + e1[1]) - e1[2]) + e1[3]) * t
			e1[2] = ((e1[0] - e1[1]) + e1[2] + e1[3]) * t
			e1[3] = (((-e1[0]) + e1[1] + e1[2]) + e1[3]) * t
		}
		t = 1.0 - t
	}
	t = t0
	timeb = ((float64(utils.MakeTimestamp()) / 1000.0) - timea) / float64(n1mult)

	pout(whetstone, "N1 floating point", float64(n1*16*xtra), 1, e1[3], timeb, calibrate, 1, output)
	timea = start / 1000.0

	for ix = 0; ix < xtra; ix++ {
		for i = 0; i < n2; i++ {
			pa(&e1, t, t2)
		}
		t = 1.0 - t
	}
	t = t0

	timeb = (float64(utils.MakeTimestamp()) / 1000.0) - timea
	pout(whetstone, "N2 floating point", float64(n2*96*xtra), 1, e1[3], timeb, calibrate, 2, output)
	j = 1
	timea = start / 1000.0

	for ix = 0; ix < xtra; ix++ {
		for i = 0; i < n3; i++ {
			if j == 1 {
				j = 2
			} else {
				j = 3
			}
			if j > 2 {
				j = 0
			} else {
				j = 1
			}
			if j < 1 {
				j = 1
			} else {
				j = 0
			}
		}
	}

	timeb = (float64(utils.MakeTimestamp()) / 1000.0) - timea
	pout(whetstone, "N3 if then else  ", float64(n3*3*xtra), 2, float64(j), timeb, calibrate, 3, output)
	j = 1
	k = 2
	l = 3
	timea = start / 1000.0

	for ix = 0; ix < xtra; ix++ {
		for i = 0; i < n4; i++ {
			j = j * (k - j) * (l - k)
			k = (l * k) - ((l - j) * k)
			l = (l - k) * (k + j)
			e1[l-2] = float64(j + k + l)
			e1[k-2] = float64(j * k * l)
		}
	}

	timeb = (float64(utils.MakeTimestamp()) / 1000.0) - timea
	x = e1[0] + e1[1]
	pout(whetstone, "N4 fixed point   ", float64(n4*15*xtra), 2, x, timeb, calibrate, 4, output)
	x = .5
	y = .5
	timea = start / 1000.0

	for ix = 0; ix < xtra; ix++ {
		for i = 1; i < n5; i++ {
			x = t * math.Atan(t2*math.Sin(x)*math.Cos(x)/(math.Cos(x+y)+math.Cos(x-y)-1.0))
			y = t * math.Atan(t2*math.Sin(y)*math.Cos(y)/(math.Cos(x+y)+math.Cos(x-y)-1.0))
		}
		t = 1.0 - t
	}
	t = t0

	timeb = (float64(utils.MakeTimestamp()) / 1000.0) - timea
	pout(whetstone, "N5 sin,cos etc.  ", float64(n5*26*xtra), 2, y, timeb, calibrate, 5, output)
	x = 1.0
	y = 1.0
	z = 1.0
	timea = start / 1000.0

	for ix = 0; ix < xtra; ix++ {
		for i = 0; i < n6; i++ {
			p3(&x, &y, &z, t, t1, t2)
		}
	}

	timeb = (float64(utils.MakeTimestamp()) / 1000.0) - timea
	pout(whetstone, "N6 floating point", float64(n6*6*xtra), 1, z, timeb, calibrate, 6, output)
	j = 0
	k = 1
	l = 2
	e1[0] = 1.0
	e1[1] = 2.0
	e1[2] = 3.0
	timea = start / 1000.0

	for ix = 0; ix < xtra; ix++ {
		for i = 0; i < n7; i++ {
			po(&e1, j, k, l)
		}
	}

	timeb = (float64(utils.MakeTimestamp()) / 1000.0) - timea
	pout(whetstone, "N7 assignments   ", float64(n7*3*xtra), 2, e1[2], timeb, calibrate, 7, output)
	x = .75
	timea = start / 1000.0

	for ix = 0; ix < xtra; ix++ {
		for i = 0; i < n8; i++ {
			x = math.Sqrt(math.Exp(math.Log(x) / t1))
		}
	}

	timeb = (float64(utils.MakeTimestamp()) / 1000.0) - timea
	pout(whetstone, "N8 exp,sqrt etc. ", float64(n8*4*xtra), 2, x, timeb, calibrate, 8, output)
}

func pa(e *[4]float64, t float64, t2 float64) {
	var j int64 = 0
	for j = 0; j < 6; j++ {
		e[0] = ((e[0] + e[1] + e[2]) - e[3]) * t
		e[1] = (((e[0] + e[1]) - e[2]) + e[3]) * t
		e[2] = ((e[0] - e[1]) + e[2] + e[3]) * t
		e[3] = (((-e[0]) + e[1] + e[2]) + e[3]) / t2
	}
	return
}

func po(e1 *[4]float64, j int64, k int64, l int64) {
	e1[j] = e1[k]
	e1[k] = e1[l]
	e1[l] = e1[j]
}

func p3(x *float64, y *float64, z *float64, t float64, t1 float64, t2 float64) {
	x = y
	y = z
	*x = t * (*x + *y)
	*y = t1 * (*x + *y)
	*z = (*x + *y) / t2
}

func pout(whetstone *Whetstone, title string, ops float64, typen int32, checknum float64, time float64, calibrate int, section int32, output utils.WriterType) {
	var mops, mflops float64 = 0.0, 0.0
	whetstone.check = whetstone.check + checknum
	whetstone.loop_time[section] = time
	whetstone.headings[section] = title
	whetstone.timeUsed = whetstone.timeUsed + time
	if calibrate == 1 {
		whetstone.results[section] = checknum
	}
	if calibrate == 0 {
		output.Write("%-18s %24.17f    ", whetstone.headings[section], whetstone.results[section])
		if typen == 1 {
			if time > 0 {
				mflops = ops / (1000000.0 * time)
			} else {
				mflops = 0.0
			}
			whetstone.loop_mops[section] = 99999.0
			whetstone.loop_mflops[section] = mflops
			output.WriteLine("%9.3f           %9.3f", whetstone.loop_mflops[section], whetstone.loop_time[section])
		} else {
			if time > 0 {
				mops = ops / (1000000.0 * time)
			} else {
				mops = 0.0
			}
			whetstone.loop_mops[section] = float64(mops)
			whetstone.loop_mflops[section] = 0.0
			output.WriteLine("           %9.3f%9.3f", whetstone.loop_mops[section], whetstone.loop_time[section])
		}
	}
	return
}
