using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix;

namespace MatrixConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SquareMatrix<int> a = new DiagonalMatrix<int>(3);
            SquareMatrix<int> b = new DiagonalMatrix<int>(3);
            SquareMatrix<int> c;
            Random random = new Random();
            for (int i = 0; i<3; i++)
            {
                for (int j = 0; j<3; j++)
                {
                    a[i, j] = random.Next(-10, 10);
                    b[i, j] = random.Next(-10, 10);
                }
            }
            MatrixAnalyser<int>.RegisterMatrix(a);
            c = a + b;
            a[1, 1] = 0;
        }

        public static class MatrixAnalyser<T>
        {
            public static void RegisterMatrix(SquareMatrix<T> matrix)
            {
                if (matrix == null)
                    throw new ArgumentNullException();
                matrix.MatrixChanged += MatrixOnMatrixChanged;
            }

            public static void UnregisterMatrix(SquareMatrix<T> matrix)
            {
                if (matrix == null)
                    throw new ArgumentNullException();
                matrix.MatrixChanged -= MatrixOnMatrixChanged;
            }

            private static void MatrixOnMatrixChanged(object sender, MatrixChangeArgs matrixChangeArgs)
            {
                Console.WriteLine($"Matrix {nameof(sender)}, element {matrixChangeArgs.A},{matrixChangeArgs.B} was changed");
            }
        }
    }
}
