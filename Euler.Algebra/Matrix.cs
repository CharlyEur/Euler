using System;

namespace Euler.Algebra
{
    public class Matrix<T>
    {
		public int M { get; private set; }
		public int N { get; private set; }

		private T[,] _matrix;

		public Matrix( T[,] input )
		{
			_matrix = input;

			M = input.GetLength(0);
			N = input.GetLength(1);
		}

		public Matrix(int m, int n)
		{
			_matrix = new T[m, n];

			M = m;
			N = n;
		}

		public T this[int m, int n]
		{
			get { return _matrix[m, n]; }
			set { _matrix[m, n] = value; }
		}

		public static Matrix<T> Addition(Matrix<T> A, Matrix<T> B, Func<T,T,T> addition)
		{
			if (A.M != B.M || A.N != B.N)
				throw new ArgumentOutOfRangeException();

			var result = new Matrix<T>(A.M, A.N);

			for (int i = 0; i < A.M; i++)
			{
				for (int j = 0; j < A.N; j++)
					result[i, j] = addition(A[i, j], B[i, j]);
			}

			return result;
		}

		public static Matrix<T> Multiplication(Matrix<T> A, Matrix<T> B, Func<T, T, T> addition, Func<T, T, T> product)
		{
			if (A.N != B.M)
				throw new ArgumentOutOfRangeException();

			var result = new Matrix<T>(A.M, B.N);

			for (int i = 0; i < A.M; i++)
			{
				for (int j = 0; j < B.N; j++)
				{
					for (int k = 0; k < A.N; k++)
					{
						result[i, j] = addition(result[i, j], product(A[i, k], B[k, j]));
					}
				}
			}

			return result;
		}

		public static bool Equality(Matrix<T> A, Matrix<T> B, Func<T, T, bool> equality)
		{
			if (A.M != B.M || A.N != B.N)
				return false;
			
			for (int i = 0; i < A.M; i++)
			{
				for (int j = 0; j < B.N; j++)
				{
					if (!equality(A[i, j], B[i, j]))
						return false;
				}
			}

			return true;
		}
	}

	public class MatrixN : Matrix<int>
	{
		public MatrixN(int[,] input) : base(input) { }
		public MatrixN(int m, int n) : base(m,n) { }

		public static MatrixN PlainAddition(MatrixN A, MatrixN B)
		{
			return Addition(A, B, (x, y) => x + y) as MatrixN;
		}

		public static MatrixN PlainMultiplication(MatrixN A, MatrixN B)
		{
			return Multiplication(A, B, (x, y) => x + y, (x, y) => x * y) as MatrixN;
		}

		public static bool PlainEquality(MatrixN A, MatrixN B)
		{
			return Equality(A, B, (x, y) => x == y);
		}

		public static MatrixN operator +(MatrixN A, MatrixN B)
		{
			if (A.M != B.M || A.N != B.N)
				throw new ArgumentOutOfRangeException();

			var result = new MatrixN(A.M, A.N);

			for (int i = 0; i < A.M; i++)
			{
				for (int j = 0; j < A.N; j++)
					result[i, j] = A[i, j] + B[i, j];
			}

			return result;
		}

		public static MatrixN operator *(MatrixN A, MatrixN B)
		{
			if (A.N != B.M)
				throw new ArgumentOutOfRangeException();

			var result = new MatrixN(A.M, B.N);

			for (int i = 0; i < A.M; i++)
			{
				for (int j = 0; j < B.N; j++)
				{
					for (int k = 0; k < A.N; k++)
						result[i, j] += A[i, k] * B[k, j];
				}
			}

			return result;
		}

		public static bool operator !=(MatrixN A, MatrixN B)
		{
			return !(A == B);
		}

		public static bool operator ==(MatrixN A, MatrixN B)
		{
			if (A.M != B.M || A.N != B.N)
				return false;

			for (int i = 0; i < A.M; i++)
			{
				for (int j = 0; j < B.N; j++)
				{
					if (A[i, j] != B[i, j])
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Doubious hashcode...
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			double buffer = 0;

			for (int i = 0; i < M; i++)
			{
				for (int j = 0; j < N; j++)
					buffer += this[i, j] * Math.Pow(31,j) + i;
			}

			return buffer.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is MatrixN)
				return (this == obj as MatrixN );

			return false;
		}
	}

}
