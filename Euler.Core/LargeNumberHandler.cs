using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
	class LargeNumberHandler
	{
		public static long ComputeExponent(int number, int exponent)
		{
			var digits = CreateOne();

			for (int exp = 0; exp < exponent; exp++)
				digits = MultiplyIntegers(digits, number);

			return Translate(digits);
		}

		public static List<short> ComputeLargeFactorial(int number)
		{
			var result = CreateOne();

			for (var i = 2; i <= number; i++)
				result = MultiplyIntegers(result, i);

			return result;
		}

		internal static List<short> MultiplyIntegers(List<short> completeNumber, int number)
		{
			var digits = Decomposition.Decompose(number);
			digits.Reverse(); // formalism to have units first is local to this class

			var lines = new List<List<short>>();

			for (int i = 0; i < digits.Count; i++)
			{
				var local = completeNumber.DeepCopy();
				MultiplyByOneDigit(local, digits[i]);
				for (int k = 0; k < i; k++)
					local.Insert(0, 0);

				lines.Add(local);
			}

			return ProperSum(lines);
		}

		internal static List<short> ProperSum(List<List<short>> lines)
		{
			var lineMaxSize = lines.Select(x => x.Count).Max();
			var result = new List<short>();

			int remain = 0;

			for (int i = 0; i < lineMaxSize; i++)
			{
				int buffer = remain;

				for (int j = 0; j < lines.Count; j++)
				{
					if (i < lines[j].Count)
						buffer += lines[j][i];
				}

				remain = buffer / 10;
				result.Add((short) (buffer % 10));
			}

			if (remain != 0)
			{
				var remainDigits = Decomposition.Decompose(remain);
				remainDigits.Reverse();

				result.AddRange(remainDigits);
			}

			return result;
		}

		private static void MultiplyByOneDigit(List<short> completeNumber, short digit)
		{
			if (digit > 10 || digit < 0)
				throw new ArgumentOutOfRangeException($"Cannot multiply here by {digit}");

			int remain = 0;

			for (int i = 0; i < completeNumber.Count; i++)
			{
				int localResult = digit * completeNumber[i] + remain;
				remain = localResult / 10;
				completeNumber[i] = (short) (localResult % 10);
			}

			if (remain != 0)
				completeNumber.Add((short) remain);
		}

		private static long Translate(List<short> input)
		{
			input.Reverse();
			var realNumber = Decomposition.Recompose(input, 10);

			return realNumber;
		}

		private static List<short> CreateOne() => new List<short> { 1 };
	}
}
