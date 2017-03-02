using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler.Core
{
    public class PrimalityProvider : IEnumerable<long>
    {
        public int MaxSize { get; set; }

        private long[] primeSet { get; set; }
        private bool[] crible;
		
		public PrimalityProvider(int maxSize)
        {
            MaxSize = maxSize;

            InitializePrimeCollection();
        }

        public int Count { get { return primeSet.Length; } }

        public long this[int index]
        {
            get { return primeSet[index]; }
        }

        public bool IsPrime(long toTest)
        {
            return !crible[toTest];
        }

        public bool SafeIsPrime(long toTest)
        {
            if (toTest < MaxSize)
                return IsPrime(toTest);

            if (toTest < Math.Pow(MaxSize, 2))
                return TestPrimality(toTest);

            throw new ArgumentOutOfRangeException("toTest", string.Format("Cannot handle integers above {0}", MaxSize));
        }

        public long FirstIndexAbove(long max)
        {
            long result = 1;

            while (primeSet[result] < max)
                result++;

            return result;
        }

        /// <summary>
        /// To be used only when candidate is above Max Size. Else, a look at the crible should do the job.
        /// </summary>
        /// <param name="toTest"></param>
        /// <returns></returns>
        private bool TestPrimality(long toTest)
        {
            foreach (var primeFactor in primeSet)
            {
                if (toTest % primeFactor == 0)
                    return false;

                if (primeFactor > Math.Sqrt(toTest))
                    return true;
            }
            return true;
        }

        private void InitializePrimeCollection()
        {
            crible = new bool[MaxSize];

            crible[0] = true;
            crible[1] = true;

            var chosenPrime = 2;

            while (chosenPrime < Math.Sqrt(MaxSize))
            {
                long i = 2;
                while (i * chosenPrime < crible.Length)
                {
                    crible[i * chosenPrime] = true;
                    i++;
                }

                chosenPrime = FindNext(crible, chosenPrime);
            }

            ExtractListFromCrible(crible);
        }
    

        private void ExtractListFromCrible(bool[] primeCrible)
        {
            var primeBuffer = new List<long>();

            for (long i = 2; i < primeCrible.Length; i++)
            {
                if (!primeCrible[i])
                    primeBuffer.Add(i);
            }

            primeSet = primeBuffer.ToArray();
        }

        private static int FindNext(IReadOnlyList<bool> crible, int chosenPrime)
        {
            for (var i = 1; i < crible.Count; i++)
            {
                if (!crible[chosenPrime + i])
                    return chosenPrime + i;
            }

            return int.MaxValue;
        }

        public IEnumerator<long> GetEnumerator()
        {
            var list = primeSet.ToList();

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return primeSet.GetEnumerator();
        }
		
		internal List<long> BuildDivisors(long toTest)
		{
			var factors = Decompose(toTest);

			var factorStack = new Stack<Tuple<long, long>>(
					factors.Select(x => new Tuple<long,long>(x.Key, x.Value)));

			var result = ParseDivisors(factorStack).ToList();

			return result;
		}

		internal IEnumerable<long> ParseDivisors(Stack<Tuple<long,long>> factors)
		{
			if (factors.Count == 0)
				yield return 1;
			else
			{
				var monom = factors.Pop();
				var currentDivisors = ParseMonom(monom.Item1, monom.Item2);

				var remainingDivisors = ParseDivisors(factors);

				foreach (var factor in remainingDivisors)
				{
					foreach (var item in currentDivisors)
					{
						yield return factor * item;
					}
				}
			}
		}

		private IEnumerable<long> ParseMonom(long key, long value)
		{
			for (int i = 0; i <= value; i++)
				yield return (long)Math.Pow(key, i);
		}

		internal Dictionary<long, long> Decompose(long candidate)
        {
            var result = new Dictionary<long, long>();

            long buffer = candidate;
            int primIdx = 0;

            while (buffer > 1)
            {
                long primeFactor = primeSet[primIdx];

                while (buffer % primeFactor == 0)
                {
                    buffer /= primeFactor;

                    if (result.ContainsKey(primeFactor))
                        result[primeFactor]++;
                    else
                        result.Add(primeFactor, 1);
                }

                primIdx++;
            }

            return result;
        }
        
        internal Tuple<long, long, long> FindLowestFactor(long candidate)
        {
            if (SafeIsPrime(candidate))
                return new Tuple<long, long, long>(candidate, 1, 1);

            bool divided = false;

            long buffer = candidate;
            int primIdx = 0;
            int power = 0;

            long primeFactor = 0;

            while (!divided)
            {
                primeFactor = primeSet[primIdx];

                while (buffer % primeFactor == 0)
                {
                    buffer /= primeFactor;
                    power++;
                    divided = true;
                }

                primIdx++;
            }

            return new Tuple<long, long, long>(primeFactor, power, buffer);
        }

        internal long Phi(long candidate)
        {
            if (SafeIsPrime(candidate))
                return candidate - 1;

            var primeFactors = Decompose(candidate);

            long result = 1;

            foreach (var factor in primeFactors)
                result *= (factor.Key - 1) * ((long)Math.Pow(factor.Key, factor.Value - 1));

            return result;
        }

        internal long Recompose(Dictionary<long, long> primeFactorDecomposition)
        {
            long result = 1;

            foreach (var factor in primeFactorDecomposition)
                result *= (long)Math.Pow(factor.Key, factor.Value);

            return result;
        }

        internal BigInteger BigRecompose(Dictionary<long, int> bigIntegerDecomposition)
        {
            BigInteger result = 1;

            foreach (var factor in bigIntegerDecomposition)
                result *= BigInteger.Pow(factor.Key, factor.Value);

            return result;
        }

        internal Dictionary<long, int> BigDecompose(BigInteger candidate)
        {
            var result = new Dictionary<long, int>();

            BigInteger buffer = candidate;
            int primIdx = 0;

            while (buffer > 1)
            {
                long primeFactor = primeSet[primIdx];

                while (buffer % primeFactor == 0)
                {
                    buffer /= primeFactor;

                    if (result.ContainsKey(primeFactor))
                        result[primeFactor]++;
                    else
                        result.Add(primeFactor, 1);
                }

                primIdx++;
            }

            return result;
        }

        internal Dictionary<long, int> LowestCommonMultiple(BigInteger a, BigInteger b)
        {
            var potentialResult = BigDecompose(a);
            var bFactors = BigDecompose(b);

            foreach (var factor in bFactors)
            {
                if (potentialResult.ContainsKey(factor.Key))
                    potentialResult[factor.Key] = Math.Max(factor.Value, potentialResult[factor.Key]);
                else
                    potentialResult.Add(factor.Key, factor.Value);
            }

            return potentialResult;
        }

        internal bool ArePrimes(long a, long b)
        {
            if (a == b || a % b == 0 || b % a == 0)
                return false;

            var aFactors = Decompose(a);

            foreach (var item in aFactors)
            {
                if (b % item.Key == 0)
                    return false;
            }

            return true;
        }
    }
}