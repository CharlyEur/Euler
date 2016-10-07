using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new NullReferenceException("Cannot populate if Source is null");

            for (long i = 2; i <= Source.MaxSize; i++)
            {
                var factor = Source.FindLowestFactor(i);

                var phiPurePrime = (long)Math.Pow(factor.Item1, factor.Item2 - 1) * (factor.Item1 - 1);

                PhiValues[i] = phiPurePrime;

                if (factor.Item3 != 1)
                    PhiValues[i] *= PhiValues[factor.Item3];
            }
        }
    }
}
