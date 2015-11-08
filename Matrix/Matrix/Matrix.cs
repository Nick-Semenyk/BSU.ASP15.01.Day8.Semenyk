using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
            get { return matrix[a][b]; }
            set
            {
                matrix[a][b] = value;
                EventHandler<MatrixChangeArgs> handler = MatrixChanged;
                handler(this, new MatrixChangeArgs(a,b));
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

        public bool Equals(SquareMatrix<T> other)
        {
            for (int i = 0; i<matrix.Count(); i++)
            {
                for(int j = 0; j<matrix[i].Count(); j++)
                {
                    if (other[i,j].Equals(matrix[i][j]))
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

    public class SymetricMatrix<T> : SquareMatrix<T>
    {
        public new T this[int a, int b]
        {
            get { return base[a, b]; }
            set
            {
                //events will trigger
                base[a, b] = value;
                base[b, a] = value;
            }
        }

        public SymetricMatrix(T[][] array):base(array)
        {
            for (int i = 0; i<array.Count(); i++)
            {
                for (int j = i + 1; j<array.Count(); j++)
                {
                    if (base[i,j].Equals(base[j,i]))
                    {

                    }
                    else
                    {
                        throw new NotSymetricArrayException();
                    }
                }
            }
        }

        public SymetricMatrix(int x) : base(x){ }


    }


    public class NotSquareArrayException : Exception
    {
        public NotSquareArrayException()
        {
        }

        public NotSquareArrayException(string message) : base(message)
        {
        }

        public NotSquareArrayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotSquareArrayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class NotSymetricArrayException : Exception
    {
        public NotSymetricArrayException()
        {
        }

        public NotSymetricArrayException(string message) : base(message)
        {
        }

        public NotSymetricArrayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotSymetricArrayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class MatrixChangeArgs : EventArgs
    {
        private readonly int a;
        private readonly int b;

        public int A { get { return a; } }
        public int B { get { return b; } }

        public MatrixChangeArgs(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }

}
