using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public int ComputeChainLength(int candidate)
        {
            var toStart = ExtractValue(candidate);

            var chain = new List<int> { candidate };

            while (!chain.Contains(toStart))
            {
                chain.Add(toStart);

                if (!chainLengthCache.ContainsKey(toStart))
                    toStart = ExtractValue(toStart);
                else
                    break;
            }

            return ProcessChainCreated(chain, toStart);
        }

        private int ProcessChainCreated(List<int> chain, int guiltyMember)
        {
            int length = chain.Count;
            int guiltyIdx = chain.IndexOf(guiltyMember);
            int loopLength = length - guiltyIdx;
            int freePathLength = guiltyIdx;

            for (int i = 0; i < guiltyIdx; i++)
                chainLengthCache.Add(chain[i], (freePathLength - i) + loopLength); // freePath - i = count between i and entering the loop

            for (int i = guiltyIdx; i < length; i++)
                chainLengthCache.Add(chain[i], loopLength);

            return chain.Count;
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
