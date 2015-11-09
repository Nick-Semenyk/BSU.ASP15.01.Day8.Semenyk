using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class SquareMatrix<T> : IEquatable<SquareMatrix<T>>
    {
        private T[][] matrix;

        public event EventHandler<MatrixChangeArgs> MatrixChanged = delegate { };

        public int Size { get; }

        public T this[int a, int b]
        {
            get
            {
                if (a < Size && b < Size && a >=0 && b >= 0)
                    return matrix[a][b];
                throw new MatrixElementAccessException("There is no element in this matrix with such index");
            }
            set
            {
                if (a < Size && b < Size && a >= 0 && b >= 0)
                {
                    matrix[a][b] = value;
                    EventHandler<MatrixChangeArgs> handler = MatrixChanged;
                    handler(this, new MatrixChangeArgs(a,b));
                }
                else
                    throw new MatrixElementAccessException("There is no element in this matrix with such index");
            }
        }

        public SquareMatrix(T[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            matrix = new T[array.Count()][];
            for (int i = 0; i<array.Count(); i++)
            {
                if (array[i] == null)
                    throw new ArgumentNullException();
                if (array[i].Count() != array.Count())
                    throw new NotSquareArrayException();
                matrix[i] = new T[array.Count()];
                for (int j = 0; j<array[i].Count(); j++)
                {
                    matrix[i][j] = array[i][j];
                }
            }
            Size = matrix.Count();
        } 

        public SquareMatrix(int x)
        {
            if (x <= 0)
                throw new ArgumentException("Matrix must have positive size");
            Size = x;
            matrix = new T[x][];
            for (int i = 0; i<x; i++)
            {
                matrix[i] = new T[x];
                for (int j = 0; j < x; j++)
                {
                    matrix[i][j] = default(T);
                }
            }
        } 

        public static SquareMatrix<T> operator +(SquareMatrix<T> lhs, SquareMatrix<T> rhs)
        {
            //can use +
            dynamic a = default(T);
            dynamic b = default(T);
            try
            {
                a = a + b;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
            if (lhs == null || rhs == null)
                throw new ArgumentNullException();
            if (lhs.Size != rhs.Size)
                throw new InvalidOperationException();
            SquareMatrix<T> result = new SquareMatrix<T>(lhs.Size);
            for (int i = 0; i<lhs.Size; i++)
            {
                for (int j = 0; j<lhs.Size; j++)
                {
                    dynamic leftSummand = lhs[i, j];
                    dynamic rightSummand = rhs[i, j];
                    dynamic sum = leftSummand + rightSummand;
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public virtual void Transpose()
        {
            for (int i = 0; i<Size; i++)
                for (int j = i+1; j<Size; j++)
                {
                    T swapValue = this[i,j];
                    this[i,j] = this[j,i];
                    this[j,i] = swapValue;
                }
        }

        public bool Equals(SquareMatrix<T> other)
        {
            for (int i = 0; i<matrix.Count(); i++)
            {
                for(int j = 0; j<matrix[i].Count(); j++)
                {
                    if (other[i,j] == null && matrix[i][j] == null)
                    {
                        continue;
                    }
                    if (other[i,j]?.Equals(matrix[i][j]) == true)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public class SymmetricMatrix<T> : SquareMatrix<T>
    {
        public new T this[int a, int b]
        {
            get { return base[a, b]; }
            set
            {
                //events will trigger
                base[a, b] = value;
                if (b != a)
                    base[b, a] = value;
            }
        }

        public SymmetricMatrix(T[][] array):base(array)
        {
            for (int i = 0; i<array.Count(); i++)
            {
                for (int j = i + 1; j<array.Count(); j++)
                {
                    if (base[i, j] == null && base[j, i] == null)
                    {
                        continue;
                    }
                    if (base[i, j]?.Equals(base[j, i]) == true)
                    {

                    }
                    else
                    {
                        throw new NotSymmetricArrayException();
                    }
                }
            }
        }

        public SymmetricMatrix(int x) : base(x){ }

        public override void Transpose()
        {

        }

        public static SymmetricMatrix<T> operator +(SymmetricMatrix<T> lhs, SymmetricMatrix<T> rhs)
        {
            //can use +
            dynamic a = default(T);
            dynamic b = default(T);
            try
            {
                a = a + b;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
            if (lhs == null || rhs == null)
                throw new ArgumentNullException();
            if (lhs.Size != rhs.Size)
                throw new InvalidOperationException();
            SymmetricMatrix<T> result = new SymmetricMatrix<T>(lhs.Size);
            for (int i = 0; i < lhs.Size; i++)
            {
                for (int j = i; j < lhs.Size; j++)
                {
                    dynamic leftSummand = lhs[i, j];
                    dynamic rightSummand = rhs[i, j];
                    dynamic sum = leftSummand + rightSummand;
                    result[i, j] = sum;
                }
            }
            return result;
        }
    }

    public class DiagonalMatrix<T> : SymmetricMatrix<T>
    {
        public new T this[int a, int b]
        {
            get { return base[a, b]; }
            set
            {
                if (a != b)
                    throw new MatrixElementAccessException("Setting something other than default value in non-diagonal element will make this matrix non-diagonal");
                base[a, a] = value;
            }
        }

        public DiagonalMatrix(T[][] array) : base(array)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                for (int j = i + 1; j < array.Count(); j++)
                {
                    if (base[i, j] == null && base[j, i] == null)
                    {
                        continue;
                    }
                    if (base[i, j]?.Equals(base[j, i]) == true)
                    {

                    }
                    else
                    {
                        throw new NotSymmetricArrayException();
                    }
                }
            }
        }

        public DiagonalMatrix(int x) : base(x) { }

        public override void Transpose()
        {

        }

        public static DiagonalMatrix<T> operator +(DiagonalMatrix<T> lhs, DiagonalMatrix<T> rhs)
        {
            //can use +
            dynamic a = default(T);
            dynamic b = default(T);
            try
            {
                a = a + b;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
            if (lhs == null || rhs == null)
                throw new ArgumentNullException();
            if (lhs.Size != rhs.Size)
                throw new InvalidOperationException();
            DiagonalMatrix<T> result = new DiagonalMatrix<T>(lhs.Size);
            for (int i = 0; i < lhs.Size; i++)
            {
                dynamic leftSummand = lhs[i, i];
                dynamic rightSummand = rhs[i, i];
                dynamic sum = leftSummand + rightSummand;
                result[i, i] = sum;
            }
            return result;
        }
    }

}
