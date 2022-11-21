using System;
using System.Collections.Generic;
using Pair = System.Tuple<long, long>;

namespace Euler.Core
{
	public class RecursiveAdder
	{
		public PrimalityProvider Source { get; private set; }

		public RecursiveAdder(PrimalityProvider source)
		{			
			Source = source;
		}

		internal int HowToWriteWithPrimes(int number)
		{
			var counter = 0;
			int initIndex = Source.FirstIndexAbove(number);

			RecursiveHowToWriteWithPrimes(number, 0, new Stack<Pair>(), new Pair( initIndex, Source[initIndex] ), ref counter);

			return counter;
		}

		internal void RecursiveHowToWriteWithPrimes(int number, long currentBuffer, Stack<Pair> currentStack, Pair unit, ref int counter)
		{
			for (int i = 0; i <= number / unit.Item2; i++)
			{
				long newBuffer = currentBuffer + i * unit.Item2;
				if (newBuffer > number)
					break;

				if (newBuffer == number)
				{
					currentStack.Push(new Pair(i, unit.Item2));
					counter++;
					
					currentStack.Clear();
					break;
				}

				if (unit.Item1 != 0)
				{
					if (i > 0)
						currentStack.Push(new Pair(i, unit.Item2));

					RecursiveHowToWriteWithPrimes(number, newBuffer, currentStack, NextUnit(unit), ref counter);
				}
			}
		}

		private Pair NextUnit(Pair index)
		{
			return new Tuple<long, long>(index.Item1-1, Source[(int) index.Item1 - 1]);
		}
	}
}
