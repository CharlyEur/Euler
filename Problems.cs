using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Core.UnitTests
{
    [TestFixture]
    class Problems
    {
        [Test]
        public void Find_3Over7_Neighbor_71()
        {
            var toTest = PrimeGames.Find37RightNeighborNumerator(1000000);

            Assert.AreEqual(428570, toTest);
        }

        [Test]
        public void Find_Reduced_Count_72()
        {
            var toTest = PrimeGames.FindReducedCount(1000000);

            Assert.AreEqual(303963552391, toTest);
        }

        [Test, Ignore("Too long")]
        public void Find_Reduced_Count_With_Boundaries_73()
        {
            var toTest = PrimeGames.FindFixedCount(12000, new Tuple<int, int>(1, 3), new Tuple<int, int>(1, 2));

            Assert.AreEqual(7295372, toTest);
        }

        [Test]//, Ignore("Too long")]
        public void Find_Factorial_Cycle_Sixty_74()
        {
            var toTest = DigitReader.FindFactorialCycles( 100, 5 );

            Assert.AreEqual(7295372, toTest);
        }
    }
}
