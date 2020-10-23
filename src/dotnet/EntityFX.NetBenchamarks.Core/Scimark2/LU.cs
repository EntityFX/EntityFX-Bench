using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Scimark2
{
    internal class LU
    {
        public static double num_flops(int N)
        {
            // rougly 2/3*N^3

            double Nd = (double)N;

            return (2.0 * Nd * Nd * Nd / 3.0);
        }

        protected static double[] new_copy(double[] x)
        {
            int N = x.Length;
            double[] T = new double[N];
            for (int i = 0; i < N; i++)
                T[i] = x[i];
            return T;
        }

        protected static double[][] new_copy(double[][] A)
        {
            int M = A.Length;
            int N = A[0].Length;

            double[][] T = new double[M][];

            for (int i = 0; i < M; i++)
            {
                double[] Ti = T[i];
                double[] Ai = A[i];
                for (int j = 0; j < N; j++)
                    Ti[j] = Ai[j];
            }

            return T;
        }



        public static int[] new_copy(int[] x)
        {
            int N = x.Length;
            int[] T = new int[N];
            for (int i = 0; i < N; i++)
                T[i] = x[i];
            return T;
        }

        protected static void insert_copy(double[][] B, double[][] A)
        {
            int M = A.Length;
            int N = A[0].Length;

            int remainder = N & 3;       // N mod 4;

            for (int i = 0; i < M; i++)
            {
                double[] Bi = B[i];
                double[] Ai = A[i];
                for (int j = 0; j < remainder; j++)
                    Bi[j] = Ai[j];
                for (int j = remainder; j < N; j += 4)
                {
                    Bi[j] = Ai[j];
                    Bi[j + 1] = Ai[j + 1];
                    Bi[j + 2] = Ai[j + 2];
                    Bi[j + 3] = Ai[j + 3];
                }
            }

        }
        public double[][] getLU()
        {
            return new_copy(LU_);
        }
        /**
            Returns a <em>copy</em> of the pivot vector.

            @return the pivot vector used in obtaining the
            LU factorzation.  Subsequent solutions must
            permute the right-hand side by this vector.

        */
        public int[] getPivot()
        {
            return new_copy(pivot_);
        }

        /**
            Initalize LU factorization from matrix.

            @param A (in) the matrix to associate with this
                    factorization.
        */
        public LU(double[][] A)
        {
            int M = A.Length;
            int N = A[0].Length;

            //if ( LU_ == null || LU_.length != M || LU_[0].length != N)
            LU_ = new double[M][];

            insert_copy(LU_, A);

            //if (pivot_.length != M)
            pivot_ = new int[M];

            factor(LU_, pivot_);
        }

        /**
            Solve a linear system, with pre-computed factorization.

            @param b (in) the right-hand side.
            @return solution vector.
        */
        public double[] solve(double[] b)
        {
            double[] x = new_copy(b);

            solve(LU_, pivot_, x);
            return x;
        }


        /**
            LU factorization (in place).

            @param A (in/out) On input, the matrix to be factored.
                On output, the compact LU factorization.

            @param pivit (out) The pivot vector records the
                reordering of the rows of A during factorization.
                
            @return 0, if OK, nozero value, othewise.
*/
        public static int factor(double[][] A, int[] pivot)
        {

            int N = A.Length;
            int M = A[0].Length;

            int minMN = Math.Min(M, N);

            for (int j = 0; j < minMN; j++)
            {
                // find pivot in column j and  test for singularity.

                int jp = j;

                double t = Math.Abs(A[j][j]);
                for (int i = j + 1; i < M; i++)
                {
                    double ab = Math.Abs(A[i][j]);
                    if (ab > t)
                    {
                        jp = i;
                        t = ab;
                    }
                }

                pivot[j] = jp;

                // jp now has the index of maximum element 
                // of column j, below the diagonal

                if (A[jp][j] == 0)
                    return 1;       // factorization failed because of zero pivot


                if (jp != j)
                {
                    // swap rows j and jp
                    double[] tA = A[j];
                    A[j] = A[jp];
                    A[jp] = tA;
                }

                if (j < M - 1)                // compute elements j+1:M of jth column
                {
                    // note A(j,j), was A(jp,p) previously which was
                    // guarranteed not to be zero (Label #1)
                    //
                    double recp = 1.0 / A[j][j];

                    for (int k = j + 1; k < M; k++)
                        A[k][j] *= recp;
                }


                if (j < minMN - 1)
                {
                    // rank-1 update to trailing submatrix:   E = E - x*y;
                    //
                    // E is the region A(j+1:M, j+1:N)
                    // x is the column vector A(j+1:M,j)
                    // y is row vector A(j,j+1:N)


                    for (int ii = j + 1; ii < M; ii++)
                    {
                        double[] Aii = A[ii];
                        double[] Aj = A[j];
                        double AiiJ = Aii[j];
                        for (int jj = j + 1; jj < N; jj++)
                            Aii[jj] -= AiiJ * Aj[jj];

                    }
                }
            }

            return 0;
        }


        /**
            Solve a linear system, using a prefactored matrix
                in LU form.


            @param LU (in) the factored matrix in LU form. 
            @param pivot (in) the pivot vector which lists
                the reordering used during the factorization
                stage.
            @param b    (in/out) On input, the right-hand side.
                        On output, the solution vector.
        */
        public static void solve(double[][] LU, int[] pvt, double[] b)
        {
            int M = LU.Length;
            int N = LU[0].Length;
            int ii = 0;

            for (int i = 0; i < M; i++)
            {
                int ip = pvt[i];
                double sum = b[ip];

                b[ip] = b[i];
                if (ii == 0)
                    for (int j = ii; j < i; j++)
                        sum -= LU[i][j] * b[j];
                else
                    if (sum == 0.0)
                    ii = i;
                b[i] = sum;
            }

            for (int i = N - 1; i >= 0; i--)
            {
                double sum = b[i];
                for (int j = i + 1; j < N; j++)
                    sum -= LU[i][j] * b[j];
                b[i] = sum / LU[i][i];
            }
        }


        private double[][] LU_;
        private int[] pivot_;
    }
}