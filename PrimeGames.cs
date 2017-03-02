using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class PrimeGames
    {
        private PrimalityProvider Source { get; set; }
		private PhiComputer PhiHelper { get; set; }

		public static long FindBiggestDecimalUnder(int upperLimit)
		{
			var primeService = new PrimalityProvider(upperLimit);
			var phiComputer = new PhiComputer { Source = primeService };
			phiComputer.PopulatePhiValues();

			var engine = new PrimeGames { Source = primeService, PhiHelper = phiComputer };

			return engine.ComputePeriodicity(upperLimit);
		}

		internal long ComputePeriodicity(int upperLimit)
		{
			long max = 0;
			long maxOrder = 0;

			for (int i = upperLimit - 1; i >= 0; i--)
			{
				if (PhiHelper.PhiValues[i] >= maxOrder)
				{
					var candidate = RemoveFactors(i, 10);
					var order = FindOrder(candidate, 10);

					if (order > maxOrder)
					{
						max = candidate;
						maxOrder = order;
					}
				}
			}

			return max;
		}

		private long FindOrder(long n, long unit)
		{
			var tester = unit % n;
			var counter = 0;

			while(tester != 1)
			{
				tester = (unit * tester) % n;
				counter++;
			}

			return counter;
		}

		internal long RemoveFactors(long candidate, long toRemove)
		{
			var factors = Source.Decompose(candidate);
			var toElimate = Source.Decompose(toRemove);

			foreach (var primeItem in toElimate)
			{
				if (factors.ContainsKey(primeItem.Key))
					factors[primeItem.Key] = factors[primeItem.Key] % primeItem.Value;
			}

			return Source.Recompose(factors);
		}

        public static long FindContraGoldbach()
        {
            long index = 1;

            var gamer = new PrimeGames { Source = new PrimalityProvider(1000000) };

            while (true)
            {
                index += 2;

                if (!gamer.IsComposite(index))
                    continue;

                if (gamer.TryToFindWeakGoldbachWriting(index))
                    return index;
            }
        }
        
        internal static long Find37RightNeighborNumerator(int maxDenominator)
        {
            var gamer = new PrimeGames { Source = new PrimalityProvider(maxDenominator) };

            return gamer.FindRightNeighborNumerator(3, 7, maxDenominator);
        }

        internal static long FindReducedCount(int maxDenominator)
        {
            var provider = new PrimalityProvider(maxDenominator);

            var phiComputer = new PhiComputer { Source = provider };

            phiComputer.PopulatePhiValues();
            
            return phiComputer.PhiValues.Values.Sum();
        }
        
        internal static long FindFixedCount(int maxDenominator, Tuple<int,int> lowBound, Tuple<int,int> upBound)
        {
            var provider = new FractionFactory(maxDenominator);

            var cache = new HashSet<Fraction>();

            for (int i = 2; i <= maxDenominator; i++)
            {
                int lowerBound = (int)((double)i * lowBound.Item1 / lowBound.Item2);
                int upperBound = (int)((double)i * upBound.Item1 / upBound.Item2);

                for (int j = lowerBound+1; j <= upperBound; j++)
                    cache.Add(provider.Create(j, i));
            }

            return cache.Count-1;
        }

        public static IEnumerable<long> FindPrimeFactors(long input)
        {
            var player = new PrimalityProvider((int) Math.Sqrt(input));

            return player.Decompose(input).Keys;
        }

        public static long FindNthPrime(int idx)
        {
            int tenthPower = 3;

            var provider = new PrimalityProvider(10);

            while(provider.Count < idx)
            { 
                int upperBound = (int) Math.Pow(10, tenthPower);
                provider = new PrimalityProvider(upperBound);
                tenthPower += 2;
            }

            return provider[idx-1]; // -> 1 is not in this prime set
        }

        internal static long SumPrimesUnder(int upperBound)
        {
            var gamer = new PrimeGames { Source = new PrimalityProvider(upperBound) };

            return gamer.Source.Sum();
        }

        public static long FindDistinctFourSequence()
        {
            var gamer = new PrimeGames { Source = new PrimalityProvider(1000000) };

            return gamer.PlayOnFour();
        }

        public static long FindMagicPermutation()
        {
            var gamer = new PrimeGames { Source = new PrimalityProvider(10000) };

            return gamer.RotatePrimeFour();
        }

        public static long FindConsecutivePrimeSum()
        {
            var limit = 1000000;

            var gamer = new PrimeGames { Source = new PrimalityProvider(limit) };

            var candidates = gamer.ComputeConsecutiveSums(limit);

            long result = 0;
            int maxLength = 0;

            foreach (var primeSum in candidates)
            {
                if (primeSum.Value > maxLength)
                {
                    result = primeSum.Key.Item1;
                    maxLength = primeSum.Value;
                }
            }

            return result;
        }
        
        public static long FindSmallestReplacementFamily()
        {
            var limit = 1000000;

            var gamer = new PrimeGames { Source = new PrimalityProvider(limit) };
            
            return gamer.FindEigthMembersFamily();
        }

        internal static long SpiralSizeUnderTenPct()
        {
            double ratio = 1;
            long step = 0;
            long primeCount = 0;
            long totalCount = 1;
            long lastPrime = 1;

            var provider = new PrimalityProvider(100000000);

            while (ratio > 0.1)
            {
                step++;
                long spread = 2 * step;

                long primeA = lastPrime + spread;
                long primeB = lastPrime + 2 * spread;
                long primeC = lastPrime + 3 * spread;
                long primeD = lastPrime + 4 * spread;

                lastPrime = primeD;

                if (provider.SafeIsPrime(primeA))
                    primeCount++;

                if (provider.SafeIsPrime(primeB))
                    primeCount++;

                if (provider.SafeIsPrime(primeC))
                    primeCount++;

                if (provider.SafeIsPrime(primeD))
                    primeCount++;

                totalCount += 4;

                ratio = (double)primeCount / totalCount;
            }

            return 2 * step + 1;
        }
        
        public static long FindGreatestPhiRatio()
        {
            var limit = 1000001;

            var gamer = new PrimeGames { Source = new PrimalityProvider(limit) };

            return gamer.FindMaxPhiRatio(limit);
        }

        public static long FindMinimumPhiRatioWithPermutation()
        {
            var limit = (int)(1e7 / 13);

            var gamer = new PrimeGames { Source = new PrimalityProvider(limit) };

            return gamer.FindPermutation_MinPhiRatio((long)1e7);
        }

        private long FindPermutation_MinPhiRatio(long limit)
        {
            double ratio = 5.0;
            long result = 0;            

            foreach (var highPrime in Source.Reverse())
            {
                var limiter = (double)limit / highPrime;

                foreach (long smallPrime in Source)
                {
                   if (smallPrime > limiter)
                        break;

                    var candidate = highPrime * smallPrime;
                    var phiCandidate = (highPrime - 1) * (smallPrime - 1);
                    
                    if (DigitReader.HasSameDigits(candidate, phiCandidate))
                    {
                        var testRatio = (double)candidate / phiCandidate;

                        if (testRatio < ratio)
                        {
                            ratio = testRatio;
                            result = candidate;
                        }
                    }
                }
            }
            return result;
        }

        private long FindMaxPhiRatio(int limit)
        {
            long maxBuffer = 0;
            double maxRatio = 0.0;
            
            for (int i = 1; i < limit; i++)
            {
                if (Source.IsPrime(i))
                    continue;

                var ratio = (double)i / Source.Phi(i);
                
                if (!(ratio > maxRatio))
                    continue;
                
                maxBuffer = i;
                maxRatio = ratio;
            }
            
            return maxBuffer;
        }

        private long FindEigthMembersFamily()
        {
            var results = new List<long>();

            foreach (var prime in Source)
            {
                var decomposition = Decomposition.Decompose(prime, 10);
                var candidate = new ReplacingDigiter { Digits = decomposition };

                for (int i = 1; i < decomposition.Count; i++)
                {
                    var replacements = Permutations.BuildIndexChoices(decomposition.Count, i);

                    foreach (var replacement in replacements)
                    {
                        int decompoScore = 10;
                        candidate.ReplacedIndex = replacement;

                        foreach (var primeABe in candidate.Family)
                        {
                            if (!Source.IsPrime(primeABe))
                                decompoScore--;

                            if (decompoScore < 8)
                                break;
                        }

                        if (decompoScore < 8)
                            continue;

                        var paterFamily = candidate.Family.First(Source.IsPrime);

                        if (!results.Contains(paterFamily))
                            results.Add(paterFamily);
                    }
                }
            }

            return results.Where(x => x > 100000).Min(); // because of unclear rule : first digit can be in pattern but not prime itself...
        }

        private Dictionary<Tuple<long, int>, int> ComputeConsecutiveSums(long limitSize)
        {
            var result = new Dictionary<Tuple<long, int>, int>();

            for (int i = 0; i < Source.Count; i++)
            {
                var candidate = ComputeMaxPrimeSum(i, limitSize);
                var key = new Tuple<long, int>(candidate.Key, i);

                if (candidate.Value > 1)
                    result.Add(key, candidate.Value);
            }

            return result;
        }
        
        private KeyValuePair<long, int> ComputeMaxPrimeSum(int startingIdx, long limitSize)
        {
            long sum = 0;
            long max = 0;
            var length = 0;

            for (int i = startingIdx; i < Source.Count; i++)
            {
                sum += Source[i];

                if (sum >= limitSize)
                    break;

                if (!Source.IsPrime(sum))
                    continue;

                max = sum;
                length = i - startingIdx + 1;
            }

            return new KeyValuePair<long, int>(max, length);
        }

        private long RotatePrimeFour()
        {
            var resultCombinations = new List<List<short>>();

            foreach (var startingPoint in Source)
            {
                var resultDigits = new List<short>();

                for (var i = 1; i < 5000; i++)
                {
                    var x = startingPoint + i;

                    if (x >= 10000 || !Source.IsPrime(x))
                        continue;

                    var y = startingPoint + 2 * i;

                    if (y > 10000 || !Source.IsPrime(y))
                        continue;

                    if (CheckRotationCondition(startingPoint, x, y))
                    {
                        resultDigits.AddRange(Decomposition.Decompose(startingPoint, 10));
                        resultDigits.AddRange(Decomposition.Decompose(x, 10));
                        resultDigits.AddRange(Decomposition.Decompose(y, 10));

                        resultCombinations.Add(resultDigits);
                    }
                }
            }

            return Decomposition.Recompose(resultCombinations[1], 10);
        }
        
        internal static bool CheckRotationCondition(long start, long x, long y)
        {
            if (DigitReader.HasSameDigits(start, x))
                return DigitReader.HasSameDigits(start, y);
            
            return false;
        }
        
        private long PlayOnFour()
        {
            long start = 1;

            while (true)
            {
                var one = Source.Decompose(start);
                var two = Source.Decompose(start + 1);
                var three = Source.Decompose(start + 2);
                var four = Source.Decompose(start + 3);

                if (one.Count == 4
                 && two.Count == 4
                 && three.Count == 4
                 && four.Count == 4
                 && AreDistinct(one, two)
                 && AreDistinct(two, three)
                 && AreDistinct(three, four))
                    return start;

                start++;
            }
        }

        private static bool AreDistinct(Dictionary<long, long> a, Dictionary<long, long> b)
        {
            foreach (var factor in a.Keys)
                if (b.ContainsKey(factor))
                    return false;

            return true;
        }

        private bool IsComposite(long candidate)
        {
            return !Source.IsPrime(candidate);
        }

        private bool TryToFindWeakGoldbachWriting(long index)
        {
            double limit = Math.Sqrt((double)index / 2);

            for (var i = 1; i < limit; i++)
            {
                var candidate = index - 2 * i * i;

                if (Source.IsPrime(candidate))
                    return false;
            }

            return true;
        }

        internal static long FindFivePrimeFamilySum()
        {
            var provider = new PrimalityProvider(10000000);

            var gamer = new PrimeGames { Source = provider };

            return gamer.ComputeFiveCountSetSum();
        }

        private long ComputeFiveCountSetSum()
        {
            var maxPrimeTested = Source.FirstIndexAbove(10000);

            for (int i = 1; i < maxPrimeTested; i++)
            {
                var aDigits = Decomposition.Decompose(Source[i], 10);

                for (int j = i + 1; j < maxPrimeTested; j++)
                {
                    var bDigits = Decomposition.Decompose(Source[j], 10);

                    if (!TestPrimality(aDigits, bDigits))
                        continue;

                    for (int k = j + 1; k < maxPrimeTested; k++)
                    {
                        var cDigits = Decomposition.Decompose(Source[k], 10);

                        if (!TestPrimality(aDigits, cDigits)
                         || !TestPrimality(bDigits, cDigits))
                            continue;

                        for (int l = k + 1; l < maxPrimeTested; l++)
                        {
                            var dDigits = Decomposition.Decompose(Source[l], 10);

                            if (!TestPrimality(aDigits, dDigits)
                             || !TestPrimality(bDigits, dDigits)
                             || !TestPrimality(cDigits, dDigits))
                                continue;

                            for (int m = l + 1; m < maxPrimeTested; m++)
                            {
                                var eDigits = Decomposition.Decompose(Source[m], 10);

                                if (!TestPrimality(aDigits, eDigits)
                                 || !TestPrimality(bDigits, eDigits)
                                 || !TestPrimality(cDigits, eDigits)
                                 || !TestPrimality(dDigits, eDigits))
                                    continue;

                                return Source[i] + Source[j] + Source[k] + Source[l] + Source[m];
                            }
                        }
                    }
                }
            }

            return 0;
        }

        private bool TestPrimality(List<short> primeA, List<short> primeB)
        {
            if (Decomposition.DigitSum(primeA, primeB) % 3 == 0)
                return false;

            return Source.SafeIsPrime(Decomposition.Recompose(primeA.Concat(primeB).ToList(), 10))
                && Source.SafeIsPrime(Decomposition.Recompose(primeB.Concat(primeA).ToList(), 10));
        }

        private long FindRightNeighborNumerator(int numerator, int denominator, int maxDenominatorSize)
        {
            double ratio = ((double)numerator) / denominator;

            double diff = 1;
            int candidateNum = 0;
            int candidateDenom = 0;

            for (int i = 1; i <= maxDenominatorSize; i++)
            {
                if (i % denominator == 0)
                    continue;

                int wannabeApprox = (int)(i * ratio);

                var tempDiff = ratio - ((double)wannabeApprox) / i;

                if (tempDiff < diff)
                {
                    diff = tempDiff;
                    candidateNum = wannabeApprox;
                    candidateDenom = i;
                }
            }

            return candidateNum;
        }
    }
}