using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler.Core
{
    public static class Utilities
    {
        public static IEnumerable<long> CreateNumberPool(List<long> criterias, long limit)
        {
            long buffer = 0;

            while (buffer < limit)
            {
                if (criterias.Select(n => buffer % n).Any(x => x == 0))
                    yield return buffer;

                buffer++;
            }
        }

        public static IEnumerable<long> CreateEvenFibonaccis(long limit)
        {
            long n = 1;
            long nPlusOne = 2;
            
            while (nPlusOne < limit)
            {
                if (nPlusOne % 2 == 0)
                    yield return nPlusOne;

                long local = nPlusOne;
                nPlusOne += n;
                n = local;
            }
		}

		internal static long FindBiiiiiigFiboIndex()
		{
			return FindExtraordinaryFibonacci(x => BigInteger.Log10(x) > 999);
		}

		public static long FindExtraordinaryFibonacci(Func<BigInteger, bool> criteria)
		{
			BigInteger n = 1;
			BigInteger nPlusOne = 1;

			int index = 1;

			while (!criteria(n))
			{
				index++;
				var local = nPlusOne;
				nPlusOne += n;
				n = local;
			}

			return index;
		}

		public static int ComputeSumSquareDiff(int limit)
        {
            var intFrom1toLimit = Enumerable.Range(1, 100);
            var sumSquare = intFrom1toLimit.Select(x => x * x).Sum();
            var squareSum = Math.Pow(intFrom1toLimit.Sum(), 2);

            return (int)(squareSum - sumSquare);
        }

        public static int ResolvePythagoreanTriplet(int sum)
        {
            var triplet = FindPythagoreanTriplet(sum);

            return triplet.Item1 * triplet.Item2 * triplet.Item3;
        }

        public static Tuple<int, int, int> FindPythagoreanTriplet(int sum)
        {
            for (int c = sum - 1; c > 0; c--)
            {
                int cSquare = c * c; 

                for (int b = 1; b < sum - c; b++)
                {
                    int bSquare = b * b;
                    int a = sum - c - b;
                    int aSquare = a * a;

                    if (cSquare - bSquare - aSquare == 0)
                        return new Tuple<int, int, int>(a, b, c);                    
                }
            }

            throw new ArgumentNullException($"Could not find proper Tuple with sum = {sum}");
        }

        internal static long ComputeDigitSum(int number, int exponent)
        {
            BigInteger local = BigInteger.Pow(number, exponent);

            var digits = Decomposition.Decompose(local);

            return digits.Sum(x => x);
        }
    }
}
