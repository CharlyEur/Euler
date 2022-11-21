using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core.UnitTests
{
	[TestFixture]
	partial class Problems
	{
		[Test]
		public void _020_Factorial_Sum()
		{
			var toTest = LargeNumberHandler.ComputeLargeFactorial(100).Sum(x => x);

			Assert.AreEqual(648, toTest);
		}

		[Test]
		public void _021_Amicable_Numbers()
		{
			var toTest = Amicability.SumAmicablesUnder(10000);

			Assert.AreEqual(31626, toTest);
		}

		[Test]
		public void _022_File_Reader()
		{
			var toTest = NameScoreReader.FindHighestScore();

			Assert.AreEqual(871198282, toTest);
		}

		[Test]
		public void _023_Non_Abundant_Sums()
		{
			var engine = new AbundantHandler(50000);

			var toTest = engine.SumNotSums();

			Assert.AreEqual(4179871, toTest);
		}

		[Test]
		public void _024_Lexicographic_Permutations()
		{
			var toTest = Permutations.GetNth(10, 1000000);

			Assert.AreEqual(2783915460, toTest);
		}

		[Test]
		public void _025_1000_Digit_Fibonacci()
		{
			var toTest = Utilities.FindBiiiiiigFiboIndex();

			Assert.AreEqual(4782, toTest);
		}

		[Test]
		public void _026_1_on_N_Periodicity()
		{
			var toTest = PrimeGames.FindBiggestDecimalUnder(1000);

			Assert.AreEqual(983, toTest);
		}

		[Test]
		public void _027_Quadratic_Primes()
		{
			var toTest = PrimeGames.FindLongestQuadratics(1000);

			Assert.AreEqual(-59231, toTest);
		}

		[Test]
		public void _028_Spiral_Diagonal()
		{
			// hand-proven formula
			long size = 500; // as 2*500+1 = 1001

			// sum(k) i=1..n = (n(n+1)/2)
			// sum(k²) i=1..n = (n(n+1)(2n+1)/6)
			long toTest = 1 + 8 * size * (size + 1) * (2 * size + 1) / 3 + 2 * size * (size + 1) + 4 * size;

			Assert.AreEqual(669171001, toTest);
		}

		[Test]
		public void _029_Distinct_Powers()
		{
			var result = new HashSet<PrimeDecomposition>();
			var searcher = new PrimalityProvider(1000);

			for (var b = 2; b <= 100; b++)
			{
				for (var a = 2; a <= 100; a++)
				{
					result.Add(searcher.Decompose(a, b));
				}
			}

			Assert.AreEqual(9183, result.Count);
		}

		[Test]
		public void _030_Sum_Of_All_Fifth_Powers()
		{
			var result = new HashSet<PrimeDecomposition>();
			var searcher = new PrimalityProvider(1000);

			for (var b = 2; b <= 100; b++)
			{
				for (var a = 2; a <= 100; a++)
				{
					result.Add(searcher.Decompose(a, b));
				}
			}

			Assert.AreEqual(9183, result.Count);
		}
	}
}
