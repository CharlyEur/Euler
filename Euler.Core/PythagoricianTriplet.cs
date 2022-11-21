using Euler.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
	public enum Matrix
	{
		R1 = 1,
		R2 = 2,
		R3 = 3,
	}

	public class PythagoricianTriplet : IComparable<PythagoricianTriplet>
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Z { get; private set; }

		public MatrixN InternalMatrix { get; private set; }

		public List<Matrix> MatrixChain { get; private set; }
		public long Multiplier { get; private set; }

		public static readonly MatrixN _Source = new MatrixN(new int[,] { { 3 }, { 4 }, { 5 } });

		public static readonly MatrixN _R1 = new MatrixN( new int[,] { { +1, -2, 2 }, { +2, -1, 2 }, { +2, -2, 3 } } );
		public static readonly MatrixN _R2 = new MatrixN( new int[,] { { +1, +2, 2 }, { +2, +1, 2 }, { +2, +2, 3 } } );
		public static readonly MatrixN _R3 = new MatrixN( new int[,] { { -1, +2, 2 }, { -2, +1, 2 }, { -2, +2, 3 } } );

		public PythagoricianTriplet(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;

			MatrixChain = new List<Matrix>();
			Multiplier = 1;

			InternalMatrix = new MatrixN(new int[,] { {x}, {y}, {z} });
		}

		public PythagoricianTriplet(MatrixN vector)
			: this(0,0,0)
		{
			if (vector.M != 3 || vector.N != 1)
				throw new ArgumentOutOfRangeException();
			
			X = vector[0, 0];
			Y = vector[1, 0];
			Z = vector[2, 0];

			InternalMatrix = vector;
		}

		public static IEnumerable<PythagoricianTriplet> PrimitiveTripletUnder(long upperBound)
		{
			var reserve = new Stack<PythagoricianTriplet>();

			reserve.Push(new PythagoricianTriplet(_Source));

			while (reserve.Count > 0)
			{
				var toMultiply = reserve.Pop();

				yield return toMultiply;

				var pytR1 = toMultiply.Multiply(Matrix.R1);
				var pytR2 = toMultiply.Multiply(Matrix.R2);
				var pytR3 = toMultiply.Multiply(Matrix.R3);

				if (pytR1.Norm <= upperBound)
					reserve.Push(pytR1);

				if (pytR2.Norm <= upperBound)
					reserve.Push(pytR2);

				if (pytR3.Norm <= upperBound)
					reserve.Push(pytR3);
			}
		}

		public PythagoricianTriplet Multiply(Matrix target)
		{
			MatrixN matrix;

			switch (target)
			{
				case Matrix.R1:
					matrix = _R1;
					break;
				case Matrix.R2:
					matrix = _R2;
					break;
				case Matrix.R3:
					matrix = _R3;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			var result = new PythagoricianTriplet(matrix * InternalMatrix);
			result.MatrixChain = MatrixChain.DeepCopy();
			result.MatrixChain.Add(target);
			
			return result;
		}

		public bool IsValid()
		{
			return (Z * Z == X * X + Y * Y);
		}

		public int Norm
		{
			get { return X + Y + Z; }
		}

		public static PythagoricianTriplet operator *(int lambda, PythagoricianTriplet pyt)
		{
			var result = new PythagoricianTriplet(lambda * pyt.X, lambda * pyt.Y, lambda * pyt.Z);

			result.Multiplier *= lambda;
			result.MatrixChain = pyt.MatrixChain.DeepCopy();

			return result;
		}

		public static bool operator !=(PythagoricianTriplet tA, PythagoricianTriplet tB)
		{
			return !(tA == tB);
		}

		/// <summary>
		/// Here, I am pretty sure if every triplet is k*primitive one, we have no use for the second line...
		/// </summary>
		/// <param name="tA"></param>
		/// <param name="tB"></param>
		/// <returns></returns>
		public static bool operator ==(PythagoricianTriplet tA, PythagoricianTriplet tB)
		{
			return (tA.X == tB.X) && (tA.Y == tB.Y) && (tA.Z == tB.Z)
				|| (tA.X == tB.Y) && (tA.Y == tB.X) && (tA.Z == tB.Z);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return (X * 391 + Y * 427 + Z * 1023);
		}

		public override string ToString()
		{
			string chain = string.Join(".", MatrixChain.Select(x => x.ToString()).ToArray());

			return $"{X}-{Y}-{Z} [{chain}] L={Multiplier}";
		}

		public int CompareTo(PythagoricianTriplet other)
		{
			return Norm.CompareTo(other.Norm);// MatrixChain.Count.CompareTo(other.MatrixChain.Count);
		}

		internal static List<PythagoricianTriplet> EnrichPrimitiveList(int upperBound)
		{
			var baseList = PrimitiveTripletUnder(upperBound).ToList();

			var additionnal = new List<PythagoricianTriplet>();

			foreach (var pyt in baseList)
			{
				var norm = pyt.Norm;
				int i = 2;

				while (norm * i <= upperBound)
				{
					additionnal.Add(i * pyt);
					i++;
				}
			}

			baseList.AddRange(additionnal);

			return baseList;
		}

		internal static List<PythagoricianTriplet> RefineTripletList(int upperBound)
		{
			var baseList = EnrichPrimitiveList(upperBound).ToList();
			
			var tripletWatcher = new SortedDictionary<int, List<PythagoricianTriplet>>();

			foreach (var triplet in baseList)
			{
				var size = triplet.Norm;

				if (!tripletWatcher.ContainsKey(size))
					tripletWatcher.Add(size, new List<PythagoricianTriplet>());

				tripletWatcher[size].Add(triplet);
			}

			return tripletWatcher
					.Where(x => x.Value.Count == 1)
					.Select(x => x.Value[0])
					.ToList();
		}
	}
}
