using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Core
{
    class CollatzHandler
    {
        public static long FindLongestChain( int seedBound )
        {
            long maxSeed = 0;
            long maxLength = 0;
            var runner = new CollatzHandler();

            for (int i = 1; i <= seedBound; i++)
            {
                long chainLength = runner.ComputeLength(i);

                if (chainLength > maxLength)
                {
                    maxLength = chainLength;
                    maxSeed = i;
                }
            }

            return maxSeed;
        }


        private SortedDictionary<long, long> _lengthCache;

        private CollatzHandler()
        {
            _lengthCache = new SortedDictionary<long, long>();
        }

        private long ComputeLength(long seed)
        {
            if (seed == 1)
                return 1;

            if (_lengthCache.ContainsKey(seed))
                return _lengthCache[seed];

            long length = 1 + ComputeLength(NextElement(seed));
            _lengthCache.Add(seed, length);

            return length;
        }

        public long NextElement(long candidate)
        {
            if (candidate % 2 == 0)
                return candidate / 2;

            return 3 * candidate + 1;
        }
    }
}
