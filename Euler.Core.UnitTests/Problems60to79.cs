using NUnit.Framework;
using System;

namespace Euler.Core.UnitTests
{
    [TestFixture]
    partial class Problems
	{
		[Test]
		public void _067_Max_Path_Sum_II()
		{
			var engine = new TriangleHelper();
			var toTest = engine.FindMaxPath(TriangleType.Big);

			Assert.AreEqual(7273, toTest);
		}

		[Test]
        public void _071_Find_3Over7_Neighbor()
        {
            var toTest = PrimeGames.Find37RightNeighborNumerator(1000000);

            Assert.AreEqual(428570, toTest);
        }

        [Test]
        public void _072_Find_Reduced_Count()
        {
            var toTest = PrimeGames.FindReducedCount(1000000);

            Assert.AreEqual(303963552391, toTest);
        }

        [Test, Ignore("Too long")]
        public void _073_Find_Reduced_Count_With_Boundaries()
        {
            var toTest = PrimeGames.FindFixedCount(12000, new Tuple<int, int>(1, 3), new Tuple<int, int>(1, 2));

            Assert.AreEqual(7295372, toTest);
        }

        [Test]
        public void _074_Find_Factorial_Cycle_Sixty()
        {
            var toTest = DigitReader.CountFactorialCycles(1000000, 60);

            Assert.AreEqual(402, toTest);
		}

		[Test]
		public void _075_Find_Single_Right_Triangles()
		{
			var resultList = PythagoricianTriplet.RefineTripletList(1500000);

			Assert.AreEqual(161667, resultList.Count);
		}

		[Test]
		public void _076_Write_Number_As_Sum()
		{
			var result = NumberWriter.HowToWrite(100);

			Assert.AreEqual(190569291, result);
		}


		[Test]
		public void _077_Write_Number_As_Sum_Of_Primes()
		{
			var result = NumberWriter.WriteWithPrimesAbove(5000);

			Assert.AreEqual(71, result);
		}
	}
}
