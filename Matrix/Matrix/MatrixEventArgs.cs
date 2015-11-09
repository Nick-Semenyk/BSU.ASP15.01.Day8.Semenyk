using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
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
