using System;
using System.Collections.Generic;

namespace Euler.Core
{
	public class PowerFinder
	{
		private int _power;
		private double _inversePower;
		private long _powLimit;
		private SortedList<int, long> _powers;

		public PowerFinder(int power)
		{
			_power = power;
			_inversePower = 1.0 / power;
			_powers = new SortedList<int, long>();			
		}

		public bool this[long index]
		{
			get
			{
				if (index > _powLimit)
				{
					_powers.Clear();
					var formerLimit = Math.Pow(_powLimit, _inversePower);
					var limit = Math.Pow(index, _inversePower);

					for (int i = 0; i < limit; i++)
						_powers.Add(i, (long)Math.Pow(i, _power));

					_powLimit = index;
				}

				return _powers.ContainsValue(index);
			}
		}

		public int GetNthSquare(long root)
		{
			if ( _powers.ContainsValue(root))
			{
				var index = _powers.IndexOfValue(root);
				return _powers.Keys[index];
			}

			throw new ArgumentOutOfRangeException("Nth square not defined !");
		}

		public long GetNthSquare(int root)
		{
			if (!_powers.ContainsKey(root))
			{
				long square = (long)Math.Pow(root, _power);
				_powers.Add(root, square);
			}

			return _powers[root];
		}

		public IEnumerable<long> FindPowersWithAdd(double max, double min, long supplement)
		{
			double lowerBound = 0, upperBound = 0;
			
			upperBound = Math.Max(Math.Abs(max), Math.Abs(min));

			if (Math.Sign(max*min) == -1)
				lowerBound = 0;
			else
				lowerBound = Math.Min(Math.Abs(max), Math.Abs(min));


			int start = (int)Math.Pow(lowerBound, _inversePower), end = (int)Math.Pow(upperBound, _inversePower);

			for (int i = start; i < end; i++)
			{
				long tester = GetNthSquare(i) + supplement;

				if (this[tester])
					yield return GetNthSquare(tester);
			}
		}
	}
}
