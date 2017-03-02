using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler.Core
{
    public class Palindroms
    {
        public static int SumOfPalindromics(int max)
        {
            var sum = 0;

            for (int i = 1; i < max; i++)
            {
                if (IsPalindromBothBases(i))
                    sum += i;
            }

            return sum;
        }

        public static IEnumerable<long> FindMax(int limit)
        {
            int lowerBound = Math.Max(limit - 100, 0);

            for (int a = limit - 1; a > lowerBound; a--)
            {
                for (int b = a; b < limit; b++)
                {
                    var productDecomposition = Decomposition.Decompose(a * b);

                    if (IsPalindrom(productDecomposition))
                        yield return Decomposition.Recompose(productDecomposition, 10);
                }
            }            
        }

        internal static long CountLychrelAbove(int limit)
        {
            long count = 0;

            for (long i = 0; i < limit; i++)
            {
                if (IsLychrel(i))
                    count++;
            }

            return count;
        }

        internal static long MaximumDigitalSum(int limit)
        {
            long max = 0;

            for (var i = 1; i < limit; i++)
            {
                for (var j = 1; j < limit; j++)
                {
                    var bigNum = BigInteger.Pow(i, j);

                    var candidate = Decomposition.Decompose(bigNum).Aggregate(0, (current, x) => x + current);

                    if (candidate > max)
                        max = candidate;
                }
            }

            return max;
        }

        private static bool IsPalindromBothBases(int candidate)
        {
            var decimalDecomposition = Decomposition.Decompose(candidate, 10);

            if (IsPalindrom(decimalDecomposition))
            {
                var binaryDecomposition = Decomposition.Decompose(candidate, 2);
                return IsPalindrom(binaryDecomposition);
            }

            return false;
        }

        private static bool IsPalindrom(List<short> decomposition)
        {
            for (int i = 0; i < decomposition.Count; i++)
            {
                if (decomposition[i] != decomposition[decomposition.Count - i - 1])
                    return false;
            }

            return true;
        }

        internal static bool IsLychrel(long candidate)
        {
            var buffer = new BigInteger(candidate);
            var opCount = 0;

            while (opCount <= 50)
            {
                var newDigits = ReverseAdd(buffer);

                if (IsPalindrom(newDigits))
                    return false;

                buffer = Decomposition.Recompose(newDigits);

                opCount++;
            }

            return true;
        }

        private static List<short> ReverseAdd(BigInteger buffer)
        {
            var reverse = Reverse(buffer);

            var intermediary = BigInteger.Add(buffer, reverse);

            return Decomposition.Decompose(intermediary);
        }

        private static BigInteger Reverse(BigInteger candidate)
        {
            var toRecompose = Decomposition.Decompose(candidate);

            toRecompose.Reverse();

            return Decomposition.Recompose(toRecompose);
        }
    }
}