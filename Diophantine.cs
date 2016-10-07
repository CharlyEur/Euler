using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DioPoint = System.Tuple<System.Numerics.BigInteger, System.Numerics.BigInteger>;

namespace Euler.Core
{
    internal class Diophantine
    {
        private readonly double _sqrtD;
        private readonly long _dKernel;

        internal long DKernel { get { return _dKernel; } }
        internal double SqrtD { get { return _sqrtD; } }

        public Diophantine(long d)
        {
            _dKernel = d;
            _sqrtD = Math.Sqrt(d);
        }

        internal static long FindMinimalAbove()
        {
            long kernelBuffer = 0;
            BigInteger solutionBuffer = 0;

            for (int i = 2; i <= 1000; i++)
            {
                if (GeometricNumbersProvider.IsSquare(i))
                    continue;

                var diophantineResult = new Diophantine(i).FindMinimumSolution();

                if (diophantineResult.Item1 > solutionBuffer)
                {
                    solutionBuffer = diophantineResult.Item1;
                    kernelBuffer = i;
                }
            }

            return kernelBuffer;
        }

        /// <summary>
        /// Return solution for x² - multiplier.y² = 1
        /// </summary>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static DioPoint FindMinimalSolutionFor(long multiplier)
        {
            var newComputer = new Diophantine(multiplier);

            return newComputer.FindMinimumSolution();
        }

        private BigInteger Evaluate(DioPoint couple)
        {
            return BigInteger.Pow(couple.Item1, 2) - DKernel * BigInteger.Pow(couple.Item2, 2);
        }

        internal Tuple<BigInteger, BigInteger> FindMinimumSolution()
        {
            var couple = FindQuasiSolution();

            return SolveBrahmagupta(couple);
        }

        private DioPoint SolveBrahmagupta(DioPoint alpha)
        {
            var choice = Evaluate(alpha);

            if (choice == 1)
                return alpha;

            if (choice == -1)
                return Multiply(alpha, alpha);

            if (choice == 2 || choice == -2)
                return Divide(Multiply(alpha, alpha), 2);

            if (choice == 4)
            {
                if (IsDivisible(alpha, 2))
                    return Divide(alpha, 2);

                if (DKernel % 2 == 0)
                    return Divide(Multiply(alpha, alpha), 4);

                return Divide(Multiply(alpha, Multiply(alpha, alpha)), 8);
            }

            if (choice == -4)
            {
                if (IsDivisible(alpha, 2))
                {
                    var cobaye = Divide(alpha, 2);
                    return Multiply(cobaye, cobaye);
                }

                if (DKernel % 2 == 0)
                    return Divide(Multiply(alpha, alpha), 4);

                var intermediary = Divide(Multiply(alpha, Multiply(alpha, alpha)), 8);
                return Multiply(intermediary, intermediary);
            }

            throw new ArgumentOutOfRangeException("alpha", @"DioPoint was not a quasi-solution");
        }

        private DioPoint FindQuasiSolution()
        {
            BigInteger norm = 0;
            DioPoint guess = new DioPoint(1, 0);

            while (BigInteger.Abs(norm) != 4
                && BigInteger.Abs(norm) != 2
                && BigInteger.Abs(norm) != 1)
            {
                guess = GenerateNextStep(guess, norm);
                norm = Evaluate(guess);
            }

            return guess;
        }

        private DioPoint GenerateNextStep(DioPoint guess, BigInteger kNorm)
        {
            DioPoint beta = BuildBeta(guess, kNorm);

            if (kNorm == 0)
                kNorm = Evaluate(guess);

            return Divide(Multiply(guess, beta), BigInteger.Abs(kNorm));
        }

        private DioPoint BuildBeta(DioPoint guess, BigInteger kNorm)
        {
            if (guess.Item2 == 0)
                return new DioPoint((long)Math.Round(_sqrtD), 1);

            var m = FindM(guess.Item1, guess.Item2, kNorm);

            return new DioPoint(m, 1);
        }

        /// <summary>
        /// m is to be found such that b.m - a = 0 [k]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private BigInteger FindM(BigInteger a, BigInteger b, BigInteger k)
        {
            var ra = ReduceMod(-a, k);
            var rb = ReduceMod(b, k);

            var constraint = FindConstraint(ra, rb, k);

            return MinimizeBetaNorm(k, constraint);
        }

        private static BigInteger FindConstraint(BigInteger ra, BigInteger rb, BigInteger norm)
        {
            BigInteger remainder;
            var candidate = BigInteger.DivRem(ra, rb, out remainder);

            if (remainder.IsZero)
                return candidate;

            for (var i = 0; i < BigInteger.Abs(norm); i++)
            {
                candidate = i * norm + ra;

                if (candidate % rb != 0)
                    continue;

                var realCandidate = candidate / rb;
                return ReduceMod(realCandidate, norm);
            }

            throw new ArithmeticException(string.Format(@"Could not find satisfying constraint m*{1} - {0} = 0 [{2}]", ra, rb, norm));
        }

        private static BigInteger ReduceMod(BigInteger dividend, BigInteger divisor)
        {
            BigInteger result;
            BigInteger.DivRem(dividend, divisor, out result);

            //if (result < 0)
            //    result += divisor;

            return result;
        }

        // minimize Norm(beta) with m = constraint + p * k --> roots are quite easy to compute
        private BigInteger MinimizeBetaNorm(BigInteger norm, BigInteger constraint)
        {
            var points = new List<BigInteger>
            {
                ((long) Math.Ceiling(_sqrtD) - constraint) / norm,
                ((long) Math.Floor(_sqrtD) - constraint) / norm,
                (-(long) Math.Ceiling(_sqrtD) - constraint) / norm,
                (-(long) Math.Floor(_sqrtD) - constraint) / norm,
            }
            .Distinct()
            .ToList();

            Func<BigInteger, BigInteger> alphaComputer = (x => constraint + x * norm);
            Func<BigInteger, BigInteger> normComputer = (x => BigInteger.Abs((constraint + x * norm) * (constraint + x * norm) - DKernel));

            var tester = points.ToDictionary(x => alphaComputer(x), x => normComputer(x));

            var min = tester.Where(x => x.Key > 0).Min(x => x.Value);

            var alpha = tester.First(y => y.Key > 0 && y.Value == min).Key;

            return alpha;// * norm + constraint;
        }


        private static bool IsDivisible(DioPoint tuple, int p)
        {
            BigInteger controlRemainder;
            BigInteger.DivRem(tuple.Item1, p, out controlRemainder);

            if (controlRemainder != 0)
                return false;

            BigInteger.DivRem(tuple.Item2, p, out controlRemainder);
            if (controlRemainder != 0)
                return false;

            return true;
        }

        private DioPoint Divide(DioPoint tuple, BigInteger p)
        {
            BigInteger controlRemainder;

            var first = BigInteger.DivRem(tuple.Item1, p, out controlRemainder);

            if (controlRemainder != 0)
                throw new ArgumentOutOfRangeException(string.Format("Item1 [{0}] not divisible by {1}", tuple.Item1, p));

            var second = BigInteger.DivRem(tuple.Item2, p, out controlRemainder);
            if (controlRemainder != 0)
                throw new ArgumentOutOfRangeException(string.Format("Item2 [{0}] not divisible by {1}", tuple.Item2, p));

            return new DioPoint(first, second);
        }

        private DioPoint Multiply(DioPoint couple1, DioPoint couple2)
        {
            var first = couple1.Item1 * couple2.Item1 + couple1.Item2 * couple2.Item2 * DKernel;
            var second = couple1.Item2 * couple2.Item1 + couple1.Item1 * couple2.Item2;

            return new DioPoint(first, second);
        }
    }
}