using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    internal class FactorialChainer
    {
        private SortedDictionary<int, int> digitFactorials;
        private SortedDictionary<int, int> chainLengthCache;

        public FactorialChainer(int numericalBase)
        {
            chainLengthCache = new SortedDictionary<int, int>();
            digitFactorials = new SortedDictionary<int, int>();

            for (int i = 0; i < numericalBase; i++)
                digitFactorials.Add(i, (int) SimpleFactorial(i));

			chainLengthCache.Add(0, 2);
			chainLengthCache.Add(1, 1);
			chainLengthCache.Add(2, 1);
			chainLengthCache.Add(145, 1);
			chainLengthCache.Add(169, 3);
			chainLengthCache.Add(871, 2);
			chainLengthCache.Add(872, 2);
			chainLengthCache.Add(1454, 3);
			chainLengthCache.Add(363601, 3);
		}

        public int ComputeChainLength(int candidate)
        {
			var chain = new List<int>();
			int next = candidate;

			bool isCached = false;

            while (!chain.Contains(next) && !isCached)
			{
				isCached = chainLengthCache.ContainsKey(next);

				if (!isCached)
				{
					chain.Add(next);
					next = ExtractValue(next);
				}
            }

			var sizeMax = chain.Count;

			if (!isCached)
			{
				chainLengthCache.Add(next, 1);
				sizeMax--;
			}			

			for (int i = 0; i < sizeMax; i++)
				chainLengthCache.Add(chain[i], chain.Count - i + chainLengthCache[next]);
			
			return chainLengthCache[candidate];

		}		

        internal int ExtractValue(int candidate)
        {
            var digits = Decomposition.Decompose(candidate, 10);

            return digits.Select(x => digitFactorials[x]).Sum();
        }

        internal static long SimpleFactorial(long input)
        {
            long result = 1;

            if (input <= 1)
                return 1;

            for (int i = 2; i <= input; i++)
                result *= i;

            return result;
        }
    }
}
