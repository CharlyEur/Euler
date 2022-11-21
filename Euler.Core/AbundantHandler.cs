using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
	class AbundantHandler
	{
		private PrimalityProvider _provider;

		private List<long> _abundantProvision;
		private readonly int _limitSize;

		public AbundantHandler(int upperLimit)
		{
			_provider = new PrimalityProvider(upperLimit);
			_abundantProvision = new List<long>();

			for (int i = 1; i < upperLimit; i++)
			{
				if (IsAbundant(i))
					_abundantProvision.Add(i);
			}

			_limitSize = _abundantProvision.Count / 2 + 1;
		}

		private bool IsAbundant(int i)
		{
			var divisors = _provider.BuildDivisors(i);

			divisors.Remove(i);

			return (divisors.Sum() > i);
		}

		internal long SumNotSums()
		{
			long limit = 28123;
			var sieve = new bool[limit+1];

			for (int i = 0; i < _abundantProvision.Count; i++)
			{
				for (int j = 0; j <= i; j++)
				{
					var index = _abundantProvision[i] + _abundantProvision[j];

					if (index > limit)
						break;

					sieve[index] = true;
				}
			}

			long sum = 0;
			for (int i = 0; i < limit; i++)
			{
				if (!sieve[i])
					sum += i;
			}

			return sum;
		}
		
		/* test is good, result is ok but within 3 minutes
		 
		internal long SumNotSums()
		{
			long limit = 28123;

			long sum = 0;

			for (int i = 0; i < limit; i++)
			{
				if (!CanBeWrittenAsAbundantSum(i))
					sum += i;
			}

			return sum;
		}
		*/
		private bool CanBeWrittenAsAbundantSum(int candidate)
		{
			for (int i = 0; i < _limitSize; i++)
			{
				var toSearch = candidate - _abundantProvision[i];

				if (toSearch < 0)
					break;

				if (_abundantProvision.Contains(toSearch))
					return true;
			}

			return false;
		}
	} 
}
