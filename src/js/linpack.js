
var Linpack = /** @class */ (function () {
    var output = new Writer();


    function Linpack(writer, name) {
        output = writer || Writer;
        this.name = name;
    }


    function abs(d) {
        return ((d >= 0) ? d : -d);
    }

    Linpack.prototype.bench = function (array_size) {
        this.output.WriteLine(("Running Linpack " + (String(array_size)) + "x") + (String(array_size)) + " in JavaScript")/* error */;
        var mflops_result = 0.0;
        var residn_result = 0.0;
        var time_result = 0.0;
        var eps_result = 0.0;
        var a = new Float64Array(array_size);
        for (var ai = 0; ai < a.length; ai++) {
            a[ai] = new Float64Array(array_size);
        }
        var b = new Float64Array(array_size);
        var x = new Float64Array(array_size);
        var ops = 0;
        var total = 0;
        var norma = 0;
        var normx = 0;
        var resid = 0;
        var n = 0;
        var i = 0;
        var info = 0;
        var lda = 0;
        var ipvt = new Int32Array(array_size);
        lda = array_size;
        n = array_size;

        ops = ((2.0e0 * n) * n * n) / 3.0 + 2.0 * (n * n);

        var wrapa12 = new RefOutArgWrapper(a);
        var wrapb13 = new RefOutArgWrapper(b);
        norma = matgen(wrapa12, lda, n, wrapb13);
        a = wrapa12.value;
        b = wrapb13.value;
        var sw = new Stopwatch();
        sw.start();
        var wrapa10 = new RefOutArgWrapper(a);
        var wrapipvt11 = new RefOutArgWrapper(ipvt);
        info = dgefa(wrapa10, lda, n, wrapipvt11);
        a = wrapa10.value;
        ipvt = wrapipvt11.value;
        var wrapa7 = new RefOutArgWrapper(a);
        var wrapipvt8 = new RefOutArgWrapper(ipvt);
        var wrapb9 = new RefOutArgWrapper(b);
        dgesl(wrapa7, lda, n, wrapipvt8, wrapb9, 0);
        a = wrapa7.value;
        ipvt = wrapipvt8.value;
        b = wrapb9.value;
        total = Utils.intDiv(sw.elapsedMilliseconds, 1000);
        for (i = 0; i < n; i++) {
            x[i] = b[i];
        }
        var wrapa5 = new RefOutArgWrapper(a);
        var wrapb6 = new RefOutArgWrapper(b);
        norma = matgen(wrapa5, lda, n, wrapb6);
        a = wrapa5.value;
        b = wrapb6.value;
        for (i = 0; i < n; i++) {
            b[i] = -b[i];
        }
        var wrapb2 = new RefOutArgWrapper(b);
        var wrapx3 = new RefOutArgWrapper(x);
        var wrapa4 = new RefOutArgWrapper(a);
        dmxpy(n, wrapb2, n, lda, wrapx3, wrapa4);
        b = wrapb2.value;
        x = wrapx3.value;
        a = wrapa4.value;
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
        this.output.WriteLine("Norma is " + (String(norma)))/* error */;
        this.output.WriteLine("Residual is " + (String(resid)))/* error */;
        this.output.WriteLine("Normalised residual is " + (String(residn_result)))/* error */;
        this.output.WriteLine("Machine result.Eepsilon is " + (String(eps_result)))/* error */;
        this.output.WriteLine("x[0]-1 is " + (String((x[0] - (1)))))/* error */;
        this.output.WriteLine("x[n-1]-1 is " + (String((x[n - 1] - (1)))))/* error */;
        this.output.WriteLine("Time is " + (String(time_result)))/* error */;
        this.output.WriteLine("MFLOPS: " + (String(mflops_result)))/* error */;
        var result = LinpackResult._new1(norma, resid, residn_result, eps_result, time_result, mflops_result, this.output.Output);
        return result;
    };

    function matgen(a, lda, n, b) {
        var norma = 0;
        var i = 0;
        var j = 0;
        var iseed = new Int32Array(4);
        iseed[0] = 1;
        iseed[1] = 2;
        iseed[2] = 3;
        iseed[3] = 1325;
        norma = 0.0;
        for (i = 0; i < n; i++) {
            for (j = 0; j < n; j++) {
                a.value[j][i] = lran(iseed) - 0.5;
                norma = ((a.value[j][i] > norma) ? a.value[j][i] : norma);
            }
        }
        for (i = 0; i < n; i++) {
            b.value[i] = 0.0;
        }
        for (j = 0; j < n; j++) {
            for (i = 0; i < n; i++) {
                b.value[i] += a.value[j][i];
            }
        }
        return norma;
    }

    function lran(seed) {
        var m1 = 0;
        var m2 = 0;
        var m3 = 0;
        var m4 = 0;
        var ipw2 = 0;
        var it1 = 0;
        var it2 = 0;
        var it3 = 0;
        var it4 = 0;
        var r = 0;
        var result = 0;
        m1 = 494;
        m2 = 322;
        m3 = 2508;
        m4 = 2549;
        ipw2 = 4096;
        r = 1.0 / (ipw2);
        it4 = seed[3] * m4;
        it3 = Utils.intDiv(it4, ipw2);
        it4 = it4 - (ipw2 * it3);
        it3 = it3 + (seed[2] * m4) + (seed[3] * m3);
        it2 = Utils.intDiv(it3, ipw2);
        it3 = it3 - (ipw2 * it2);
        it2 = (it2 + (seed[1] * m4) + (seed[2] * m3)) + (seed[3] * m2);
        it1 = Utils.intDiv(it2, ipw2);
        it2 = it2 - (ipw2 * it1);
        it1 = (it1 + (seed[0] * m4) + (seed[1] * m3)) + (seed[2] * m2) + (seed[3] * m1);
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
                col_k = a.value[k];
                kp1 = k + 1;
                l_ = idamax(n - k, col_k, k, 1) + k;
                ipvt.value[k] = l_;
                if (col_k[l_] !== 0) {
                    if (l_ !== k) {
                        t = col_k[l_];
                        col_k[l_] = col_k[k];
                        col_k[k] = t;
                    }
                    t = -1.0 / col_k[k];
                    var wrapcol_k14 = new RefOutArgWrapper(col_k);
                    dscal(n - ((kp1)), t, wrapcol_k14, kp1, 1);
                    col_k = wrapcol_k14.value;
                    for (j = kp1; j < n; j++) {
                        col_j = a.value[j];
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
        ipvt.value[n - 1] = n - 1;
        if (a.value[(n - 1)][(n - 1)] === 0)
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
                    l_ = ipvt.value[k];
                    t = b.value[l_];
                    if (l_ !== k) {
                        b.value[l_] = b.value[k];
                        b.value[k] = t;
                    }
                    kp1 = k + 1;
                    daxpy(n - ((kp1)), t, a.value[k], kp1, 1, b.value, kp1, 1);
                }
            }
            for (kb = 0; kb < n; kb++) {
                k = n - ((kb + 1));
                b.value[k] /= a.value[k][k];
                t = -b.value[k];
                daxpy(k, t, a.value[k], 0, 1, b.value, 0, 1);
            }
        }
        else {
            for (k = 0; k < n; k++) {
                t = ddot(k, a.value[k], 0, 1, b.value, 0, 1);
                b.value[k] = ((b.value[k] - t)) / a.value[k][k];
            }
            if (nm1 >= 1) {
                for (kb = 1; kb < nm1; kb++) {
                    k = n - ((kb + 1));
                    kp1 = k + 1;
                    b.value[k] += ddot(n - ((kp1)), a.value[k], kp1, 1, b.value, kp1, 1);
                    l_ = ipvt.value[k];
                    if (l_ !== k) {
                        t = b.value[l_];
                        b.value[l_] = b.value[k];
                        b.value[k] = t;
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
                    dx.value[i + dx_off] *= da;
                }
            }
            else
                for (i = 0; i < n; i++) {
                    dx.value[i + dx_off] *= da;
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
                y.value[i] += (x.value[j] * m.value[j][i]);
            }
        }
    }

    return Linpack;
}());