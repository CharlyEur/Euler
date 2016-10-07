using System.Collections.Generic;

namespace Euler.Core
{
    internal class ConcatenatedProduct
    {
        public int Seed { get; private set; }

        public int MaxOperand { get; private set; }

        public List<short> ResultsDigits { get; private set; }

        public long FullResult { get; private set; }

        private ConcatenatedProduct()
        {
        }

        public static ConcatenatedProduct Compute(int seed, int maxOperand)
        {
            var digits = new List<short>();

            for (int i = 1; i <= maxOperand; i++)
                digits.AddRange(Decomposition.Decompose(seed * i, 10));

            return new ConcatenatedProduct
            {
                FullResult = Decomposition.Recompose(digits, 10),
                MaxOperand = maxOperand,
                ResultsDigits = digits,
                Seed = seed
            };
        }

        public bool IsPandigital()
        {
            if (ResultsDigits.Count != 9)
                return false;

            for (short i = 1; i < 10; i++)
            {
                if (!ResultsDigits.Contains(i))
                    return false;
            }

            return true;
        }
    }
}