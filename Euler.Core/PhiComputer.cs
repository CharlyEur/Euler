using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    internal class PhiComputer
    {
        public SortedDictionary<long, long> PhiValues { get; set; }

        public PrimalityProvider Source { get; set; }

        public void PopulatePhiValues()
        {
            PhiValues = new SortedDictionary<long, long>();
            PhiValues[0] = 0;
            PhiValues[1] = 0;

            if (Source == null)
            {
                throw new NullReferenceException("Cannot populate if Source is null");
            }

            for (long i = 2; i <= Source.MaxSize; i++)
            {
                var factor = Source.FindLowestFactor(i);

                var phiPurePrime = (long) Math.Pow(factor.Item1, factor.Item2 - 1) * (factor.Item1 - 1);

                PhiValues[i] = phiPurePrime;

                if (factor.Item3 != 1)
                    PhiValues[i] *= PhiValues[factor.Item3];
            }
        }

        /// <summary>
        /// Implements divisors count. DivisorCount(Product(pi^ki)) = Product(ki+1) as
        /// you have ki + 1 choices for each factor.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public long CountDivisors(long input)
        {
            var decomposition = Source.Decompose(input);

            long result = decomposition
                            .Select(x => x.Value + 1)
                            .Aggregate((long) 1, (x, y) => x * y);
            return result;
        }

        /// <summary>
        /// Implements arithmetic phi formula. Phi(Product(pi^ki)) = Product((pi-1)*pi^(ki-1))
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public long CountLocalPhi(long input)
        {
            var decomposition = Source.Decompose(input);

            long result = decomposition
                            .Select(x => (x.Key - 1) * ((long) Math.Pow(x.Key, x.Value - 1)))
                            .Aggregate((long) 1, (x, y) => x * y);
            return result;
        }
    }
}
