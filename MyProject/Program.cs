using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<double>();
            double[,] A = new double[10, 2] {{0.2, 1},{0.4, 1},{0.6, 1},{0.8, 1},{1, 1},{1.2, 1},{1.4, 1},{1.6, 1},{1.8, 1}, {2, 1}};
            double[,] b = new double[10, 1] {{1.01}, {1.04}, {1.09}, {1.16}, {1.25}, {1.36}, {1.49}, {1.64}, {1.81}, {2}};
            double[,] c = new double[2, 1] {{1}, {1}};
            double g = 0.8;
            double e = 0.4;

            double[,] x0 = new double[2, 1] { { 0.5 }, { 0.5 } }; ; //matrix 2x1 (0,0)transp
            while (true)
            {
                double[,] v = sum(b, multiplication(multiplication(A,x0), -1));
                double[,] D = diag(v);
                double[,] hx = multiplication(invers(multiplication(multiplication(transp(A), multiplication(invers(D), invers(D))), A)), c);
                double[,] hv = multiplication(multiplication(A, -1), hx);
                
                for (int i = 0; i < v.Length; i++) //v.length = hv.lenght
                {
                    list.Add((-1) * v[i, 0] / hv[i, 0]);
                }
                double a = g * list.Min();

                double[,] x1 = sum(x0, multiplication(hx, a)); //next x
                double[,] z = sum(multiplication(transp(c), x1), multiplication(multiplication(transp(c), -1), x0));
                double stop = z[0, 0];
                if (Math.Abs(stop) < e)
                {
                    print(x1);
                    return;
                }
                x0 = x1;
                list.Clear();
            }

        }

        static double[,] transp(double[,] a)
        {
            double[,] r = new double[a.GetLength(1), a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    r[j, i] = a[i, j];
                }
            }
            return r;
        }

        static double[,] invers(double[,] a)
        {
            double temp;
            double[,] r = new double[a.GetLength(0), a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    r[i,j] = 0;

                    if (i == j)
                        r[i,j] = 1;
                }

            for (int k = 0; k < a.GetLength(0); k++)
            {
                temp = a[k,k];

                for (int j = 0; j < a.GetLength(0); j++)
                {
                    a[k,j] /= temp;
                    r[k,j] /= temp;
                }

                for (int i = k + 1; i < a.GetLength(0); i++)
                {
                    temp = a[i,k];

                    for (int j = 0; j < a.GetLength(0); j++)
                    {
                        a[i,j] -= a[k,j] * temp;
                        r[i,j] -= r[k,j] * temp;
                    }
                }
            }

            for (int k = a.GetLength(0) - 1; k > 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    temp = a[i,k];

                    for (int j = 0; j < a.GetLength(0); j++)
                    {
                        a[i,j] -= a[k,j] * temp;
                        r[i,j] -= r[k,j] * temp;
                    }
                }
            }

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(0); j++)
                    a[i,j] = r[i,j];

            return r;
        }

        static double[,] diag(double[,] a)
        {
            double[,] r = new double[a.GetLength(0), a.GetLength(0)];
            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        r[i, j] = a[i,0];
                    }
                    else
                    {
                        r[i, j] = 0;
                    }

                }
            }
            return r;
        }

        static double[,] multiplication(double[,] a, double b)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = a[i, j] * b;
                }
            }
            return a;
        }

        static double[,] multiplication(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        r[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return r;
        }

        static double[,] sum(double[,] a, double[,] b)
        {
            
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    r[i, j] += a[i, j] + b[i, j];
                }
            }
            return r;
        }

        static void print(double[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0} ", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
