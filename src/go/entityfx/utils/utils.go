package utils

import (
	"math/rand"
	"time"
)

func Average(xs []float64) float64 {
	total := 0.0
	for _, v := range xs {
		total += v
	}
	return total / float64(len(xs))
}

func RandomIntArray(size int32, max int32) []int32 {
	rand.Seed(time.Now().Unix())
	var ar = make([]int32, size)
	var i int32 = 0
	for ; i < size; i++ {
		ar[i] = rand.Int31n(max)
	}
	return ar
}

func RandomLongArray(size int32, max int64) []int64 {
	rand.Seed(time.Now().Unix())
	var ar = make([]int64, size)
	var i int32 = 0
	for ; i < size; i++ {
		ar[i] = rand.Int63n(max)
	}
	return ar
}

func MakeTimestamp() int64 {
	t := time.Now()
	return int64(time.Nanosecond) * t.UnixNano() / int64(time.Millisecond)
}
