using System;
using GInt = Euler.Core.GaussianInteger;

namespace Euler.Core
{
	public class GaussianInteger : IComparable, IComparable<GaussianInteger>
	{
		public readonly static GInt One = N(1, 0);
		public readonly static GInt I = N(0, 1);
		public readonly static GInt MinusI = N(0, -1);
		public readonly static GInt MinusOne = N(-1, 0);

		public long A { get; private set; }
		public long B { get; private set; }

		public long SquareModule { get { return A * A + B * B; } }

		public double Module { get { return Math.Sqrt(SquareModule); } }

		public double Theta { get { return Math.Atan((double)B / A); } }

		private GaussianInteger() { }

		public override string ToString()
		{
			return string.Format("{0}+{1}.i", A, B);
		}

		public static GInt N(long a, long b)
		{
			return new GaussianInteger { A = a, B = b };
		}

		protected bool Equals(GaussianInteger other)
		{
			return A == other.A && B == other.B;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (obj.GetType() != GetType())
				return false;

			return Equals((GaussianInteger)obj);
		}


		public override int GetHashCode()
		{
			unchecked
			{
				return (A.GetHashCode() * 397) ^ B.GetHashCode();
			}
		}

		#region Operators

		public static GInt operator !(GInt instance)
		{
			return N(instance.A, -instance.B);
		}

		public static implicit operator GaussianInteger(int xPart)
		{
			return N(xPart, 0);
		}

		public static GaussianInteger operator +(GInt left, GInt right)
		{
			return N(left.A + right.A, left.B + right.B);
		}
		
		public static GInt operator -(GInt instance)
		{
			return N(-instance.A, -instance.B);
		}
		
		public static GInt operator -(GInt left, GInt right)
		{
			return left + (-right);
		}
		
		public static GInt operator *(GInt left, GInt right)
		{
			return N(left.A * right.A - left.B * right.B, left.B * right.A + left.A * right.B);
		}
		
		public static bool MultiplyWithCheck(long real, long imaginary, GInt right, long moduleMax, out GInt result)
		{
			result = One;
			
			var a = real * right.A - imaginary * right.B;

			if (a >= moduleMax || a <= 0.0)
				return false;
			
			var b = imaginary * right.A + real * right.B;
			if (b >= moduleMax || b < 0.0)
				return false;
			
			result = N(a, b);

			return true;
		}

		public static GInt operator /(GInt left, GInt right)
		{
			var gint = GaussianDivide(left, right);

			if (gint == null)
				throw new InvalidOperationException(string.Format("Cannot divide those numbers Num[{0}] / Denom[{1}]", left, right));

			return N(gint.Item1, gint.Item2);
		}

		#endregion

		/// <summary>
		/// Returns true if THIS is in upper right square delimited by Real(a) = moduleMax and Im(a) = ModuleMax.
		/// </summary>
		/// <param name="moduleMax"></param>
		/// <returns></returns>
		public bool IsInUpperRightSquare(long moduleMax)
		{
			return A >= 0
				&& A < moduleMax
				&& B >= 0
				&& B < moduleMax;
		}

		/// <summary>
		/// Returns true if THIS divides the candidate. 4.Divide(8) would be true.
		/// </summary>
		/// <param name="candidate"></param>
		/// <returns></returns>
		public bool Divide(GaussianInteger candidate)
		{
			var gint = GaussianDivide(candidate, this);

			return (gint != null);
		}

		private static Tuple<long, long> GaussianDivide(GInt left, GInt right)
		{
			var denom = right.SquareModule;

			if (denom == 0)
				return null;

			var aPot = left.A * right.A + left.B * right.B;

			if (aPot % denom != 0)
				return null;

			var bPot = left.B * right.A - left.A * right.B;
			if (bPot % denom != 0)
				return null;

			return new Tuple<long, long>(aPot / denom, bPot / denom);
		}



		public GInt Friend
		{
			get { return N(B, A); }
		}

		public int CompareTo(object obj)
		{
			if (!(obj is GInt))
				return -1;

			return CompareTo(obj as GInt);
		}



		public int CompareTo(GInt other)
		{
			var moduleTest = SquareModule.CompareTo(other.SquareModule);
			if (moduleTest != 0)
				return moduleTest;

			var realTest = A.CompareTo(other.A);

			if (realTest != 0)
				return realTest;

			var imaginaryTest = B.CompareTo(other.B);
			if (imaginaryTest != 0)
				return imaginaryTest;

			return 0;
		}
	}
}