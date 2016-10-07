using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class NGonRing
    {
        public int Size { get; private set; }
        public List<short> Digits { get; private set; }

        public NGonRing(int size, List<short> digits)
        {
            Size = size;

            var candidateList = Reorganize(digits);

            Digits = candidateList;
        }

        public static bool TryBuild(long digits, out NGonRing result)
        {
            result = null;
            var itemCount = Decomposition.Decompose(digits, 10).Count;

            if (itemCount % 2 > 0)
                return false;

            var size = itemCount / 2;

            var candidateItems = Decomposition.Decompose(digits, 10, itemCount)
                                               .Select(x => (short)(x + 1)) // no zero, but item 10
                                               .ToList();

            if (candidateItems.Distinct().Count() < candidateItems.Count)
                return false;

            var max = candidateItems.Max();

            if (max > candidateItems.Count)
                return false;

            if (max > 9)
            {
                if (candidateItems.IndexOf(max) % 2 == 1)
                    return false;
            }

            result = new NGonRing(size, candidateItems);
            return true;
        }

        internal static bool AdvancedTryBuild(List<int> innerDigits, int sum, out NGonRing ring)
        {
            ring = null;
            var size = innerDigits.Count;
            var fullSize = size * 2;

            var checker = new bool[fullSize];
            foreach (var idx in innerDigits)
                checker[idx] = true;

            List<short> candidate = innerDigits.Select(x => (short)x).ToList();

            for (var i = 0; i < size; i++)
            {
                var next = (i + 1) % size;
                var toTest = sum - innerDigits[i] - innerDigits[next] - 3; //-3 comes from 0 - (n-1) convention (sum)-(i+1)-(j+1) as an index : -1

                if (toTest >= 0 && (toTest < fullSize) && !checker[toTest])
                {
                    checker[toTest] = true;
                    candidate.Add((short)toTest);
                }
                else
                    return false;
            }

            ring = new NGonRing(size, Alternate(candidate.Select(x => (short)(x + 1)).ToList(), 0));
            return true;
        }

        internal static List<short> Alternate(IReadOnlyList<short> candidate, int shift)
        {
            var result = new List<short>();

            var fullSize = candidate.Count;
            var size = candidate.Count / 2;

            for (var i = 0; i < fullSize; i++)
            {
                if (i % 2 == 0)
                    result.Add(candidate[size + ((i / 2 + shift) % size)]);
                else
                    result.Add(candidate[(i - 1) / 2]);
            }

            return result;
        }

        private static bool[] Copy(IReadOnlyList<bool> checker)
        {
            var result = new bool[checker.Count];

            for (int i = 0; i < checker.Count; i++)
                if (checker[i]) result[i] = true;

            return result;
        }

        private List<short> Reorganize(List<short> candidateItems)
        {
            var lowExternal = short.MaxValue;

            for (var i = 0; i < Size; i++)
            {
                var externalIdx = 2 * i + 1;

                if (candidateItems[externalIdx - 1] < lowExternal)
                    lowExternal = candidateItems[externalIdx - 1];
            }

            var starting = candidateItems.IndexOf(lowExternal);

            var result = new List<short>();

            for (int i = 0; i < candidateItems.Count; i++)
            {
                var idx = (starting + i) % candidateItems.Count;
                result.Add(candidateItems[idx]);
            }

            return result;
        }

        public bool IsMagic()
        {
            var sum = 0;

            for (var i = 0; i < Size; i++)
            {
                var first = 2 * i + 1;
                var second = first + 1;
                var third = second + 2;

                if (third > 2 * Size)
                    third = 2;

                var candidateSum = Digits[first - 1] + Digits[second - 1] + Digits[third - 1];

                if (sum != 0 && candidateSum != sum)
                    return false;

                sum = candidateSum;
            }

            return true;
        }

        public long PublishDigit()
        {
            List<short> concatenation = new List<short>();

            for (var i = 0; i < Size; i++)
            {
                var first = 2 * i + 1;
                var second = first + 1;
                var third = second + 2;

                if (third > 2 * Size)
                    third = 2;

                concatenation.AddRange(Decomposition.Decompose(Digits[first - 1], 10));
                concatenation.AddRange(Decomposition.Decompose(Digits[second - 1], 10));
                concatenation.AddRange(Decomposition.Decompose(Digits[third - 1], 10));
            }

            return Decomposition.Recompose(concatenation, 10);
        }

        public static long FindMaximumDigit_10()
        {
            long max = 0;

            foreach (var digitChoice in Permutations.BuildIndexPermutations(10))
            {
                NGonRing ring;

                long value = Decomposition.Recompose(digitChoice.Select(x => (short)x).ToList(), 10);

                if (TryBuild(value, out ring)
                    && ring.IsMagic())
                {
                    if (ring.PublishDigit() > max)
                        max = ring.PublishDigit();
                }
            }

            return max;
        }

        public static long FindMaximumDigit_Alternative_10()
        {
            long max = 0;

            var permutations = Permutations.BuildIndexPermutations(5).ToList();

            foreach (var innerDigitChoice in Permutations.BuildIndexChoices(9, 5))
            {
                foreach (var permutation in permutations)
                {
                    var candidate = Permutations.Apply(innerDigitChoice, permutation);

                    for (var i = 14; i < 17; i++) // 13 = 10+1+2 et 20 = 10+9+1 -> found the bounds experimentally
                    {
                        NGonRing ring;

                        if (!AdvancedTryBuild(candidate, i, out ring))
                            continue;

                        if (ring.PublishDigit() > max)
                            max = ring.PublishDigit();
                    }
                }
            }

            return max;
        }
    }
}