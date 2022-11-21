using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Euler.Algebra.UnitTests
{
	[TestClass]
	public class MatrixTest
	{
		[TestMethod]
		public void Matrix_Product_Test()
		{
			var A = new MatrixN(new int[,] { { 1, 0 }, { 0, 1 } });
			var B = new MatrixN(new int[,] { { 1, 1 }, { 1, 1 } });
			var C = new MatrixN(new int[,] { { 2, 2 }, { 2, 2 } });

			Assert.IsTrue((A * A) == A);
			Assert.IsTrue((A * B) == B);
			Assert.IsTrue((B * B) == C);
		}
	}
}
