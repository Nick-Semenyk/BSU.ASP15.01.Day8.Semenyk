using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Matrix;

namespace Matrix.Tests
{
    [TestFixture]
    public class MatrixTests
    {
        private SquareMatrix<int>[] source = new SquareMatrix<int>[3];
        private int n = 3;

        [TestFixtureSetUp]
        public void Initialize()
        {
            source[0] = new SquareMatrix<int>(n);
            source[1] = new SquareMatrix<int>(n);
            source[2] = new SquareMatrix<int>(n);
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    source[0][i, j] = random.Next(-1000, 1000);
                    source[1][i, j] = random.Next(-1000, 1000);
                    source[2][i, j] = source[0][i, j] + source[1][i, j];
                }
            }
        }

        //[TearDown]
        //public void Deinitialize()
        //{

        //}

        [ExpectedException(typeof(ArgumentException))]
        [TestCase(-5)]
        [TestCase(0)]
        public void ConstructorSquareMatrixTests(int size)
        {
            SquareMatrix<int> matr = new SquareMatrix<int>(size);
            //Assert.True(matr != null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestCase(-5)]
        [TestCase(0)]
        public void ConstructorDiagonalMatrixTests(int size)
        {
            DiagonalMatrix<int> matr = new DiagonalMatrix<int>(size);
            Assert.True(matr != null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestCase(-5)]
        [TestCase(0)]
        public void ConstructorSymmetricMatrixTests(int size)
        {
            SymmetricMatrix<int> matr = new SymmetricMatrix<int>(size);
            Assert.True(matr != null);
        }

        [Test]
        public void SumTests()
        {
            Console.WriteLine("Left summand");
            Console.WriteLine(source[0].ToString());
            Console.WriteLine("Right summand");
            Console.WriteLine(source[1].ToString());
            Console.WriteLine("Sum");
            Console.WriteLine(source[2].ToString());
            SquareMatrix<int> result = source[0] + source[1];
            Assert.AreEqual(result, source[2]);
        }
    }
}
