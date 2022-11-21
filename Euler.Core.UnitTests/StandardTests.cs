using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GInt = Euler.Core.GaussianInteger;
using PyT = Euler.Core.PythagoricianTriplet;

namespace Euler.Core.UnitTests
{
	[TestFixture]
	public class StandardTests
	{
		[Test]
		public void Sum_Of_Multiples()
		{
			var test = Utilities.CreateNumberPool(new List<long> { 3, 5 }, 10).Sum();

			Assert.AreEqual(23, test);
		}

		[Test]
		[TestCase(342, 23)]
		[TestCase(115, 20)]
		[TestCase(1, 3)]
		[TestCase(1000, 11)]
		public void Letters_Used(int candidate, int lenght)
		{
			var test = NumberWriter.FindLength(candidate);

			Assert.AreEqual(lenght, test);
		}

		[Test]
		public void Sum_Of_Fibos()
		{
			var test = Utilities.CreateEvenFibonaccis(100).Sum();

			Assert.AreEqual(44, test);
		}

		[Test]
		public void Sum_Of_Int()
		{
			GInt a = 4;
			GInt b = 8;

			var test = a + b;

			Assert.AreEqual(12, test.A);
			Assert.AreEqual(0, test.B);
		}

		[Test]
		public void Product_Of_Int()
		{
			GInt a = 4;
			GInt b = 8;

			var test = a * b;

			Assert.AreEqual(32, test.A);
			Assert.AreEqual(0, test.B);
		}
		[Test]
		public void Substraction_Of_Int()
		{
			GInt a = 4;
			GInt b = 8;

			var test = a - b;

			Assert.AreEqual(-4, test.A);
			Assert.AreEqual(0, test.B);
		}
		[Test]
		public void Division_Of_Int()
		{
			GInt a = 4;
			GInt b = 8;

			var test = b / a;

			Assert.AreEqual(2, test.A);
			Assert.AreEqual(0, test.B);
		}
		[Test]
		public void Division_Of_Int_Crash()
		{
			GInt a = 6;
			GInt b = 8;

			Assert.Throws<InvalidOperationException>(() => { var plouf = b / a; });
		}
		[Test]
		public void Division_Of_GInt()
		{
			GInt a = 2;
			GInt b = GInt.N(1, 1);

			var test = a / b;

			Assert.AreEqual(1, test.A);
			Assert.AreEqual(-1, test.B);
		}
		[Test]
		public void Conjuge_Of_GInt()
		{
			var a = GInt.N(1, 1);
			var b = !a;

			var test = a * b;

			Assert.AreEqual(2, test.A);
			Assert.AreEqual(0, test.B);
		}
		[Test]
		public void SquareModule_Of_GInt()
		{
			var a = GInt.N(1, 1);

			var test = a.SquareModule;

			Assert.AreEqual(2, test);
		}
		[Test]
		public void Square_Of_Units()
		{
			Assert.AreEqual(GInt.One, GInt.One * GInt.One);
			Assert.AreEqual(GInt.One, GInt.MinusOne * GInt.MinusOne);
			Assert.AreEqual(GInt.MinusOne, GInt.I * GInt.I);
			Assert.AreEqual(GInt.MinusOne, GInt.MinusI * GInt.MinusI);

			Assert.AreEqual(GInt.One, GInt.MinusI * GInt.I);
			Assert.AreEqual(GInt.I, GInt.MinusI * GInt.MinusOne);
			Assert.AreEqual(GInt.I, GInt.One * GInt.I);
			Assert.AreEqual(GInt.MinusI, GInt.MinusI * GInt.One);
		}

		[Test, Ignore("Too long")]
		public void Square_Course_Over_GInt()
		{
			int max = 10000; // timeout au delà de 10^4
			int limit = max * max;

			int count = 0;

			for (int i = -max; i < max; i++)
			{
				for (int j = -max; j < max; j++)
				{
					var gp = GInt.N(i, j);

					if (gp.SquareModule < limit)
						count++;
				}
			}

			Assert.AreEqual(314159017, count);
		}


		/*************************/
		[Test]
		[TestCase(234, 10)]
		[TestCase(2, 10)]
		[TestCase(2, 2)]
		[TestCase(5, 10)]
		[TestCase(1000, 10)]
		[TestCase(1000000, 10)]
		[TestCase(999, 10)]
		[TestCase(234, 5)]
		public void ComposerTest(int cobaye, int numericalBase)
		{
			var decompo = Decomposition.Create(cobaye, numericalBase);

			var guineaPig = Decomposition.Recompose(decompo.Digits, numericalBase);

			Assert.AreEqual(guineaPig, cobaye);
		}

		[Test]
		[TestCase(99, 10)]
		[TestCase(234, 10)]
		public void ComposerTestWithMinimum(int cobaye, int numericalBase)
		{
			var decompo = Decomposition.Decompose(cobaye, numericalBase, 4);
			Assert.AreEqual(decompo.Count, 4);

			var guineaPig = Decomposition.Recompose(decompo, numericalBase);
			Assert.AreEqual(guineaPig, cobaye);
		}

		[Test]
		[TestCase(3797, true)]
		[TestCase(432, false)]
		[TestCase(3, false)]
		[TestCase(19, false)]
		public void TruncatableTest(int cobaye, bool solution)
		{
			var primeSource = new PrimalityProvider(10000);

			var answer = Truncatables.IsTruncatable(cobaye, primeSource);

			Assert.AreEqual(answer, solution);
		}

		[Test]
		[TestCase(2, 2, 4)]
		[TestCase(2, 2, 4)]
		[TestCase(8, 2, 64)]
		[TestCase(4, 3, 64)]
		public void DecompositionTest(int a, int b, int other)
		{
			var primeSource = new PrimalityProvider(100);

			var toTest = primeSource.Decompose(a, b);
			var expected = primeSource.Decompose(other).ToPrimeDecomposition();

			Assert.AreEqual(toTest, expected);
		}

		[Test]
		public void DecompositionHashSet()
		{
			var primeSource = new PrimalityProvider(100);

			var cumulator = new HashSet<PrimeDecomposition>();

			cumulator.Add(primeSource.Decompose(2, 18));
			cumulator.Add(primeSource.Decompose(4, 9));
			cumulator.Add(primeSource.Decompose(64, 3));

			Assert.AreEqual(1, cumulator.Count);
		}

		[Test]
		public void Concatenated_Pandigital()
		{
			var seed = 9;
			var maxOp = 5;

			var answer = ConcatenatedProduct.Compute(seed, maxOp);

			Assert.AreEqual(true, answer.IsPandigital());
		}

		[Test]
		public void Triangle_Solutions()
		{
			var answer = RightTriangle.FindSolutionsSets(120);

			Assert.AreEqual(3, answer);
		}

		[Test]
		public void Simple_Product()
		{
			int answer = 0;

			answer = DigitReader.ComputeProduct(new List<short> { 1, 2 });
			Assert.AreEqual(2, answer);

			answer = DigitReader.ComputeProduct(new List<short> { 2 });
			Assert.AreEqual(2, answer);

			answer = DigitReader.ComputeProduct(new List<short> { 2, 3, 5, 8 });
			Assert.AreEqual(240, answer);
		}

		[Test]
		public void Digit_Index_Finder()
		{
			var test = DigitReader.ChampowneExtract(12);
			Assert.AreEqual(1, test[0]);
		}

		[Test]
		public void Pandigits_Permutations()
		{
			var test = Permutations.BuildIndexPermutations(9).ToList();
			Assert.AreEqual(362880, test.Count);
		}

		[Test]
		public void Pandigits_Permutations_Values()
		{
			var test = Permutations.BuildIndexPermutations(7).ToList();
			Assert.AreEqual(5040, test.Count);
		}

		[Test]
		public void Words_Values()
		{
			var test = Words.WeightWord("SKY");
			Assert.AreEqual(55, test);
		}

		[Test]
		public void Pandigital_AdvancedDivisibility()
		{
			var test = Pandigitals.SatisfyAdvancedDivisibility(Decomposition.Decompose(1406357289, 10));
			Assert.AreEqual(true, test);
		}

		[Test]
		public void Primary_Decomposition_2()
		{
			var source = new PrimalityProvider(100000);
			var toTest = source.Decompose(4);

			Assert.AreEqual(2, toTest[2]);
		}

		[Test]
		public void Primary_Decomposition_42()
		{
			var source = new PrimalityProvider(1000);
			var toTest = source.Decompose(42);

			Assert.AreEqual(1, toTest[2]);
			Assert.AreEqual(1, toTest[3]);
			Assert.AreEqual(1, toTest[7]);
		}

		[Test]
		public void Primary_Decomposition_86437218()
		{
			var source = new PrimalityProvider(100000);

			long cobaye = 86437218;
			var toTest = source.Decompose(cobaye);

			Assert.AreEqual(1, toTest[2]);
			Assert.AreEqual(1, toTest[3]);
			Assert.AreEqual(1, toTest[7]);
			Assert.AreEqual(1, toTest[79]);
			Assert.AreEqual(1, toTest[109]);
			Assert.AreEqual(1, toTest[239]);

			var reversed = source.Recompose(toTest);
			Assert.AreEqual(cobaye, reversed);
		}

		[Test]
		public void Primary_Decomposition_87109()
		{
			var source = new PrimalityProvider(100000);

			long cobaye = 87109;
			var toTest = source.Decompose(cobaye);

			Assert.AreEqual(1, toTest[11]);
			Assert.AreEqual(1, toTest[7919]);

			var reversed = source.Recompose(toTest);
			Assert.AreEqual(cobaye, reversed);
		}

		[Test]
		public void Power_Modulo()
		{
			long control = (long) Math.Pow(10, 10);

			long tester = 999;

			for (int i = 0; i < 1000; i++)
			{
				tester *= 1000;
				tester = tester % control;
			}

			Assert.AreEqual(0, tester);
		}

		[Test]
		public void Rotation_Condition()
		{
			var test = PrimeGames.CheckRotationCondition(1487, 1487 + 3330, 1487 + 6660);

			Assert.IsTrue(test);
		}

		[Test]
		public void Searching_Decomposition_Basics()
		{
			var toTest = Decomposition.SearchingDecomposition(145214528, 10);

			Assert.AreEqual(2, toTest[1]);
			Assert.AreEqual(1, toTest[8]);
			Assert.AreEqual(false, toTest.ContainsKey(3));
		}

		[Test]
		[TestCase(1, 1, 1, new int[] { 0 })]
		[TestCase(2, 2, 1, new int[] { 0, 1 })]
		[TestCase(10, 10, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
		[TestCase(1, 10, 10, new int[] { 9 })]
		[TestCase(1, 5, 5, new int[] { 4 })]
		[TestCase(2, 5, 10, new int[] { 3, 4 })]
		[TestCase(2, 6, 15, new int[] { 4, 5 })]
		[TestCase(5, 9, 126, new int[] { 0, 1, 2, 3, 4 })]
		public void Choices_Basics(int k, int n, int count, int[] example)
		{
			var toTest = Permutations.BuildIndexChoices(n, k).ToList();

			Assert.AreEqual(count, toTest.Count);

			Assert.IsTrue(toTest.Any(y => y.Count == example.Length));
			Assert.IsTrue(toTest.Any(y => {
				bool assertion = true;
				foreach (var item in example)
					assertion &= y.Contains(item);

				return assertion;
			}));
		}

		[TestCase(2, 5, 10)]
		[TestCase(2, 6, 15)]
		public void Choices_Computation_Basics(int k, int n, int count)
		{
			var computer = new Binomials { Provider = new PrimalityProvider(1000) };

			var factors = computer.ComputeCnkFactors(n, k);

			var result = computer.Provider.Recompose(factors);

			Assert.AreEqual(count, result);
		}

		[Test]
		[TestCase(1245)]
		[TestCase(759846120126)]
		[TestCase(253400014)]
		public void Decompose_BigInt(long input)
		{
			var bigInt = new BigInteger(input);

			var result = Decomposition.Decompose(bigInt);
			var startingPoint = Decomposition.Recompose(result);

			Assert.AreEqual(input, (long) startingPoint);
		}

		[Test]
		[TestCase(4994, true)]
		[TestCase(196, true)]
		[TestCase(1947, true)]
		[TestCase(592, true)]
		[TestCase(47, false)]
		[TestCase(349, false)]
		[TestCase(10677, true)]
		public void Is_Lychrel(long candidate, bool result)
		{
			var test = Palindroms.IsLychrel(candidate);

			Assert.AreEqual(result, test);
		}

		[Test]
		public void Is_Xored_Properly()
		{
			var test = Decryptor.XorChars('A', '*');

			Assert.AreEqual('k', test);
		}

		[Test]
		[TestCase(5)]
		[TestCase(22)]
		[TestCase(35)]
		public void Is_Pentagon(long candidate)
		{
			var test = GeometricNumbersProvider.IsPentagon(candidate);

			Assert.IsTrue(test);
		}

		[Test]
		[TestCase(8)]
		[TestCase(21)]
		[TestCase(65)]
		public void Is_Octogon(long candidate)
		{
			var test = GeometricNumbersProvider.IsOctogon(candidate);

			Assert.IsTrue(test);
		}

		[Test]
		[TestCase(7)]
		[TestCase(34)]
		[TestCase(55)]
		public void Is_Heptagon(long candidate)
		{
			var test = GeometricNumbersProvider.IsHeptagon(candidate);

			Assert.IsTrue(test);
		}

		[Test]
		public void Geometric_Magnitude_Test()
		{
			var test = new GeometricNumbersProvider(150);

			Assert.IsTrue(test.Triangles.Max() > 10000);
		}

		[Test]
		public void Continuous_Fractions_Atom()
		{
			var example = new ContinuousFractionsFactory.RootAtom { Denominator = 1, NumeratorInteger = 0, SquareCandidate = 13 };

			var test = example.Reduce();

			Assert.AreEqual(test.Item1, 3);
			Assert.AreEqual(3, test.Item2.NumeratorInteger);
			Assert.AreEqual(4, test.Item2.Denominator);
			Assert.AreEqual(13, test.Item2.SquareCandidate);

			test = test.Item2.Reduce();

			Assert.AreEqual(test.Item1, 1);
			Assert.AreEqual(1, test.Item2.NumeratorInteger);
			Assert.AreEqual(3, test.Item2.Denominator);

			test = test.Item2.Reduce();

			Assert.AreEqual(test.Item1, 1);
			Assert.AreEqual(2, test.Item2.NumeratorInteger);
			Assert.AreEqual(3, test.Item2.Denominator);

			test = test.Item2.Reduce();

			Assert.AreEqual(test.Item1, 1);
			Assert.AreEqual(1, test.Item2.NumeratorInteger);
			Assert.AreEqual(4, test.Item2.Denominator);

			test = test.Item2.Reduce();

			Assert.AreEqual(test.Item1, 1);
			Assert.AreEqual(3, test.Item2.NumeratorInteger);
			Assert.AreEqual(1, test.Item2.Denominator);

			test = test.Item2.Reduce();

			Assert.AreEqual(test.Item1, 6);
			Assert.AreEqual(3, test.Item2.NumeratorInteger);
			Assert.AreEqual(4, test.Item2.Denominator);
		}

		[Test]
		[TestCase(245, 1595, false)]
		[TestCase(245, 245, false)]
		[TestCase(245, 3, true)]
		[TestCase(24, 1595324, false)]
		[TestCase(245, 35, false)]
		public void Are_Primes_Test(int a, int b, bool result)
		{
			var testSource = new PrimalityProvider(10000);

			Assert.AreEqual(result, testSource.ArePrimes(a, b));
		}

		[Test]
		[TestCase(2, 1)]
		[TestCase(3, 2)]
		[TestCase(4, 0)]
		[TestCase(5, 1)]
		[TestCase(6, 2)]
		[TestCase(7, 4)]
		[TestCase(8, 2)]
		[TestCase(10, 1)]
		[TestCase(11, 2)]
		[TestCase(12, 2)]
		[TestCase(13, 5)]
		public void Continuous_Fractions(int square, int frequency)
		{
			var test = ContinuousFractionsFactory.AdvancedRootCreate(square);
			Assert.IsTrue(test.Frequency == frequency);
		}

		[Test]
		public void Read_Continuous_Fractions()
		{
			var test0 = ContinuousFractionHandler.ReadCoefficients(new List<long> { 1, 2 });
			Assert.AreEqual(3, (long) test0.Item1);
			Assert.AreEqual(2, (long) test0.Item2);

			var test2 = ContinuousFractionHandler.ReadCoefficients(new List<long> { 1, 2, 2, 2 });
			Assert.AreEqual(17, (long) test2.Item1);
			Assert.AreEqual(12, (long) test2.Item2);

			var test3 = ContinuousFractionHandler.ReadCoefficients(new List<long> { 1, 2, 2, 2, 2 });
			Assert.AreEqual(41, (long) test3.Item1);
			Assert.AreEqual(29, (long) test3.Item2);
		}

		[Test]
		public void Test_Phi_Computation()
		{
			var ploum = new PhiComputer { Source = new PrimalityProvider(1000) };

			Assert.IsNotNull(ploum);

			ploum.PopulatePhiValues();

			Assert.AreEqual(6, ploum.PhiValues[7]);
			Assert.AreEqual(18, ploum.PhiValues[54]);
			Assert.AreEqual(24, ploum.PhiValues[72]);
			Assert.AreEqual(64, ploum.PhiValues[85]);
			Assert.AreEqual(32, ploum.PhiValues[96]);
		}

		[Test]
		public void Find_Diophantine_Solution()
		{
			var runner = new Diophantine(2);
			var minimalSolution = runner.FindMinimumSolution();
			Assert.AreEqual((BigInteger) 3, minimalSolution.Item1);
			Assert.AreEqual((BigInteger) 2, minimalSolution.Item2);

			runner = new Diophantine(13);
			minimalSolution = runner.FindMinimumSolution();
			Assert.AreEqual((BigInteger) 649, minimalSolution.Item1);
			Assert.AreEqual((BigInteger) 180, minimalSolution.Item2);

			runner = new Diophantine(61);
			minimalSolution = runner.FindMinimumSolution();
			Assert.AreEqual((BigInteger) 1766319049, minimalSolution.Item1);

			runner = new Diophantine(83);
			minimalSolution = runner.FindMinimumSolution();
			Assert.AreEqual((BigInteger) 82, minimalSolution.Item1);
			Assert.AreEqual((BigInteger) 9, minimalSolution.Item2);

			runner = new Diophantine(103);
			minimalSolution = runner.FindMinimumSolution();
			Assert.AreEqual((BigInteger) 227528, minimalSolution.Item1);
			Assert.AreEqual((BigInteger) 22419, minimalSolution.Item2);

			runner = new Diophantine(313);
			minimalSolution = runner.FindMinimumSolution();
			Assert.AreEqual((BigInteger) 32188120829134849, minimalSolution.Item1);
			Assert.AreEqual((BigInteger) 1819380158564160, minimalSolution.Item2);

			runner = new Diophantine(661);
			minimalSolution = runner.FindMinimumSolution();
			Assert.AreEqual(BigInteger.Parse("16421658242965910275055840472270471049"), minimalSolution.Item1);
			Assert.AreEqual(BigInteger.Parse("638728478116949861246791167518480580"), minimalSolution.Item2);
		}

		[Test]
		public void Validate_N_Gon_Ring()
		{
			NGonRing validator;

			var test = NGonRing.TryBuild(403251, out validator);

			Assert.IsTrue(test);
			Assert.IsTrue(validator.IsMagic());
			Assert.AreEqual(432621513, validator.PublishDigit());

			test = NGonRing.TryBuild(9280736154, out validator);

			Assert.IsTrue(test);
			Assert.IsTrue(validator.IsMagic());
		}

		[Test]
		public void Special_NGong_Alternator()
		{
			var toTest = new List<short> { 2, 4, 1, 3, 0, 5, 6, 7, 8, 9 };

			var check = NGonRing.Alternate(toTest, 4);

			var cobaye = new List<short> { 9, 2, 5, 4, 6, 1, 7, 3, 8, 0 };

			CollectionAssert.AreEqual(cobaye, check);
		}

		[Test]
		public void Advanced_Validator_5_Gon_Ring()
		{
			NGonRing validator;

			var test = NGonRing.AdvancedTryBuild(new List<int> { 2, 0, 3, 1, 4 }, 14, out validator);

			Assert.IsTrue(test);
			Assert.IsTrue(validator.IsMagic());
			Assert.AreEqual(6531031914842725, validator.PublishDigit());
		}

		[Test]
		public void Factorial_Chain_Computing()
		{
			var chainer = new FactorialChainer(10);

			var toTest = chainer.ComputeChainLength(145);
			Assert.AreEqual(1, toTest);

			toTest = chainer.ComputeChainLength(169);
			Assert.AreEqual(3, toTest);

			toTest = chainer.ComputeChainLength(78);
			Assert.AreEqual(4, toTest);

			toTest = chainer.ComputeChainLength(69);
			Assert.AreEqual(5, toTest);

			toTest = chainer.ComputeChainLength(540);
			Assert.AreEqual(2, toTest);
		}

		[Test]
		[TestCase(2, 4, 16)]
		[TestCase(5, 3, 125)]
		[TestCase(2, 10, 1024)]
		[TestCase(2, 16, 65536)]
		public void Array_Digit_Multiplier(short number, int exponent, long checkValue)
		{
			var realNumber = LargeNumberHandler.ComputeExponent(number, exponent);

			Assert.AreEqual(checkValue, realNumber);
		}

		[Test]
		// 1234 * 111 = 136974
		// 7789 * 111111 = 865443579
		public void Sum_Of_Lines_Test()
		{
			var line1 = new List<short> { 4, 3, 2, 1 };
			var line2 = new List<short> { 0, 4, 3, 2, 1 };
			var line3 = new List<short> { 0, 0, 4, 3, 2, 1 };

			var toTest = LargeNumberHandler.ProperSum(new List<List<short>> { line1, line2, line3 });

			var result = new List<short> { 4, 7, 9, 6, 3, 1 };

			CollectionAssert.AreEqual(result, toTest);


			line1 = new List<short> { 9, 8, 7, 7 };
			line2 = new List<short> { 0, 9, 8, 7, 7 };
			line3 = new List<short> { 0, 0, 9, 8, 7, 7 };
			var line4 = new List<short> { 0, 0, 0, 9, 8, 7, 7 };
			var line5 = new List<short> { 0, 0, 0, 0, 9, 8, 7, 7 };
			var line6 = new List<short> { 0, 0, 0, 0, 0, 9, 8, 7, 7 };

			toTest = LargeNumberHandler.ProperSum(new List<List<short>> { line1, line2, line3, line4, line5, line6 });

			result = new List<short> { 9, 7, 5, 3, 4, 4, 5, 6, 8 };

			CollectionAssert.AreEqual(result, toTest);
		}

		[Test]
		// 1234 * 111 = 136974
		// 7789 * 111111 = 865443579
		// 7789 * 898989 = 7002225321
		public void Multiply_Integer_Test()
		{
			var lineA = new List<short> { 4, 3, 2, 1 };
			var toTest = LargeNumberHandler.MultiplyIntegers(lineA, 111);
			var result = new List<short> { 4, 7, 9, 6, 3, 1 };
			CollectionAssert.AreEqual(result, toTest);

			var lineB = new List<short> { 9, 8, 7, 7 };
			toTest = LargeNumberHandler.MultiplyIntegers(lineB, 111111);
			result = new List<short> { 9, 7, 5, 3, 4, 4, 5, 6, 8 };
			CollectionAssert.AreEqual(result, toTest);

			var lineC = new List<short> { 9, 8, 7, 7 };
			toTest = LargeNumberHandler.MultiplyIntegers(lineC, 898989);
			result = new List<short> { 1, 2, 3, 5, 2, 2, 2, 0, 0, 7 };
			CollectionAssert.AreEqual(result, toTest);
		}

		[Test]
		public void Large_Factorial_Test()
		{
			var toTest = LargeNumberHandler.ComputeLargeFactorial(6);
			var result = new List<short> { 0, 2, 7 };
			CollectionAssert.AreEqual(result, toTest);

			toTest = LargeNumberHandler.ComputeLargeFactorial(11);
			result = new List<short> { 0, 0, 8, 6, 1, 9, 9, 3 };
			CollectionAssert.AreEqual(result, toTest);

			toTest = LargeNumberHandler.ComputeLargeFactorial(200);
			Assert.AreEqual(375, toTest.Count);
		}

		[Test]
		public void Test_Amicability()
		{
			var ploum = new Amicability { Engine = new PrimalityProvider(1000) };

			long binom = 0;

			var toTest = ploum.TryAmicable(284, out binom);

			Assert.IsTrue(toTest);
			Assert.AreEqual(220, binom);
		}

		[Test]
		public void Pythagorician_Triplets_R1()
		{
			var toTest = PyT._Source;
			var result = PyT._R1 * toTest;
			var pytho = new PyT(result);
			var pythoCount = 0;

			Assert.IsTrue(pytho.IsValid());

			while (pytho.Norm < 1500000)
			{
				result = PyT._R1 * result;
				// result = Matrix<int>.Multiplication(PythagoricianTriplet._R1, result, (x, y) => x + y, (x, y) => x * y) as MatrixN;
				pytho = new PyT(result);
				//Assert.IsTrue(pyT.IsValid());

				pythoCount++;
			}

			Assert.AreEqual(1225, pytho.X);
			Assert.AreEqual(750312, pytho.Y);
			Assert.AreEqual(750313, pytho.Z);

			Assert.AreEqual(610, pythoCount);
		}

		[Test]
		public void Pythagorician_Triplets_R2()
		{
			var toTest = PyT._Source;
			var result = PyT._R1 * toTest;
			var pytho = new PyT(result);
			var pythoCount = 0;

			Assert.IsTrue(pytho.IsValid());

			while (pytho.Norm < 1500000)
			{
				result = PyT._R2 * result;
				pytho = new PyT(result);

				pythoCount++;
			}

			Assert.AreEqual(2021235, pytho.X);
			Assert.AreEqual(2021228, pytho.Y);
			Assert.AreEqual(2858453, pytho.Z);

			Assert.AreEqual(7, pythoCount);
		}

		[Test]
		public void Pythagorician_Triplets_R3()
		{
			var toTest = PyT._Source;
			var result = PyT._R3 * toTest;
			var pytho = new PyT(result);
			var pythoCount = 0;

			Assert.IsTrue(pytho.IsValid());

			while (pytho.Norm < 1500000)
			{
				result = PyT._R3 * result;
				pytho = new PyT(result);

				pythoCount++;
			}

			Assert.AreEqual(749955, pytho.X);
			Assert.AreEqual(1732, pytho.Y);
			Assert.AreEqual(749957, pytho.Z);

			Assert.AreEqual(431, pythoCount);
		}

		[Test]
		public void Primitive_Triplets()
		{
			var triplets = PyT.PrimitiveTripletUnder(100).ToList();
			Assert.AreEqual(7, triplets.Count);

			triplets = PyT.PrimitiveTripletUnder(1000).ToList();
			Assert.AreEqual(70, triplets.Count);
		}

		[Test]
		public void Full_Triplets()
		{
			var resultList = PyT.EnrichPrimitiveList(1000);

			Assert.AreEqual(325, resultList.Count);
		}

		[Test]
		[TestCase(5, 6)]
		[TestCase(10, 41)]
		[TestCase(20, 626)]
		[TestCase(50, 204225)]
		[TestCase(100, 190569291)]
		public void Write_Number_As_Sum(int input, int count)
		{
			var result = NumberWriter.HowToWrite(input);

			Assert.AreEqual(count, result);
		}

		[Test]
		[TestCase(10, 5)]
		[TestCase(20, 26)]
		[TestCase(30, 98)]
		[TestCase(50, 819)]
		[TestCase(71, 5007)]
		[TestCase(100, 40899)]
		public void Write_Number_As_Sum_With_Primes(int input, int count)
		{
			var provider = new PrimalityProvider(1000);

			var adder = new RecursiveAdder(provider);

			var result = adder.HowToWriteWithPrimes(input);

			Assert.AreEqual(count, result);
		}
	}
}