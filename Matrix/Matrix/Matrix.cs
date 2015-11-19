using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using static System.Math;

namespace Matrix
{
    public abstract class Matrix<T> : IEnumerable<T>
    {
        public int Size { get; protected set; }

        public T this[int a, int b]
        {
            get { return GetValue(a, b); }
            set { SetValue(a, b, value); }
        }

        public abstract T GetValue(int a, int b);

        public abstract void SetValue(int a, int b, T value);

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i<Size; i++)
                for (int j = 0; j<Size; j++)
                    yield return GetValue(i, j);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class SquareMatrix<T> : Matrix<T>, IEquatable<SquareMatrix<T>>
    {
        private T[][] matrix;

        public event EventHandler<MatrixChangeArgs> MatrixChanged = delegate { };

        public override T GetValue(int a, int b)
        {
            if (a < Size && b < Size && a >= 0 && b >= 0)
                return matrix[a][b];
            throw new MatrixElementAccessException("There is no element in this matrix with such index");
        }

        public override void SetValue(int a, int b, T value)
        {
            if (a < Size && b < Size && a >= 0 && b >= 0)
            {
                matrix[a][b] = value;
                EventHandler<MatrixChangeArgs> handler = MatrixChanged;
                handler(this, new MatrixChangeArgs(a, b));
            }
            else
                throw new MatrixElementAccessException("There is no element in this matrix with such index");
        }

        public SquareMatrix(T[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (!array.Any())
                throw new ArgumentException();
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
            T b = default(T);
            try
            {
                a = a + b;
            }
            catch (RuntimeBinderException)
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

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result.Append(matrix[i][j].ToString());
                    result.Append(' ');
                }
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }
    }

    public class SymmetricMatrix<T> : Matrix<T>
    {
        private T[] elements;

        public SymmetricMatrix(T[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (!array.Any())
                throw new ArgumentException();
            elements = new T[array.Count() * (array.Count() - 1) / 2];
            Size = array.Count();
            for (int i = 0; i<array.Count(); i++)
            {
                if (array[i] == null)
                    throw new ArgumentNullException();
                if (array[i].Count() != array.Count())
                    throw new NotSquareArrayException();
                for (int j = i; j<array.Count(); j++)
                {
                    if (array[j] == null)
                        throw new ArgumentNullException();
                    if (array[i][j]?.Equals(array[j][i]) ?? (array[i][j] == null && array[j][i] == null))
                    {
                        SetValue(i, j, array[i][j]);
                    }
                    else
                    {
                        throw new NotSymmetricArrayException();
                    }
                }
            }
        }

        public SymmetricMatrix(int x)
        {
            if (x <= 1)
                throw new ArgumentException();
            elements = new T[x * (x + 1) / 2];
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

        public override T GetValue(int a, int b)
        {
            if (a < Size && b < Size && a >= 0 && b >= 0)
                return elements[Max(a,b) * (Max(a,b) + 1) + Min(a,b)];
            throw new MatrixElementAccessException("There is no element in this matrix with such index");
        }

        public override void SetValue(int a, int b, T value)
        {
            if (a < Size && b < Size && a >= 0 && b >= 0)
                elements[Max(a, b)*(Max(a, b) + 1) + Min(a, b)] = value;
            throw new MatrixElementAccessException("There is no element in this matrix with such index");
        }
    }

    public class DiagonalMatrix<T> : Matrix<T>
    {
        private T[] elements;
        public DiagonalMatrix(T[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (!array.Any())
                throw new ArgumentException();
            elements = new T[array.Count()];
            Size = array.Count();
            for (int i = 0; i < array.Count(); i++)
            {
                if (array[i] == null)
                    throw new ArgumentNullException();
                if (array[i].Count() != array.Count())
                    throw new NotSquareArrayException();
                for (int j = 0; j < array.Count(); j++)
                {
                    if (i == j)
                    {
                        SetValue(i, i, array[i][j]);
                        continue;
                    }
                    if (!default(T).Equals(array[i][j]))
                        throw new NotDiagonalMatrixExeption();
                }
            }
        }

        public DiagonalMatrix(int size)
        {
            if (size <= 1)
                throw new ArgumentException();
            elements = new T[size];
            Size = size;
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

        public override T GetValue(int a, int b)
        {
            if (a < Size && b < Size && a >= 0 && b >= 0)
                if (a == b)
                    return elements[a];
                else
                    return default(T);
            throw new MatrixElementAccessException("There is no element in this matrix with such index");
        }

        public override void SetValue(int a, int b, T value)
        {
            if (a < Size && b < Size && a >= 0 && b >= 0)
                if (a == b)
                    elements[a] = value;
            throw new MatrixElementAccessException("There is no element in this matrix with such index");
        }
    }

}
