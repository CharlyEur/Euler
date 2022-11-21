using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Core
{
    internal class FractionFactory
    {
        private SortedDictionary<long, Dictionary<long, long>> decomposer;
        private PrimalityProvider Source { get; set; }

        public FractionFactory(int sizeMax)
        {
            Source = new PrimalityProvider(sizeMax);
            decomposer = new SortedDictionary<long, Dictionary<long, long>>();
        }

        public Fraction Create(long a, long b)
        {
            if (ArePrimes(a, b))
                return new Fraction(a, b);

            return Simplify(a, b);
        }

        internal bool ArePrimes(long a, long b)
        {
            if (a == b || a % b == 0 || b % a == 0)
                return false;

            var aFactors = GetDecomposition(a);

            foreach (var item in aFactors)
            {
                if (b % item.Key == 0)
                    return false;
            }

            return true;
        }

        internal Fraction Simplify(long a, long b)
        {
            var decomposeA = GetDecomposition(a);
            var decomposeB = GetDecomposition(b);

            long aBuffer = a;
            long bBuffer = b;

            foreach (var factor in decomposeA)
            {
                long prime = factor.Key;

                if ( decomposeB.ContainsKey(prime))
                {
                    var aPow = (int)Math.Pow(prime, factor.Value);
                    var bPow = (int)Math.Pow(prime, decomposeB[prime]);

                    var divider = Math.Min(aPow, bPow);

                    aBuffer /= divider;
                    bBuffer /= divider;
                }
            }

            return new Fraction(aBuffer, bBuffer);
        }

        internal Dictionary<long, long> GetDecomposition(long candidate)
        {
            if (!decomposer.ContainsKey(candidate))
                decomposer.Add(candidate, Source.Decompose(candidate));

            return decomposer[candidate];


        }
    }
}
