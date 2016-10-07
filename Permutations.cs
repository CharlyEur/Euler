using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    internal class Permutations
    {
        internal static IEnumerable<int[]> BuildIndexPermutations(int listCount)
        {
            if (listCount == 0)
            {
                yield return new int[0];
                yield break;
            }

            if (listCount == 1)
            {
                yield return new int[] { 0 };
                yield break;
            }

            var permutations = BuildIndexPermutations(listCount - 1).ToList();

            for (var i = 0; i < listCount; i++)
            {
                foreach (var item in permutations)
                {
                    var toManipulate = item.ToList();
                    toManipulate.Insert(i, listCount - 1);
                    yield return toManipulate.ToArray();
                }
            }
        }

        internal static IEnumerable<List<int>> BuildIndexChoices(int n, int k)
        {
            if (k > n)
                throw new ArgumentOutOfRangeException("k", string.Format("k [{0}] cannot be superior to n [{1}]", k, n));

            if (k == 1)
            {
                for (int i = 0; i < n; i++)
                    yield return new List<int> { i };
            }
            else
            {
                if (k == n)
                {
                    yield return BuildList(k);
                }
                else
                {
                    var withN = BuildIndexChoices(n - 1, k - 1);

                    foreach (var choiceSet in withN)
                    {
                        choiceSet.Add(n - 1);
                        yield return choiceSet;
                    }

                    var withoutN = BuildIndexChoices(n - 1, k);

                    foreach (var choiceSet in withoutN)
                        yield return choiceSet;
                }
            }
        }

        private static List<int> BuildList(int total)
        {
            var result = new List<int>();

            for (var i = 0; i < total; i++)
                result.Add(i);

            return result;
        }

        internal static List<T> Apply<T>(List<T> candidate, int[] permutation)
        {
            if (candidate.Count != permutation.Length)
                throw new ArgumentOutOfRangeException();

            var result = new List<T>();

            for (var i = 0; i < permutation.Length; i++)
                result.Add(candidate[permutation[i]]);

            return result;
        }
    }
}