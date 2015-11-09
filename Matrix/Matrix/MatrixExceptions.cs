using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{


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

    public class NotSymmetricArrayException : Exception
    {
        public NotSymmetricArrayException()
        {
        }

        public NotSymmetricArrayException(string message) : base(message)
        {
        }

        public NotSymmetricArrayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotSymmetricArrayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class MatrixElementAccessException : Exception
    {
        public MatrixElementAccessException()
        {
        }

        public MatrixElementAccessException(string message) : base(message)
        {
        }

        public MatrixElementAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MatrixElementAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
