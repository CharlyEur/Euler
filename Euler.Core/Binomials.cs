using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class Binomials
    {
        public static long CountOverLimit(long limit)
        {
            var maxSize = (int)Math.Floor(Math.Sqrt(limit));

            var computer = new Binomials { Provider = new PrimalityProvider(maxSize) };

            return computer.CountOverLimits(limit, 100);
        }
        
        public PrimalityProvider Provider { get; set; }
        
        public long ComputeCnk(long n, long k)
        {
            return Provider.Recompose(ComputeCnkFactors(n, k));
        }

        public Dictionary<long, long> ComputeCnkFactors(long n, long k)
        {
            if (k >= n)
                return new Dictionary<long, long> { { 1, 1 } };

            if (k >= n / 2)
                k = n - k;

            var nFactors = new Dictionary<long, long>();

            for (var i = n - k + 1; i <= n; i++)
                nFactors.Multiply(Provider.Decompose(i));

            for (long i = 2; i <= k; i++)
            {
                var toRemove = Provider.Decompose(i);
                nFactors.Divide(toRemove);
            }

            return nFactors;
        }

        private long CountOverLimits(long limit, int coeffLimit)
        {
            int count = 0;
            var logLimit = Math.Log10(limit);

            for (var i = 0; i <= coeffLimit; i++)
            {
                for (var j = 0; j <= i; j++)
                {
                    var cij = ComputeCnkFactors(i, j);

                    var log10Cij = ComputeLog10(cij);

                    if (log10Cij >= logLimit)
                        count++;
                }
            }

            return count;
        }

        private static double ComputeLog10(Dictionary<long, long> cij)
        {
            double result = 0;

            foreach (var factorPower in cij)
                result += factorPower.Value * Math.Log10(factorPower.Key);

            return result;
        }
    }
}