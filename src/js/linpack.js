
var Linpack = /** @class */ (function () {
    var output = new Writer();


    function Linpack(writer, name) {
        output = writer || new Writer();
        this.name = name;
    }

    function abs(d) {
        return ((d >= 0) ? d : -d);
    }

    function second() {
        return getTime();
    }

    Linpack.prototype.bench = function (array_size) {
        output.writeLine(("Running Linpack " + (array_size) + "x") + (array_size) + " in JavaScript")
        var mflops_result = 0.0;
        var residn_result = 0.0;
        var time_result = 0.0;
        var eps_result = 0.0;
        var a = Array(array_size);
        for (var ai = 0; ai < a.length; ai++) {
            a[ai] = Array(array_size);
        }
        var b = Array(array_size);
        var x = Array(array_size);
        var ops = 0;
        var total = 0;
        var norma = 0;
        var normx = 0;
        var resid = 0;
        var time = 0;
        var n = 0;
        var i = 0;
        var info = 0;
        var lda = 0;
        var ipvt = Array(array_size);
        lda = array_size;
        n = array_size;

        ops = ((2.0e0 * n) * n * n) / 3.0 + 2.0 * (n * n);

        norma = matgen(a, lda, n, b);
        time = second();
        info = dgefa(a, lda, n, ipvt);
        dgesl(a, lda, n, ipvt, b, 0);
        total = second() - time;
        for (i = 0; i < n; i++) {
            x[i] = b[i];
        }

        norma = matgen(a, lda, n, b);

        for (i = 0; i < n; i++) {
            b[i] = -b[i];
        }

        dmxpy(n, b, n, lda, x, a);

        resid = 0.0;
        normx = 0.0;
        for (i = 0; i < n; i++) {
            resid = ((resid > abs(b[i])) ? resid : abs(b[i]));
            normx = ((normx > abs(x[i])) ? normx : abs(x[i]));
        }

        eps_result = epslon(1.0);

        residn_result = resid / ((((n) * norma * normx) * eps_result));
        residn_result += 0.005;
        residn_result = Math.floor((residn_result * (100)));
        residn_result /= (100);

        time_result = total;
        time_result += 0.005;
        time_result = Math.floor((time_result * (100)));
        time_result /= (100);

        mflops_result = ops / (1.0e6 * total);
        mflops_result += 0.0005;
        mflops_result = Math.floor
            ((mflops_result * (1000)));
        mflops_result /= (1000);
        output.writeLine("Norma is " + ((norma)));
        output.writeLine("Residual is " + ((resid)));
        output.writeLine("Normalised residual is " + ((residn_result)));
        output.writeLine("Machine result.Eepsilon is " + ((eps_result)));
        output.writeLine("x[0]-1 is " + (((x[0] - (1)))));
        output.writeLine("x[n-1]-1 is " + (((x[n - 1] - (1)))));
        output.writeLine("Time is " + ((time_result)));
        output.writeLine("MFLOPS: " + ((mflops_result)));
        var result = {
            "Norma" :norma, 
            "Residual":resid, 
            "NormalisedResidual":residn_result,
            "Epsilon": eps_result,
            "Time" : time_result, 
            "Mflops" : mflops_result, 
            "Output" : ""
        };
        return result;
    };

    function matgen(a, lda, n, b) {
        var norma = 0;
        var i = 0;
        var j = 0;
        var iseed = [];
        iseed[0] = 1;
        iseed[1] = 2;
        iseed[2] = 3;
        iseed[3] = 1325;
        norma = 0.0;
        for (i = 0; i < n; i++) {
            for (j = 0; j < n; j++) {
                a[j][i] = lran(iseed) - 0.5;
                norma = ((a[j][i] > norma) ? a[j][i] : norma);
            }
        }
        for (i = 0; i < n; i++) {
            b[i] = 0.0;
        }
        for (j = 0; j < n; j++) {
            for (i = 0; i < n; i++) {
                b[i] += a[j][i];
            }
        }
        return norma;
    }

    function lran(seed) {
        var m1 = 494;
        var m2 = 322;
        var m3 = 2508;
        var m4 = 2549;
        ipw2 = 4096;
        var it1 = 0;
        var it2 = 0;
        var it3 = 0;
        var it4 = 0;
        r = 1.0 / ipw2;

        it4 = seed[3] * m4;
        it3 = Math.floor(it4 / ipw2);
        it4 = it4 - ipw2 * it3;
        it3 = it3 + seed[2] * m4 + seed[3] * m3;
        it2 = Math.floor(it3 / ipw2);
        it3 = it3 - ipw2 * it2;
        it2 = it2 + seed[1] * m4 + seed[2] * m3 + seed[3] * m2;
        it1 = Math.floor(it2 / ipw2);
        it2 = it2 - ipw2 * it1;
        it1 = it1 + seed[0] * m4 + seed[1] * m3 + seed[2] * m2 + seed[3] * m1;
        it1 = it1 % ipw2;
    
        seed[0] = it1;
        seed[1] = it2;
        seed[2] = it3;
        seed[3] = it4;
        result = r * (((it1) + (r * (((it2) + (r * (((it3) + (r * (it4))))))))));
        return result;
    }

    function dgefa(a, lda, n, ipvt) {
        var col_k = [];
        var col_j = [];
        var t = 0;
        var j = 0;
        var k = 0;
        var kp1 = 0;
        var l_ = 0;
        var nm1 = 0;
        var info = 0;
        info = 0;
        nm1 = n - 1;
        if (nm1 >= 0) {
            for (k = 0; k < nm1; k++) {
                col_k = a[k];
                kp1 = k + 1;
                l_ = idamax(n - k, col_k, k, 1) + k;
                ipvt[k] = l_;
                if (col_k[l_] !== 0) {
                    if (l_ !== k) {
                        t = col_k[l_];
                        col_k[l_] = col_k[k];
                        col_k[k] = t;
                    }
                    t = -1.0 / col_k[k];
                    dscal(n - (kp1), t, col_k, kp1, 1);

                    for (j = kp1; j < n; j++) {
                        col_j = a[j];
                        t = col_j[l_];
                        if (l_ !== k) {
                            col_j[l_] = col_j[k];
                            col_j[k] = t;
                        }
                        daxpy(n - ((kp1)), t, col_k, kp1, 1, col_j, kp1, 1);
                    }
                }
                else
                    info = k;
            }
        }
        ipvt[n - 1] = n - 1;
        if (a[(n - 1)][(n - 1)] === 0)
            info = n - 1;
        return info;
    }

    function dgesl(a, lda, n, ipvt, b, job) {
        var t = 0;
        var k = 0;
        var kb = 0;
        var l_ = 0;
        var nm1 = 0;
        var kp1 = 0;
        nm1 = n - 1;
        if (job === 0) {
            if (nm1 >= 1) {
                for (k = 0; k < nm1; k++) {
                    l_ = ipvt[k];
                    t = b[l_];
                    if (l_ !== k) {
                        b[l_] = b[k];
                        b[k] = t;
                    }
                    kp1 = k + 1;
                    daxpy(n - ((kp1)), t, a[k], kp1, 1, b, kp1, 1);
                }
            }
            for (kb = 0; kb < n; kb++) {
                k = n - ((kb + 1));
                b[k] /= a[k][k];
                t = -b[k];
                daxpy(k, t, a[k], 0, 1, b, 0, 1);
            }
        }
        else {
            for (k = 0; k < n; k++) {
                t = ddot(k, a[k], 0, 1, b, 0, 1);
                b[k] = ((b[k] - t)) / a[k][k];
            }
            if (nm1 >= 1) {
                for (kb = 1; kb < nm1; kb++) {
                    k = n - ((kb + 1));
                    kp1 = k + 1;
                    b[k] += ddot(n - ((kp1)), a[k], kp1, 1, b, kp1, 1);
                    l_ = ipvt[k];
                    if (l_ !== k) {
                        t = b[l_];
                        b[l_] = b[k];
                        b[k] = t;
                    }
                }
            }
        }
    }

    function daxpy(n, da, dx, dx_off, incx, dy, dy_off, incy) {
        var i = 0;
        var ix = 0;
        var iy = 0;
        if (((n > 0)) && ((da !== 0))) {
            if (incx !== 1 || incy !== 1) {
                ix = 0;
                iy = 0;
                if (incx < 0)
                    ix = (((-n) + 1)) * incx;
                if (incy < 0)
                    iy = (((-n) + 1)) * incy;
                for (i = 0; i < n; i++) {
                    dy[iy + dy_off] += (da * dx[ix + dx_off]);
                    ix += incx;
                    iy += incy;
                }
                return;
            }
            else
                for (i = 0; i < n; i++) {
                    dy[i + dy_off] += (da * dx[i + dx_off]);
                }
        }
    }

    function ddot(n, dx, dx_off, incx, dy, dy_off, incy) {
        var dtemp = 0;
        var i = 0;
        var ix = 0;
        var iy = 0;
        dtemp = 0;
        if (n > 0) {
            if (incx !== 1 || incy !== 1) {
                ix = 0;
                iy = 0;
                if (incx < 0)
                    ix = (((-n) + 1)) * incx;
                if (incy < 0)
                    iy = (((-n) + 1)) * incy;
                for (i = 0; i < n; i++) {
                    dtemp += (dx[ix + dx_off] * dy[iy + dy_off]);
                    ix += incx;
                    iy += incy;
                }
            }
            else
                for (i = 0; i < n; i++) {
                    dtemp += (dx[i + dx_off] * dy[i + dy_off]);
                }
        }
        return (dtemp);
    }

    function dscal(n, da, dx, dx_off, incx) {
        var i = 0;
        var nincx = 0;
        if (n > 0) {
            if (incx !== 1) {
                nincx = n * incx;
                for (i = 0; i < nincx; i += incx) {
                    dx[i + dx_off] *= da;
                }
            }
            else
                for (i = 0; i < n; i++) {
                    dx[i + dx_off] *= da;
                }
        }
    }

    function idamax(n, dx, dx_off, incx) {
        var dmax = 0;
        var dtemp = 0;
        var i = 0;
        var ix = 0;
        var itemp = 0;
        if (n < 1)
            itemp = -1;
        else if (n === 1)
            itemp = 0;
        else if (incx !== 1) {
            dmax = abs(dx[0 + dx_off]);
            ix = 1 + incx;
            for (i = 1; i < n; i++) {
                dtemp = abs(dx[ix + dx_off]);
                if (dtemp > dmax) {
                    itemp = i;
                    dmax = dtemp;
                }
                ix += incx;
            }
        }
        else {
            itemp = 0;
            dmax = abs(dx[0 + dx_off]);
            for (i = 1; i < n; i++) {
                dtemp = abs(dx[i + dx_off]);
                if (dtemp > dmax) {
                    itemp = i;
                    dmax = dtemp;
                }
            }
        }
        return (itemp);
    }

    function epslon(x) {
        var a = 0;
        var b = 0;
        var c = 0;
        var eps = 0;

        a = 4.0e0 / 3.0e0;
        eps = 0;
        while (eps === 0) {
            b = a - 1.0;
            c = b + b + b;
            eps = abs(c - 1.0);
        }
        return (eps * abs(x));
    }

    function dmxpy(n1, y, n2, ldm, x, m) {
        var j = 0;
        var i = 0;
        for (j = 0; j < n2; j++) {
            for (i = 0; i < n1; i++) {
                y[i] += (x[j] * m[j][i]);
            }
        }
    }

    return Linpack;
}());