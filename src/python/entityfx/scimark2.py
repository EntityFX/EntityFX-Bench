import math
import random
import time

from entityfx.writer import Writer


class Constants:

    RESOLUTION_DEFAULT = 2.0

    RANDOM_SEED = 101010

    FFT_SIZE = 1024

    SOR_SIZE = 100

    SPARSE_SIZE_M = 1000

    SPARSE_SIZE_NZ = 5000

    LU_SIZE = 100

    LG_FFT_SIZE = 1048576

    LG_SOR_SIZE = 1000

    LG_SPARSE_SIZE_M = 100000

    LG_SPARSE_SIZE_NZ = 1000000

    LG_LU_SIZE = 1000

    TINY_FFT_SIZE = 16

    TINY_SOR_SIZE = 10

    TINY_SPARSE_SIZE_M = 10

    TINY_SPARSE_SIZE_N = 10

    TINY_SPARSE_SIZE_NZ = 50

    TINY_LU_SIZE = 10


class Kernel:

    @staticmethod
    def measureFFT(n: int, mintime: float) -> float:
        x = Kernel.__randomVector(2 * n)
        oldx = Kernel.__newVectorCopy(x)
        cycles = 1

        while True:
            start = time.time()
            i = 0
            while i < cycles:
                FFT.transform(x)
                FFT.inverse(x)
                i += 1
            elapsed = time.time() - start
            if (elapsed >= mintime):
                break
            cycles *= (2)

        EPS = 1.0e-10
        if ((FFT.test(x) / (n)) > EPS):
            return .0
        return FFT.num_flops(n) * cycles / elapsed * 1.0e-6

    @staticmethod
    def measureSOR(n: int, min_time: float) -> float:
        g = Kernel.__randomMatrix(n, n)
        cycles = 1
        while True:
            start = time.time()
            SOR.execute(1.25, g, cycles)
            elapsed = time.time() - start
            if (elapsed >= min_time):
                break
            cycles *= 2
        return SOR.num_flops(n, n, cycles) / elapsed * 1.0e-6

    @staticmethod
    def measureMonteCarlo(min_time: float) -> float:
        cycles = 1
        while True:
            start = time.time()
            MonteCarlo.integrate(cycles)
            elapsed = time.time() - start
            if (elapsed >= min_time):
                break
            cycles *= 2
        return MonteCarlo.num_flops(cycles) / elapsed * 1.0e-6

    @staticmethod
    def measureSparseMatmult(n: int, nz: int, min_time: float) -> float:
        x = Kernel.__randomVector(n)
        y = [0] * n
        nr = math.floor(nz / n)
        anz = nr * n
        val = Kernel.__randomVector(anz)
        col = [0] * anz
        row = [0] * (n + 1)
        row[0] = 0
        r_ = 0
        while r_ < n:
            rowr = row[r_]
            row[r_ + 1] = (rowr + nr)
            step = math.floor(r_ / nr)
            if (step < 1):
                step = 1
            i = 0
            while i < nr:
                col[rowr + i] = (i * step)
                i += 1
            r_ += 1
        cycles = 1
        while True:
            start = time.time()
            SparseCompRow.matmult(y, val, row, col, x, cycles)
            elapsed = time.time() - start
            if (elapsed >= min_time):
                break
            cycles *= 2
        return SparseCompRow.num_flops(n, nz, cycles) / elapsed * 1.0e-6

    @staticmethod
    def measureLU(n: int, min_time: float) -> float:
        a = Kernel.__randomMatrix(n, n)
        lu = [None] * n
        i = 0
        while i < n:
            lu[i] = [0] * n
            i += 1
        pivot = [0] * n

        cycles = 1
        while True:
            start = time.time()
            i = 0
            while i < cycles:
                Kernel.__copyMatrix(lu, a)
                LU.factor(lu, pivot)
                i += 1
            elapsed = time.time() - start
            if (elapsed >= min_time):
                break
            cycles *= 2
        b = Kernel.__randomVector(n)
        x = Kernel.__newVectorCopy(b)
        LU.solve(lu, pivot, x)
        EPS = 1.0e-12
        if ((Kernel.__normabs(b, Kernel.__matvec(a, x)) / (n)) > EPS):
            return .0
        return LU.num_flops(n) * cycles / elapsed * 1.0e-6

    @staticmethod
    def __newVectorCopy(x):
        n = len(x)
        y = [0] * n
        i = 0
        while i < n:
            y[i] = x[i]
            i += 1
        return y

    @staticmethod
    def __copyVector(b, a) -> None:
        n = len(a)
        i = 0
        while i < n:
            b[i] = a[i]
            i += 1

    @staticmethod
    def __normabs(x, y) -> float:
        n = len(x)
        sum0_ = .0
        i = 0
        while i < n:
            sum0_ += math.fabs(x[i] - y[i])
            i += 1
        return sum0_

    @staticmethod
    def __copyMatrix(b, a) -> None:
        m = len(a)
        n = len(a[0])
        remainder = n & 3
        i = 0
        while i < m:
            bi = b[i]
            ai = a[i]
            j = 0
            while j < remainder:
                bi[j] = ai[j]
                j += 1
            j = remainder
            while j < n:
                bi[j] = ai[j]
                bi[j + 1] = ai[j + 1]
                bi[j + 2] = ai[j + 2]
                bi[j + 3] = ai[j + 3]
                j += 4
            i += 1

    @staticmethod
    def __randomMatrix(m: int, n: int):
        a = [None] * m
        i = 0
        while i < n:
            a[i] = [0] * n
            j = 0
            while j < n:
                a[i][j] = random.random()
                j += 1
            i += 1
        return a

    @staticmethod
    def __randomVector(n: int):
        a = [0] * n
        i = 0
        while i < n:
            a[i] = random.random()
            i += 1
        return a

    @staticmethod
    def __matvec(a, x):
        n = len(x)
        y = [0] * n
        Kernel.__matvec0(a, x, y)
        return y

    @staticmethod
    def __matvec0(a, x, y) -> None:
        m = len(a)
        n = len(a[0])
        i = 0
        while i < m:
            sum0_ = .0
            ai = a[i]
            j = 0
            while j < n:
                sum0_ += (ai[j] * x[j])
                j += 1
            y[i] = sum0_
            i += 1


class FFT:

    @staticmethod
    def num_flops(n: int) -> float:
        nd = n
        logn = FFT._log2(n)
        return ((((5.0 * nd) - (2))) * logn) + ((2) * ((nd + (1))))

    @staticmethod
    def transform(data) -> None:
        FFT._transform_internal(data, -1)

    @staticmethod
    def inverse(data) -> None:
        FFT._transform_internal(data, 1)
        nd = len(data)
        n = math.floor(nd / 2)
        norm = (1) / ((n))
        i = 0
        while i < nd:
            data[i] *= norm
            i += 1

    @staticmethod
    def test(data) -> float:
        nd = len(data)
        copy = data.copy()
        FFT.transform(data)
        FFT.inverse(data)
        diff = .0
        i = 0
        while i < nd:
            d = data[i] - copy[i]
            diff += (d * d)
            i += 1
        return math.sqrt(diff / (nd))

    @staticmethod
    def makeRandom(n: int):
        nd = 2 * n
        data = [0] * nd
        i = 0
        while i < nd:
            data[i] = random.random()
            i += 1
        return data

    @staticmethod
    def main(args) -> None:
        if (len(args) == 0):
            n = 1024
            print(("n=" + (chr(n)) + " => RMS Error=") +
                  (FFT.test(FFT.makeRandom(n))), flush=True)
        i = 0
        while i < len(args):
            n = int(args[i])
            print(("n=" + (chr(n)) + " => RMS Error=") +
                  (FFT.test(FFT.makeRandom(n))), flush=True)
            i += 1

    @staticmethod
    def _log2(n: int) -> int:
        log0_ = 0
        k = 1
        while k < n:
            pass
            k *= 2
            log0_ += 1
        if (n != ((1 << log0_))):
            raise RuntimeError(
                "FFT: Data length is not a power of 2!: " + (chr(n)))
        return log0_

    @staticmethod
    def _transform_internal(data, direction: int) -> None:
        if (len(data) == 0):
            return
        n = len(data) / 2
        if (n == 1):
            return
        logn = FFT._log2(n)
        FFT._bitreverse(data)
        bit = 0
        dual = 1
        while bit < logn:
            w_real = 1.0
            w_imag = .0
            theta = (2.0 * (direction) * math.pi) / ((2.0 * (dual)))
            s = math.sin(theta)
            t = math.sin(theta / 2.0)
            s2 = 2.0 * t * t
            b = 0
            while b < n:
                i = 2 * b
                j = 2 * ((b + dual))
                wd_real = data[j]
                wd_imag = data[j + 1]
                data[j] = (data[i] - wd_real)
                data[j + 1] = (data[i + 1] - wd_imag)
                data[i] += wd_real
                data[i + 1] += wd_imag
                b += (2 * dual)
            a = 1
            while a < dual:
                tmp_real = w_real - (s * w_imag) - (s2 * w_real)
                tmp_imag = (w_imag + (s * w_real)) - (s2 * w_imag)
                w_real = tmp_real
                w_imag = tmp_imag
                b = 0
                while b < n:
                    i = 2 * ((b + a))
                    j = 2 * ((b + a + dual))
                    z1_real = data[j]
                    z1_imag = data[j + 1]
                    wd_real = (w_real * z1_real) - (w_imag * z1_imag)
                    wd_imag = (w_real * z1_imag) + (w_imag * z1_real)
                    data[j] = (data[i] - wd_real)
                    data[j + 1] = (data[i + 1] - wd_imag)
                    data[i] += wd_real
                    data[i + 1] += wd_imag
                    b += (2 * dual)
                a += 1
            bit += 1
            dual *= 2

    @staticmethod
    def _bitreverse(data) -> None:
        n = len(data) // 2
        nm1 = n - 1
        i = 0
        j = 0
        while i < nm1:
            ii = i << 1
            jj = j << 1
            k = n >> 1
            if (i < j):
                tmp_real = data[ii]
                tmp_imag = data[ii + 1]
                data[ii] = data[jj]
                data[ii + 1] = data[jj + 1]
                data[jj] = tmp_real
                data[jj + 1] = tmp_imag
            while k <= j:
                j -= k
                k >>= 1
            j += k
            i += 1


class LU:

    @staticmethod
    def num_flops(n: int) -> float:
        nd = n
        return (((2.0 * nd * nd) * nd) / 3.0)

    @staticmethod
    def _new_copy(x):
        n = len(x)
        t = [0] * n
        i = 0
        while i < n:
            t[i] = x[i]
            i += 1
        return t

    @staticmethod
    def _new_copy0(a):
        m = len(a)
        n = len(a[0])
        t = [0] * m
        i = 0
        while i < m:
            ti = t[i]
            ai = a[i]
            j = 0
            while j < n:
                ti[j] = ai[j]
                j += 1
            i += 1
        return t

    @staticmethod
    def new_copy(x):
        n = len(x)
        t = [0] * n
        i = 0
        while i < n:
            t[i] = x[i]
            i += 1
        return t

    @staticmethod
    def _insert_copy(b, a) -> None:
        m = len(a)
        n = len(a[0])
        remainder = n & 3
        i = 0
        while i < m:
            bi = b[i]
            ai = a[i]
            j = 0
            while j < remainder:
                bi[j] = ai[j]
                j += 1
            j = remainder
            while j < n:
                bi[j] = ai[j]
                bi[j + 1] = ai[j + 1]
                bi[j + 2] = ai[j + 2]
                bi[j + 3] = ai[j + 3]
                j += 4
            i += 1

    def getLU(self):
        return LU._new_copy0(self.__lu_)

    def getPivot(self):
        return LU.new_copy(self.__pivot_)

    def __init__(self, a) -> None:
        self.__lu_ = None
        self.__pivot_ = None
        m = len(a)
        n = len(a[0])
        self.__lu_ = [0] * m
        LU._insert_copy(self.__lu_, a)
        self.__pivot_ = [0] * m
        LU.factor(self.__lu_, self.__pivot_)

    def solve0(self, b):
        x = LU._new_copy(b)
        LU.solve(self.__lu_, self.__pivot_, x)
        return x

    @staticmethod
    def factor(a, pivot) -> int:
        n = len(a)
        m = len(a[0])
        minmn = min(m, n)
        j = 0
        while j < minmn:
            jp = j
            t = math.fabs(a[j][j])
            i = j + 1
            while i < m:
                ab = math.fabs(a[i][j])
                if (ab > t):
                    jp = i
                    t = ab
                i += 1
            pivot[j] = jp
            if (a[jp][j] == 0):
                return 1
            if (jp != j):
                ta = a[j]
                a[j] = a[jp]
                a[jp] = ta
            if (j < (m - 1)):
                recp = 1.0 / a[j][j]
                k = j + 1
                while k < m:
                    a[k][j] *= recp
                    k += 1
            if (j < (minmn - 1)):
                ii = j + 1
                while ii < m:
                    aii = a[ii]
                    aj = a[j]
                    aiij = aii[j]
                    jj = j + 1
                    while jj < n:
                        aii[jj] -= (aiij * aj[jj])
                        jj += 1
                    ii += 1
            j += 1
        return 0

    @staticmethod
    def solve(lu, pvt, b) -> None:
        m = len(lu)
        n = len(lu[0])
        ii = 0
        i = 0
        j = 0
        while i < m:
            ip = pvt[i]
            sum0_ = b[ip]
            b[ip] = b[i]
            if (ii == 0):
                j = ii
                while j < i:
                    sum0_ -= (lu[i][j] * b[j])
                    j += 1
            elif (sum0_ == .0):
                ii = i
            b[i] = sum0_
            i += 1
        for i in range(n - 1, -1, -1):
            sum0_ = b[i]
            j = i + 1
            while j < n:
                sum0_ -= (lu[i][j] * b[j])
                j += 1
            b[i] = (sum0_ / lu[i][i])


class MonteCarlo:

    __seed = 113

    @staticmethod
    def num_flops(num_samples: int) -> float:
        return ((num_samples)) * 4.0

    @staticmethod
    def integrate(num_samples: int) -> float:
        under_curve = 0
        count = 0
        while count < num_samples:
            x = random.random()
            y = random.random()
            if (((x * x) + (y * y)) <= 1.0):
                under_curve += 1
            count += 1
        return (((under_curve) / (num_samples))) * 4.0


class SOR:

    @staticmethod
    def num_flops(m: int, n: int, num_iterations: int) -> float:
        md = m
        nd = n
        num_iterd = num_iterations
        return (((md - (1))) * ((nd - (1))) * num_iterd) * 6.0

    @staticmethod
    def execute(omega: float, g, num_iterations: int) -> None:
        m = len(g)
        n = len(g[0])
        omega_over_four = omega * .25
        one_minus_omega = 1.0 - omega
        mm1 = m - 1
        nm1 = n - 1
        p = 0
        while p < num_iterations:
            i = 1
            while i < mm1:
                gi = g[i]
                gim1 = g[i - 1]
                gip1 = g[i + 1]
                j = 1
                while j < nm1:
                    gi[j] = ((omega_over_four * (((gim1[j] + gip1[j] +
                                                   gi[j - 1]) + gi[j + 1]))) + (one_minus_omega * gi[j]))
                    j += 1
                i += 1
            p += 1


class SparseCompRow:

    @staticmethod
    def num_flops(n: int, nz: int, num_iterations: int) -> float:
        actual_nz = ((math.floor(nz / n))) * n
        return ((actual_nz)) * 2.0 * ((num_iterations))

    @staticmethod
    def matmult(y, val, row, col, x, num_iterations: int) -> None:
        m = len(row) - 1
        reps = 0
        while reps < num_iterations:
            r = 0
            while r < m:
                sum0_ = .0
                rowr = row[r]
                row_rp1 = row[r + 1]
                i = rowr
                while i < row_rp1:
                    sum0_ += (x[col[i]] * val[i])
                    i += 1
                y[r] = sum0_
                r += 1
            reps += 1


class Scimark2:

    def __init__(self, print_to_console: bool = True) -> None:
        self.__output = Writer(None)
        self.__output.UseConsole = (print_to_console)

    def bench(self, min_time: float = Constants.RESOLUTION_DEFAULT, is_large: bool = False):
        fft_size = Constants.FFT_SIZE
        sor_size = Constants.SOR_SIZE
        sparse_size_m = Constants.SPARSE_SIZE_M
        sparse_size_nz = Constants.SPARSE_SIZE_NZ
        lu_size = Constants.LU_SIZE
        current_arg = 0
        if (is_large):
            fft_size = Constants.LG_FFT_SIZE
            sor_size = Constants.LG_SOR_SIZE
            sparse_size_m = Constants.LG_SPARSE_SIZE_M
            sparse_size_nz = Constants.LG_SPARSE_SIZE_NZ
            lu_size = Constants.LG_LU_SIZE
            current_arg += 1
        res = [0] * 6
        res[1] = Kernel.measureFFT(fft_size, min_time)
        res[2] = Kernel.measureSOR(sor_size, min_time)
        res[3] = Kernel.measureMonteCarlo(min_time)
        res[4] = Kernel.measureSparseMatmult(
            sparse_size_m, sparse_size_nz, min_time)
        res[5] = Kernel.measureLU(lu_size, min_time)
        res[0] = ((((res[1] + res[2] + res[3]) + res[4] + res[5])) / (5))
        self.__output.write_line()
        self.__output.write_line("SciMark 2.0a")
        self.__output.write_line()
        self.__output.write_line("Composite Score: {0}", res[0])
        self.__output.write("FFT ({0}): ", fft_size)
        if (res[1] == .0):
            self.__output.write_line(" ERROR, INVALID NUMERICAL RESULT!")
        else:
            self.__output.write_line("{0}", res[1])
        self.__output.write_line(
            "SOR ({0}x{1}):   {2}", sor_size, sor_size, res[2])
        self.__output.write_line("Monte Carlo : {0}", res[3])
        self.__output.write_line(
            "Sparse matmult (N={0}, nz={1}): {2}", sparse_size_m, sparse_size_nz, res[4])
        self.__output.write("LU ({0}x{1}): ", lu_size, lu_size)
        if (res[5] == .0):
            self.__output.write_line(" ERROR, INVALID NUMERICAL RESULT!")
        else:
            self.__output.write_line("{0}", res[5])
        return {
            "CompositeScore": res[0],
            "FFT": res[1],
            "SOR": res[2],
            "MonteCarlo": res[3],
            "SparseMathmult": res[4],
            "LU": res[5],
            "Output": self.__output.output
        }
