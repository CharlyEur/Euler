using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class Pandigitals
    {
        public static long FindLargest()
        {
            var candidates = new List<long>();

            for (int i = 98; i > 90; i--)
            {
                var concat = ConcatenatedProduct.Compute(i, 4);

                if (concat.IsPandigital())
                    candidates.Add(concat.FullResult);
            }

            for (int i = 98; i > 90; i--)
            {
                var concat = ConcatenatedProduct.Compute(i, 3);

                if (concat.IsPandigital())
                    candidates.Add(concat.FullResult);
            }

            for (int i = 987; i > 900; i--)
            {
                var concat = ConcatenatedProduct.Compute(i, 3);

                if (concat.IsPandigital())
                    candidates.Add(concat.FullResult);
            }

            for (int i = 987; i > 900; i--)
            {
                var concat = ConcatenatedProduct.Compute(i, 2);

                if (concat.IsPandigital())
                    candidates.Add(concat.FullResult);
            }

            for (int i = 9876; i > 9000; i--)
            {
                var concat = ConcatenatedProduct.Compute(i, 2);

                if (concat.IsPandigital())
                    candidates.Add(concat.FullResult);
            }

            return candidates.Max();
        }

        public static long FindLargestPandigitalPrime()
        {
            var source = new PrimalityProvider(100000);
            var candidates = new List<long>();

            short numberCount = 7; // 9 and 8 are eliminated, as sum of digits would be divisible by 3

            while (!candidates.Any())
            {
                foreach (var o in PandigitalSet(numberCount))
                {
                    if (source.SafeIsPrime(o))
                        candidates.Add(o);
                }

                if (candidates.Any())
                    return candidates.Max();

                numberCount--;
            }

            throw new ApplicationException("Could not find any pandigital prime");
        }

        public static long SumWithSubDivisibility()
        {
            long totalSum = 0;

            foreach (var item in Permutations.BuildIndexPermutations(10))
            {
                List<short> digits = item.Select(x => (short)x).ToList();

                if (SatisfyAdvancedDivisibility(digits))
                    totalSum += Decomposition.Recompose(digits, 10);
            }

            return totalSum;
        }

        public static long SameDigitFinder()
        {
            bool found = false;

            long x = 1;
            long xTime2;
            long xTime3;
            long xTime4;
            long xTime5;
            long xTime6;

            while (!found)
            {
                xTime2 = x * 2;
                xTime3 = x * 3;
                xTime4 = x * 4;
                xTime5 = x * 5;
                xTime6 = x * 6;

                if (CompareDigits(x, xTime2, xTime3, xTime4, xTime5, xTime6))
                    found = true;
                else
                    x++;
            }

            return x;
        }

        private static bool CompareDigits(long a, long b, long c, long d, long e, long f)
        {
            return DigitReader.HasSameDigits(a, b)
                && DigitReader.HasSameDigits(a, c)
                && DigitReader.HasSameDigits(a, d)
                && DigitReader.HasSameDigits(a, e)
                && DigitReader.HasSameDigits(a, f);
        }

        internal static bool SatisfyAdvancedDivisibility(List<short> digits)
        {
            //if (digits[3] % 2 != 0) // by 2
            //    return false;

            //if ((digits[2] + digits[3] + digits[4]) % 3 != 0) // by 3
            //    return false;

            //if (digits[5] != 5 && digits[5] != 0) // by 5
            //    return false;

            //var tenthFigure = Decomposition.Recompose(new List<short> { digits[4], digits[5] }, 10);

            //if (tenthFigure - 2 * digits[6] % 7 == 0) // by 7
            //    return false;

            var byTwo = Decomposition.Recompose(new List<short> { digits[1], digits[2], digits[3] }, 10);
            if (byTwo % 2 != 0)
                return false;

            var byThree = Decomposition.Recompose(new List<short> { digits[2], digits[3], digits[4] }, 10);
            if (byThree % 3 != 0)
                return false;

            var byFive = Decomposition.Recompose(new List<short> { digits[3], digits[4], digits[5] }, 10);
            if (byFive % 5 != 0)
                return false;

            var bySeven = Decomposition.Recompose(new List<short> { digits[4], digits[5], digits[6] }, 10);
            if (bySeven % 7 != 0)
                return false;

            var byEleven = Decomposition.Recompose(new List<short> { digits[5], digits[6], digits[7] }, 10);
            if (byEleven % 11 != 0)
                return false;

            var byThirteen = Decomposition.Recompose(new List<short> { digits[6], digits[7], digits[8] }, 10);
            if (byThirteen % 13 != 0)
                return false;

            var bySeventeen = Decomposition.Recompose(new List<short> { digits[7], digits[8], digits[9] }, 10);
            if (bySeventeen % 17 != 0)
                return false;

            return true;
        }

        private static IEnumerable<long> PandigitalSet(short digitUsed)
        {
            foreach (var permutation in Permutations.BuildIndexPermutations(digitUsed))
            {
                var digits = permutation.Select(x => (short)(x + 1)).ToList();
                yield return Decomposition.Recompose(digits, 10);
            }
        }
    }
}