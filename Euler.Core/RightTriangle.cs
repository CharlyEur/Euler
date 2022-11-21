using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class RightTriangle
    {
        public static int FindLargestSolutionsSet()
        {
            var maxCards = FindSolutionsSetsUnder(1000);
            return maxCards;
        }

        internal static int FindSolutionsSetsUnder(int maxPerimeter)
        {
            var candidate = 0;
            var champion = 3;

            for (int i = 2; i <= maxPerimeter; i += 2)
            {
                var challenger = FindSolutionsSets(i);

                if (challenger > champion)
                {
                    champion = challenger;
                    candidate = i;
                }
            }

            return candidate;
        }

        internal static int FindSolutionsSets(int perimeter)
        {
            var count = 0;

            for (int x = 1; x < perimeter; x++)
            {
                for (int y = x + 1; y < perimeter - x; y++)
                {
                    if (x * y % perimeter != 0)
                        continue;

                    if (x + y - x * y / perimeter == perimeter / 2)
                        count++;
                }
            }

            return count;
        }
    }
}
