using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class Truncatables
    {
        public static long SumOfAll()
        {
            var pool = GetAllTruncatables();

            return pool.Sum();
        }

        private static IEnumerable<long> GetAllTruncatables()
        {
            var source = new PrimalityProvider(1000000);

            var primeSequence = source.GetEnumerator();

            var count = 0;

            while ((count < 11) && primeSequence.MoveNext())
            {
                var candidate = primeSequence.Current;

                if (IsTruncatable(candidate, source))
                {
                    count++;
                    yield return candidate;
                }
            }
        }

        internal static bool IsTruncatable(long candidate, PrimalityProvider source)
        {
            var candidateDigits = Decomposition.Decompose(candidate, 10);
            var digitNumber = candidateDigits.Count;

            if (digitNumber == 1)
                return false;

            // aller
            for (int i = 1; i < digitNumber; i++)
            {
                candidateDigits.RemoveAt(0);
                var primeCandidate = Decomposition.Recompose(candidateDigits, 10);

                if (!source.IsPrime(primeCandidate))
                    return false;
            }

            candidateDigits.Clear();

            candidateDigits = Decomposition.Decompose(candidate, 10);

            // retour
            for (int i = 1; i < digitNumber; i++)
            {
                candidateDigits.RemoveAt(candidateDigits.Count - 1);
                var primeCandidate = Decomposition.Recompose(candidateDigits, 10);

                if (!source.IsPrime(primeCandidate))
                    return false;
            }

            return true;
        }
    }
}