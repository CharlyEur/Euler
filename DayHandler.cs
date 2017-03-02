using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Core
{
	public class DayHandler
	{
		public static long CountFirstSundays(DateTime startDate, DateTime endDate)
		{
			long buffer = 0;
			DateTime nextSunday = GoToNextSunday(startDate);

			while (nextSunday <= endDate )
			{
				if (nextSunday.Day == 1)
					buffer++;

				nextSunday = nextSunday.AddDays(7);
			}

			return buffer;
		}

		private static DateTime GoToNextSunday(DateTime candidate)
		{
			var weekDay = candidate.DayOfWeek;

			switch(weekDay)
			{
				case DayOfWeek.Monday: return candidate.AddDays(6);
				case DayOfWeek.Tuesday: return candidate.AddDays(5);
				case DayOfWeek.Wednesday: return candidate.AddDays(4);
				case DayOfWeek.Thursday: return candidate.AddDays(3);
				case DayOfWeek.Friday: return candidate.AddDays(2);
				case DayOfWeek.Saturday: return candidate.AddDays(1);
				case DayOfWeek.Sunday: return candidate;
				default:
					throw new ArgumentOutOfRangeException("Unkown weekday ! ?");
			}			
		}
	}
}
