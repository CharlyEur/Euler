using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
	public class Amicability
	{
		public PrimalityProvider Engine { get; set; }

		public static long SumAmicablesUnder( int upperBound )
		{
			var engine = new Amicability { Engine = new PrimalityProvider(upperBound * 10) };

			return engine.FindAmicableUnder(upperBound).Sum();
		}

		public IEnumerable<long> FindAmicableUnder(int upperBound)
		{
			var result = new HashSet<long>();

			for (int i = 2; i < upperBound; i++)
			{
				long amiBinom = 0;
				if (!result.Contains(i) && TryAmicable(i, out amiBinom))
				{
					result.Add(i);
					result.Add(amiBinom);
				}
			}

			return result;
		}

		public bool TryAmicable(long toTest, out long amicableBinom)
		{
			List<long> divisors = Engine.BuildDivisors(toTest);
			amicableBinom = divisors.Where(x => x < toTest ).Sum();
			List<long> binomDivisors = Engine.BuildDivisors(amicableBinom);

			long validationSum = binomDivisors.Sum() - amicableBinom; // because Divisors contains it

			return toTest == validationSum && toTest != amicableBinom;
		}
	}
}
