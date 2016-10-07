using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class DigitReader
    {
        public static int ComputeMagicFactor()
        {
            List<short> extraction = ChampowneExtract(1, 10, 100, 1000, 10000, 100000, 1000000);

            return ComputeProduct(extraction);
        }

        public static long FindMagicDigits()
        {
            long result = 0;
            long control = (long)Math.Pow(10, 10);

            for (int i = 1; i < 1001; i++)
            {
                result += FindLastNumbers(i, control);
                result = result % control;
            }

            return result;
        }

        public static long FindPermutableCubes()
        {
            var cubes = ComputeCubesWithNDigits(11).ToList();
            var wannabeCubes = ComputeCubesWithNDigits(11).ToList();

            List<long> results = new List<long>();

            for (int i = 0; i < cubes.Count; i++)
            {
                results.AddRange(wannabeCubes.Where(l => HasSameDigits(cubes[i], l)));

                if (results.Count < 5)
                {
                    foreach (var wannaBe in results)
                        wannabeCubes.Remove(wannaBe);
                }

                if (results.Count == 5)
                    return results.Min();

                results.Clear();
            }

            return 0;
        }

        internal static object FindFactorialCycles(int maxSize, int chainLength)
        {
            int count = 0;

            var chainer = new FactorialChainer(10);

            for (int i = 0; i < maxSize; i++)
            {
                int length = chainer.ComputeChainLength(i);

                if (length == chainLength)
                    count++;
            }

            return count;
        }

        public static long FindNthDigitNthPower()
        {
            long count = 1;// to take 1^1 into account

            for (var i = 2; i < 10; i++)
            {
                var j = 1;
                var toTest = 1.0;

                while (toTest > 0)
                {
                    toTest = j * Math.Log10(i) - j + 1;
                    if (toTest > 0)
                        count++;

                    j++;
                }
            }

            return count;
        }

        private static IEnumerable<long> ComputeCubesWithNDigits(int power)
        {
            double cubicPow = (double)1 / 3;

            var start = (long)Math.Ceiling(Math.Pow(10, power * cubicPow));
            var end = (long)Math.Ceiling(Math.Pow(10, (power + 1) * cubicPow));

            for (long i = start; i < end; i++)
                yield return (long)Math.Pow(i, 3);
        }

        internal static List<short> ChampowneExtract(params int[] relevantIndices)
        {
            var fullList = new List<short>();

            for (int i = 1; i < 1000000; i++)
                fullList.AddRange(Decomposition.Decompose(i, 10));

            return relevantIndices.Select(t => fullList[t - 1]).ToList();
        }

        internal static int ComputeProduct(List<short> extraction)
        {
            return extraction.Aggregate(1, (x, y) => x * y);
        }

        internal static long FindLastNumbers(long item, long control)
        {
            long tester = 1;

            for (int i = 0; i < item; i++)
            {
                tester *= item;
                tester = tester % control;
            }

            return tester;
        }

        internal static bool HasSameDigits(long x, long y)
        {
            if ((Math.Abs((int)Math.Log10(x) - (int)Math.Log10(y)) > 1))
                return false;

            var xDigs = Decomposition.SearchingDecomposition(x, 10);
            var yDigs = Decomposition.SearchingDecomposition(y, 10);

            if (xDigs.Count != yDigs.Count)
                return false;

            foreach (var digit in xDigs)
            {
                if (!yDigs.ContainsKey(digit.Key) || yDigs[digit.Key] != xDigs[digit.Key])
                    return false;
            }

            return true;
        }        
    }
}