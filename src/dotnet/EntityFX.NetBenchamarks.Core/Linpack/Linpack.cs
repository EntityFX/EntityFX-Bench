﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EntityFX.NetBenchmark.Core.Linpack
{
    class Linpack
    {

        private IWriter output;

        public Linpack(bool printToConsole)
        {
            output = WriterFactory.Build();
            this.output.UseConsole = printToConsole;
        }

        public static void main(String[] args)
        {
            int array_size = 2000;
            if (args.Length > 0)
            {
                try
                {
                    array_size = Int32.Parse(args[0]);
                }
                catch (Exception e)
                {
                }
            }
            Linpack l = new Linpack(true);
            l.Bench(array_size);
        }

        double abs(double d)
        {
            return (d >= 0) ? d : -d;
        }

        double second_orig = -1;

        /*double second()
        {
            if (second_orig == -1)
            {
                second_orig = System.currentTimeMillis();
            }
            return (System.currentTimeMillis() - second_orig) / 1000;
        }*/

        public LinpackResult Bench(int array_size)
        {
            output.WriteLine("Running Linpack " + array_size + "x" + array_size + " in C#");
            double mflops_result = 0.0;
            double residn_result = 0.0;
            double time_result = 0.0;
            double eps_result = 0.0;

            double[][] a = new double[array_size][];
            for (int ai = 0; ai < a.Length; ai++)
            {
                a[ai] = new double[array_size];
            }

            double[] b = new double[array_size];
            double[] x = new double[array_size];
            double ops, total, norma, normx;
            double resid;//, time;
            int n, i, info, lda;
            int[] ipvt = new int[array_size];

            lda = array_size;
            n = array_size;

            ops = ((2.0e0 * n) * n * n) / 3.0 + 2.0 * (n * n);

            norma = matgen(ref a, lda, n, ref b);
            var sw = new Stopwatch();
            sw.Start();
            //time = second();
            info = dgefa(ref a, lda, n, ref ipvt);
            dgesl(ref a, lda, n, ref ipvt, ref b, 0);
            total = sw.ElapsedMilliseconds / 1000.0;

            for (i = 0; i < n; i++)
            {
                x[i] = b[i];
            }
            norma = matgen(ref a, lda, n, ref b);
            for (i = 0; i < n; i++)
            {
                b[i] = -b[i];
            }
            dmxpy(n, ref b, n, lda, ref x, ref a);
            resid = 0.0;
            normx = 0.0;
            for (i = 0; i < n; i++)
            {
                resid = (resid > abs(b[i])) ? resid : abs(b[i]);
                normx = (normx > abs(x[i])) ? normx : abs(x[i]);
            }

            eps_result = epslon((double)1.0);

            residn_result = resid / (n * norma * normx * eps_result);
            residn_result += 0.005; // for rounding
            residn_result = (int)(residn_result * 100);
            residn_result /= 100;

            time_result = total;
            time_result += 0.005; // for rounding
            time_result = (int)(time_result * 100);
            time_result /= 100;

            mflops_result = ops / (1.0e6 * total);
            mflops_result += 0.0005; // for rounding
            mflops_result = (int)(mflops_result * 1000);
            mflops_result /= 1000;

            output.WriteLine("Norma is " + norma);
            output.WriteLine("Residual is " + resid);
            output.WriteLine("Normalised residual is " + residn_result);
            output.WriteLine("Machine result.Eepsilon is " + eps_result);
            output.WriteLine("x[0]-1 is " + (x[0] - 1));
            output.WriteLine("x[n-1]-1 is " + (x[n - 1] - 1));
            output.WriteLine("Time is " + time_result);
            output.WriteLine("MFLOPS: " + mflops_result);

            LinpackResult result = new LinpackResult
            {
                Norma = norma,
                Residual = resid,
                NormalisedResidual = residn_result,
                Epsilon = eps_result,
                Time = time_result,
                MFLOPS = mflops_result,
                Output = output.Output
            };

            return result;
        }

        double matgen(ref double[][] a, int lda, int n, ref double[] b)
        {
            double norma;
            int i, j;
            int[] iseed = new int[4];

            /* Magic numbers from original Linpack source */
            iseed[0] = 1;
            iseed[1] = 2;
            iseed[2] = 3;
            iseed[3] = 1325;

            norma = 0.0;
            /*
             * Next two for() statements switched. Solver wants matrix in column order.
             * --dmd 3/3/97
             */
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    a[j][i] = lran(iseed) - 0.5;
                    norma = (a[j][i] > norma) ? a[j][i] : norma;
                }
            }
            for (i = 0; i < n; i++)
            {
                b[i] = 0.0;
            }
            for (j = 0; j < n; j++)
            {
                for (i = 0; i < n; i++)
                {
                    b[i] += a[j][i];
                }
            }

            return norma;
        }

        /*
         * Taken from original Linpack source. Claims to be a multiplicative
         * congruential random number generator with modulus 2^48, storing 48 bit
         * integers as four integer array elements, 12 bits per element.
         */
        double lran(int[] seed)
        {
            int m1, m2, m3, m4, ipw2;
            int it1, it2, it3, it4;
            double r, result;

            m1 = 494;
            m2 = 322;
            m3 = 2508;
            m4 = 2549;
            ipw2 = 4096;

            r = 1.0 / ipw2;

            it4 = seed[3] * m4;
            it3 = it4 / ipw2;
            it4 = it4 - ipw2 * it3;
            it3 = it3 + seed[2] * m4 + seed[3] * m3;
            it2 = it3 / ipw2;
            it3 = it3 - ipw2 * it2;
            it2 = it2 + seed[1] * m4 + seed[2] * m3 + seed[3] * m2;
            it1 = it2 / ipw2;
            it2 = it2 - ipw2 * it1;
            it1 = it1 + seed[0] * m4 + seed[1] * m3 + seed[2] * m2 + seed[3] * m1;
            it1 = it1 % ipw2;

            seed[0] = it1;
            seed[1] = it2;
            seed[2] = it3;
            seed[3] = it4;

            result = r * ((double)it1 + r * ((double)it2 + r * ((double)it3 + r * (double)it4)));

            return result;
        }

        /*
         * dgefa factors a double precision matrix by gaussian elimination.
         * 
         * dgefa is usually called by dgeco, but it can be called directly with a saving
         * in time if rcond is not needed. (time for dgeco) = (1 + 9/n)*(time for dgefa)
         * .
         * 
         * on entry
         * 
         * a double precision[n][lda] the matrix to be factored.
         * 
         * lda integer the leading dimension of the array a .
         * 
         * n integer the order of the matrix a .
         * 
         * on return
         * 
         * a an upper triangular matrix and the multipliers which were used to obtain
         * it. the factorization can be written a = l*u where l is a product of
         * permutation and unit lower triangular matrices and u is upper triangular.
         * 
         * ipvt integer[n] an integer vector of pivot indices.
         * 
         * info integer = 0 normal value. = k if u[k][k] .eq. 0.0 . this is not an error
         * condition for this subroutine, but it does indicate that dgesl or dgedi will
         * divide by zero if called. use rcond in dgeco for a reliable indication of
         * singularity.
         * 
         * linpack. this version dated 08/14/78. cleve moler, university of new mexico,
         * argonne national lab.
         * 
         * functions
         * 
         * blas daxpy,dscal,idamax
         */
        int dgefa(ref double[][] a, int lda, int n, ref int[] ipvt)
        {
            double[] col_k, col_j;
            double t;
            int j, k, kp1, l, nm1;
            int info;

            // gaussian elimination with partial pivoting

            info = 0;
            nm1 = n - 1;
            if (nm1 >= 0)
            {
                for (k = 0; k < nm1; k++)
                {
                    col_k = a[k];
                    kp1 = k + 1;

                    // find l = pivot index

                    l = idamax(n - k, col_k, k, 1) + k;
                    ipvt[k] = l;

                    // zero pivot implies this column already triangularized

                    if (col_k[l] != 0)
                    {

                        // interchange if necessary

                        if (l != k)
                        {
                            t = col_k[l];
                            col_k[l] = col_k[k];
                            col_k[k] = t;
                        }

                        // compute multipliers

                        t = -1.0 / col_k[k];
                        dscal(n - (kp1), t, ref col_k, kp1, 1);

                        // row elimination with column indexing

                        for (j = kp1; j < n; j++)
                        {
                            col_j = a[j];
                            t = col_j[l];
                            if (l != k)
                            {
                                col_j[l] = col_j[k];
                                col_j[k] = t;
                            }
                            daxpy(n - (kp1), t, col_k, kp1, 1, col_j, kp1, 1);
                        }
                    }
                    else
                    {
                        info = k;
                    }
                }
            }
            ipvt[n - 1] = n - 1;
            if (a[(n - 1)][(n - 1)] == 0)
                info = n - 1;

            return info;
        }

        /*
         * dgesl solves the double precision system a * x = b or trans(a) * x = b using
         * the factors computed by dgeco or dgefa.
         * 
         * on entry
         * 
         * a double precision[n][lda] the output from dgeco or dgefa.
         * 
         * lda integer the leading dimension of the array a .
         * 
         * n integer the order of the matrix a .
         * 
         * ipvt integer[n] the pivot vector from dgeco or dgefa.
         * 
         * b double precision[n] the right hand side vector.
         * 
         * job integer = 0 to solve a*x = b , = nonzero to solve trans(a)*x = b where
         * trans(a) is the transpose.
         * 
         * on return
         * 
         * b the solution vector x .
         * 
         * error condition
         * 
         * a division by zero will occur if the input factor contains a zero on the
         * diagonal. technically this indicates singularity but it is often caused by
         * improper arguments or improper setting of lda . it will not occur if the
         * subroutines are called correctly and if dgeco has set rcond .gt. 0.0 or dgefa
         * has set info .eq. 0 .
         * 
         * to compute inverse(a) * c where c is a matrix with p columns
         * dgeco(a,lda,n,ipvt,rcond,z) if (!rcond is too small){ for (j=0,j<p,j++)
         * dgesl(a,lda,n,ipvt,c[j][0],0); }
         * 
         * linpack. this version dated 08/14/78 . cleve moler, university of new mexico,
         * argonne national lab.
         * 
         * functions
         * 
         * blas daxpy,ddot
         */
        void dgesl(ref double[][] a, int lda, int n, ref int[] ipvt, ref double[] b, int job)
        {
            double t;
            int k, kb, l, nm1, kp1;

            nm1 = n - 1;
            if (job == 0)
            {

                // job = 0 , solve a * x = b. first solve l*y = b

                if (nm1 >= 1)
                {
                    for (k = 0; k < nm1; k++)
                    {
                        l = ipvt[k];
                        t = b[l];
                        if (l != k)
                        {
                            b[l] = b[k];
                            b[k] = t;
                        }
                        kp1 = k + 1;
                        daxpy(n - (kp1), t, a[k], kp1, 1, b, kp1, 1);
                    }
                }

                // now solve u*x = y

                for (kb = 0; kb < n; kb++)
                {
                    k = n - (kb + 1);
                    b[k] /= a[k][k];
                    t = -b[k];
                    daxpy(k, t, a[k], 0, 1, b, 0, 1);
                }
            }
            else
            {

                // job = nonzero, solve trans(a) * x = b. first solve trans(u)*y = b

                for (k = 0; k < n; k++)
                {
                    t = ddot(k, a[k], 0, 1, b, 0, 1);
                    b[k] = (b[k] - t) / a[k][k];
                }

                // now solve trans(l)*x = y

                if (nm1 >= 1)
                {
                    for (kb = 1; kb < nm1; kb++)
                    {
                        k = n - (kb + 1);
                        kp1 = k + 1;
                        b[k] += ddot(n - (kp1), a[k], kp1, 1, b, kp1, 1);
                        l = ipvt[k];
                        if (l != k)
                        {
                            t = b[l];
                            b[l] = b[k];
                            b[k] = t;
                        }
                    }
                }
            }
        }

        /*
         * constant times a vector plus a vector. jack dongarra, linpack, 3/11/78.
         */
        void daxpy(int n, double da, double[] dx, int dx_off, int incx, double[] dy, int dy_off, int incy)
        {
            int i, ix, iy;

            if ((n > 0) && (da != 0))
            {
                if (incx != 1 || incy != 1)
                {

                    // code for unequal increments or equal increments not equal to 1

                    ix = 0;
                    iy = 0;
                    if (incx < 0)
                        ix = (-n + 1) * incx;
                    if (incy < 0)
                        iy = (-n + 1) * incy;
                    for (i = 0; i < n; i++)
                    {
                        dy[iy + dy_off] += da * dx[ix + dx_off];
                        ix += incx;
                        iy += incy;
                    }
                    return;
                }
                else
                {

                    // code for both increments equal to 1

                    for (i = 0; i < n; i++)
                        dy[i + dy_off] += da * dx[i + dx_off];
                }
            }
        }

        /*
         * forms the dot product of two vectors. jack dongarra, linpack, 3/11/78.
         */
        double ddot(int n, double[] dx, int dx_off, int incx, double[] dy, int dy_off, int incy)
        {
            double dtemp;
            int i, ix, iy;

            dtemp = 0;

            if (n > 0)
            {

                if (incx != 1 || incy != 1)
                {

                    // code for unequal increments or equal increments not equal to 1

                    ix = 0;
                    iy = 0;
                    if (incx < 0)
                        ix = (-n + 1) * incx;
                    if (incy < 0)
                        iy = (-n + 1) * incy;
                    for (i = 0; i < n; i++)
                    {
                        dtemp += dx[ix + dx_off] * dy[iy + dy_off];
                        ix += incx;
                        iy += incy;
                    }
                }
                else
                {

                    // code for both increments equal to 1

                    for (i = 0; i < n; i++)
                        dtemp += dx[i + dx_off] * dy[i + dy_off];
                }
            }
            return (dtemp);
        }

        /*
         * scales a vector by a constant. jack dongarra, linpack, 3/11/78.
         */
        void dscal(int n, double da, ref double[] dx, int dx_off, int incx)
        {
            int i, nincx;

            if (n > 0)
            {
                if (incx != 1)
                {

                    // code for increment not equal to 1

                    nincx = n * incx;
                    for (i = 0; i < nincx; i += incx)
                        dx[i + dx_off] *= da;
                }
                else
                {

                    // code for increment equal to 1

                    for (i = 0; i < n; i++)
                        dx[i + dx_off] *= da;
                }
            }
        }

        /*
         * finds the index of element having max. absolute value. jack dongarra,
         * linpack, 3/11/78.
         */
        int idamax(int n, double[] dx, int dx_off, int incx)
        {
            double dmax, dtemp;
            int i, ix, itemp = 0;

            if (n < 1)
            {
                itemp = -1;
            }
            else if (n == 1)
            {
                itemp = 0;
            }
            else if (incx != 1)
            {

                // code for increment not equal to 1

                dmax = abs(dx[0 + dx_off]);
                ix = 1 + incx;
                for (i = 1; i < n; i++)
                {
                    dtemp = abs(dx[ix + dx_off]);
                    if (dtemp > dmax)
                    {
                        itemp = i;
                        dmax = dtemp;
                    }
                    ix += incx;
                }
            }
            else
            {

                // code for increment equal to 1

                itemp = 0;
                dmax = abs(dx[0 + dx_off]);
                for (i = 1; i < n; i++)
                {
                    dtemp = abs(dx[i + dx_off]);
                    if (dtemp > dmax)
                    {
                        itemp = i;
                        dmax = dtemp;
                    }
                }
            }
            return (itemp);
        }

        /*
         * estimate unit roundoff in quantities of size x.
         * 
         * this program should function properly on all systems satisfying the following
         * two assumptions, 1. the base used in representing dfloating point numbers is
         * not a power of three. 2. the quantity a in statement 10 is represented to the
         * accuracy used in dfloating point variables that are stored in memory. the
         * statement number 10 and the go to 10 are intended to force optimizing
         * compilers to generate code satisfying assumption 2. under these assumptions,
         * it should be true that, a is not exactly equal to four-thirds, b has a zero
         * for its last bit or digit, c is not exactly equal to one, eps measures the
         * separation of 1.0 from the next larger dfloating point number. the developers
         * of eispack would appreciate being informed about any systems where these
         * assumptions do not hold.
         *****************************************************************
         * 
         * this routine is one of the auxiliary routines used by eispack iii to avoid
         * machine dependencies.
         *****************************************************************
         * 
         * this version dated 4/6/83.
         */
        double epslon(double x)
        {
            double a, b, c, eps;

            a = 4.0e0 / 3.0e0;
            eps = 0;
            while (eps == 0)
            {
                b = a - 1.0;
                c = b + b + b;
                eps = abs(c - 1.0);
            }
            return (eps * abs(x));
        }

        /*
         * purpose: multiply matrix m times vector x and add the result to vector y.
         * 
         * parameters:
         * 
         * n1 integer, number of elements in vector y, and number of rows in matrix m
         * 
         * y double [n1], vector of length n1 to which is added the product m*x
         * 
         * n2 integer, number of elements in vector x, and number of columns in matrix m
         * 
         * ldm integer, leading dimension of array m
         * 
         * x double [n2], vector of length n2
         * 
         * m double [ldm][n2], matrix of n1 rows and n2 columns
         */
        void dmxpy(int n1, ref double[] y, int n2, int ldm, ref double[] x, ref double[][] m)
        {
            int j, i;

            // cleanup odd vector
            for (j = 0; j < n2; j++)
            {
                for (i = 0; i < n1; i++)
                {
                    y[i] += x[j] * m[j][i];
                }
            }
        }
    }
}
