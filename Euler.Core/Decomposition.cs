
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler.Core
{
    public class Decomposition : IEquatable<Decomposition>
    {
        public List<short> Digits { get; set; }

        public int Base { get; private set; }

        public long StandardValue { get; private set; }

        private static SortedDictionary<char, short> _digits;

        static Decomposition()
        {
            _digits = new SortedDictionary<char, short>();

            _digits.Add('0', 0);
            _digits.Add('1', 1);
            _digits.Add('2', 2);
            _digits.Add('3', 3);
            _digits.Add('4', 4);
            _digits.Add('5', 5);
            _digits.Add('6', 6);
            _digits.Add('7', 7);
            _digits.Add('8', 8);
            _digits.Add('9', 9);
            _digits.Add('A', 10);
            _digits.Add('B', 11);
            _digits.Add('C', 12);
            _digits.Add('D', 13);
            _digits.Add('E', 14);
            _digits.Add('F', 15);
        }

        private Decomposition() { }

        public static Decomposition Create(long stdWriting, int numericalBase)
        {
            var digits = Decompose(stdWriting, numericalBase);

            return new Decomposition { Base = numericalBase, Digits = digits, StandardValue = stdWriting };
        }

        public static Decomposition Create(List<short> digits, int numericalBase)
        {
            var stdValue = Recompose(digits, numericalBase);

            return new Decomposition { Base = numericalBase, Digits = digits, StandardValue = stdValue };
        }

        internal static long Recompose(List<short> digits, int numericalBase)
        {
            long buffer = 0;

            var count = digits.Count;

            for (int i = 0; i < count; i++)
                buffer += (long) (digits[count - i - 1] * Math.Pow(numericalBase, i));

            return buffer;
        }

        internal static List<short> Decompose(long candidate, int numericalBase)
        {
            var digits = new List<short>();

            if (candidate < numericalBase)
            {
                digits.Add((short) candidate);
                return digits;
            }

            var powerDouble = Math.Log(candidate) / Math.Log(numericalBase);
            var power = (int) Math.Floor(powerDouble);

            var test = (long) Math.Pow(numericalBase, power);
            if (candidate > test)
                power++;

            var buffer = candidate;

            for (var i = power; i >= 0; i--)
            {
                var curtPower = (long) Math.Pow(numericalBase, i);
                var digit = buffer / curtPower;
                digits.Add((short) digit);

                buffer -= digit * curtPower;
            }

            while (digits[0] == 0)
                digits.RemoveAt(0);

            return digits;
        }

        internal static List<short> Decompose(long candidate, int numericalBase, int digitCount)
        {
            var result = Decompose(candidate, numericalBase);

            while (result.Count < digitCount)
                result.Insert(0, 0);

            return result;
        }

        internal static BigInteger Recompose(List<short> toRecompose)
        {
            var value = new BigInteger();

            for (var i = 0; i < toRecompose.Count; i++)
                value += toRecompose[i] * BigInteger.Pow(10, toRecompose.Count - i - 1);

            return value;
        }

        internal static List<short> DecomposeRaw(BigInteger value)
        {
            var digits = new List<short>();

            if (value < 10)
            {
                digits.Add((short) value);
                return digits;
            }

            var power = (int) Math.Floor(BigInteger.Log10(value));
            BigInteger buffer = value;

            for (int i = power; i >= 0; i--)
            {
                var curtPower = BigInteger.Pow(10, i);
                var digit = (short) BigInteger.Divide(buffer, curtPower);
                digits.Add(digit);

                buffer = BigInteger.Subtract(buffer, BigInteger.Multiply(digit, curtPower));
            }

            return digits;
        }

        internal static List<short> Decompose(int value)
        {
            var digits = new List<short>();

            if (value < 10)
            {
                digits.Add((short) value);
                return digits;
            }

            var readNumber = value.ToString();

            return readNumber.Select(x => _digits[x]).ToList();
        }

        internal static List<short> Decompose(BigInteger value)
        {
            var digits = new List<short>();

            if (value < 10)
            {
                digits.Add((short) value);
                return digits;
            }

            var readNumber = value.ToString();

            return readNumber.Select(x => _digits[x]).ToList();
        }

        internal static Dictionary<short, int> SearchingDecomposition(long candidate, int numericalBase)
        {
            var result = new Dictionary<short, int>();

            foreach (var item in Decompose(candidate, numericalBase))
            {
                if (result.ContainsKey(item))
                    result[item]++;
                else
                    result.Add(item, 1);
            }

            return result;
        }

        internal static int DigitSum(params List<short>[] numbers)
            => numbers.SelectMany(number => number).Aggregate(0, (current, s) => current + s);

        public bool Equals(Decomposition other) =>
            Digits.Count == other.Digits.Count && Digits.All(d => other.Digits.Contains(d));
    }
}