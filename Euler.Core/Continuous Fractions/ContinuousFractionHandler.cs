using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler.Core
{
    public class ContinuousFractionHandler
    {
        public static long CountSquareRootTwo(int limit)
        {
            var fractions = SequentialApproxSquareRootTwo(limit);
            long count = 0;

            foreach (var approx in fractions)
            {
                if (Math.Floor(BigInteger.Log10(approx.Item1)) > Math.Floor(BigInteger.Log10(approx.Item2)))
                    count++;
            }

            return count;
        }

        public static long CountOddFrequencyUnder(long limit)
        {
            long count = 0;

            for (int i = 2; i <= limit; i++)
            {
                var continuousFraction = ContinuousFractionsFactory.AdvancedRootCreate(i);

                if (continuousFraction.Frequency % 2 == 1)
                    count++;
            }

            return count;
        }

        public static long SumDigit100th_e()
        {
            var e_Coefficients = ApproxExponential1(99);
            var numeratorDigits = Decomposition.Decompose(e_Coefficients.Item1);
            return Decomposition.DigitSum(numeratorDigits);
        }



        internal static IEnumerable<Tuple<BigInteger, BigInteger>> SequentialApproxSquareRootTwo(int limit)
        {
            var u = new Tuple<BigInteger, BigInteger>(3, 2);

            for (int i = 0; i <= limit; i++)
            {
                yield return u;
                u = new Tuple<BigInteger, BigInteger>(u.Item1 + 2 * u.Item2, u.Item1 + u.Item2);
            }
        }

        internal static Tuple<BigInteger, BigInteger> ApproxExponential1(int rank)
        {
            List<long> continuousFractionCoefficients = GenerateECoeffsUntil(rank).ToList();

            return ReadCoefficients(continuousFractionCoefficients);
        }

        internal static Tuple<BigInteger, BigInteger> ReadCoefficients(List<long> continuousFractionCoefficients)
        {
            var workingCopy = continuousFractionCoefficients.ToArray();

            BigInteger numerator = workingCopy[workingCopy.Length - 1];
            BigInteger denominator = 1;

            for (var i = workingCopy.Length - 2; i >= 0; i--)
            {
                var localNumerator = numerator;
                numerator = workingCopy[i] * numerator + denominator;
                denominator = localNumerator;
            }

            return new Tuple<BigInteger, BigInteger>(numerator, denominator);
        }

        private static IEnumerable<long> GenerateECoeffsUntil(int rank)
        {
            yield return 2;

            for (var i = 0; i < rank; i++)
            {
                if (i % 3 == 1)
                    yield return 2 * (i / 3 + 1);
                else
                    yield return 1;
            }
        }
    }
}