using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core.UnitTests
{
    [TestFixture]
    partial class Problems
    {
        [Test]
        public void _001_Sum_Multiple_3_and_5_Under_1000()
        {
            var test = Utilities.CreateNumberPool(new List<long> { 3, 5 }, 1000).Sum();

            Assert.AreEqual(233168, test);
        }

        [Test]
        public void _002_Even_Fibonacci_Under_4_Millions()
        {
            var toTest = Utilities.CreateEvenFibonaccis(4000000).Sum();

            Assert.AreEqual(4613732, toTest);
        }

        [Test]
        public void _003_Biggest_Prime_Factor()
        {
            var toTest = PrimeGames.FindPrimeFactors(600851475143).Max();

            Assert.AreEqual(6857, toTest);
        }
        
        [Test]
        public void _004_Largest_Palindrom_Product_3_Digits()
        {
            var toTest = Palindroms.FindMax(1000).Max();

            Assert.AreEqual(906609, toTest);
        }

        [Test]
        public void _005_Smallest_Multiple_1_to_20()
        {
            var toTest = 232792560; // 2^4 * 3^2 * 5 * 7 * 11 * 13 * 17 * 19 calculé à la main

            Assert.AreEqual(232792560, toTest);
        }

        [Test]
        public void _006_Sum_Square_Difference()
        {
            var toTest = Utilities.ComputeSumSquareDiff(100);

            Assert.AreEqual(25164150, toTest);
        }

        [Test]
        public void _007_Prime_number_10001()
        {
            var toTest = PrimeGames.FindNthPrime(10001);

            Assert.AreEqual(104743, toTest);
        }

        [Test]
        public void _008_Largest_Product_in_a_Serie()
        {
            var toTest = SpecialLongNumber.FindConsecutiveProduct(13);

            Assert.AreEqual(23514624000, toTest);
        }

        [Test]
        public void _009_Special_Pythagorean_Triplet()
        {
            var toTest = Utilities.ResolvePythagoreanTriplet(1000);

            Assert.AreEqual(31875000, toTest);
        }

        [Test]
        public void _010_Prime_Below_2_Millions()
        {
            var toTest = PrimeGames.SumPrimesUnder(2000000);

            Assert.AreEqual(142913828922, toTest);
        }

        [Test]
        public void _011_Max_Product_In_A_Grid()
        {
            var toTest = NumberGridder.FindMaxProduct();

            Assert.AreEqual(70600674, toTest);
        }

        [Test]
        public void _012_Highly_Divisible_Triangle()
        {
            var toTest = GeometricNumbersProvider.FindHighlyDivisable(GeometricForm.Triangle, 500);

            Assert.AreEqual(76576500, toTest);
        }

        [Test]
        public void _013_Monstruous_Sum()
        {
            var toTest = BigNumbersReader.SmartReadSum();

            Assert.AreEqual("5537376230", toTest);
        }

        [Test]
        public void _014_Long_Collatz_Sum()
        {
            var toTest = CollatzHandler.FindLongestChain(1000000);

            Assert.AreEqual(837799, toTest);
        }
        
        /// <summary>
        /// No specific code, as you just have to notice a path is a choice
        /// of 20 places among 40 (you must go n times right and n times down).
        /// </summary>
        [Test]
        public void _015_Lattice_Path()
        {
            var engine = new Binomials { Provider = new PrimalityProvider(1000000) };

            var toTest = engine.ComputeCnk(40, 20);

            Assert.AreEqual(137846528820, toTest);
        }

        [Test]
        public void _016_Power_Digit_Sum()
        {
            var toTest = Utilities.ComputeDigitSum(2, 1000);

            Assert.AreEqual(1366, toTest);
		}

		[Test]
		public void _017_Number_Letter_Count()
		{
			var toTest = NumberWriter.ComputeLetterCount(1000);

			Assert.AreEqual(21124, toTest);
		}

		[Test]
		public void _018_Maximum_Path_Sum_I()
		{
			var engine = new TriangleHelper();
			var toTest = engine.FindMaxPath(TriangleType.Small);

			Assert.AreEqual(1074, toTest);
		}

		[Test]
		public void _019_Counting_Sundays()
		{
			var toTest = DayHandler.CountFirstSundays(new DateTime(1901, 1, 1), new DateTime(2000, 12, 31));

			Assert.AreEqual(171, toTest);
		}
	}
}
