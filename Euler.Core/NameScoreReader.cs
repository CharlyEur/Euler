using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Euler.Core
{
	class NameScoreReader
	{
		private const string _Names_Path = @"..\Euler.Core\p022_names.txt";

		public static long FindHighestScore()
		{
			var names = ReadNames().ToList();
			names.Sort();

			var rank = 0;
			long total = 0;

			foreach (var item in names)
			{
				rank++;
				long localScore = rank * ComputeScore(item);
				total += localScore;
			}

			return total;
		}

		private static IEnumerable<string> ReadNames()
		{
			foreach (var line in File.ReadLines(_Names_Path))
				yield return line;
		}

		private static long ComputeScore(string item)
		{
			long score = 0;

			foreach (var letter in item)
				score += ((int) letter - 64);

			return score;
		}
	}
}
