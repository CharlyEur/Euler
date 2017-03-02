using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Core
{
    public class NumberWriter
    {
        public static long ComputeLetterCount(int upperLimit)
        {
            long total = 0;

            for (int i = 1; i <= upperLimit; i++)
                total += FindLength(i);

            return total;
        }

        internal static int FindLength(int input)
        {
			int letterCount = 0;

			int thousand = input / 1000;

			if (thousand != 0)
				letterCount += FindThousandLength(thousand);

			letterCount += FindNumberLength(input % 1000);

			return letterCount;
        }

		private static int FindNumberLength(int input)
		{
			if (input <= 0)
				return 0;

			if (input > 1000)
				throw new ArgumentOutOfRangeException($"This method handles number under 1000 not {input}");

			int letterCount = 0;
			int hundreds = input / 100;

			letterCount += FindHundredLength(hundreds);

			int tenthPart = input % 100;

			if (tenthPart == 0) // no "and" in three hundred
				return letterCount;

			if (letterCount != 0) // "and" if only some hundred is already present
				letterCount += 3;

			if (tenthPart < 20)
				letterCount += FindUnitLength(tenthPart);
			else
			{
				int tenth = tenthPart / 10;
				int units = tenthPart % 10;

				letterCount += (FindTenthLength(tenth) + FindUnitLength(units));
			}

			return letterCount;
		}

		private static int FindUnitLength(int input)
		{
			if (input <= 0)
				return 0;

			switch (input)
            {
                case 1:			// one
				case 2:         // two
				case 6:         // six
				case 10:        // ten
					return 3;	// xxx
				case 4:			// four
                case 5:         // five
				case 9:         // nine
					return 4;   // xxxx
				case 3:         // three
				case 7:         // seven
				case 8:         // eight
					return 5;   // xxxxx
				case 11:        // eleven
				case 12:        // twelve
					return 6;   // xxxxxx
				case 15:        // fifteen
				case 16:        // sixteen
					return 7;   // xxxxxxx
				case 13:        // thirteen
				case 14:        // fourteen
				case 18:        // eighteen
				case 19:        // nineteen
					return 8;   // xxxxxxxx
				case 17:        // seventeen
					return 9;   // xxxxxxxxx
				default:
                    throw new ArgumentOutOfRangeException($"Could not handle {input}");
            }
        }
        
        private static int FindTenthLength(int input)
        {
            if (input == 0)
                return 0;

            switch (input)
            {
                case 4:         // forty
                case 5:         // fifty
                case 6:			// sixty
                    return 5;   // xxxxx
				case 2:			// twenty
                case 3:			// thirty
                case 8:			// eighty
                case 9:			// ninety
                    return 6;	// xxxxxx
                case 7:			// seventy
                    return 7;   // xxxxxxx
				default:
                    throw new ArgumentOutOfRangeException($"Could not handle {input}");
            }
        }

        private static int FindHundredLength(int input)
        {
            if (input == 0)
                return 0;

            int unitLenght = FindUnitLength(input);

            return unitLenght + 7;//X hundred and _
        }

        private static int FindThousandLength(int input)
        {
            if (input == 0)
                return 0;

            int unitLenght = FindNumberLength(input);

            return unitLenght + 8;//X thousand and _
        }
    }
}
