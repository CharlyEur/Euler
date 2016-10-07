﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Core
{
    public class Decomposition
    {
        public List<short> Digits { get; set; }

        public int Base { get; private set; }

        public long StandardValue { get; private set; }

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
                buffer += (long)(digits[count - i - 1] * Math.Pow(numericalBase, i));

            return buffer;
        }

        internal static List<short> Decompose(long candidate, int numericalBase)
        {
            var digits = new List<short>();

            if (candidate < numericalBase)
            {
                digits.Add((short)candidate);
                return digits;
            }

            var powerDouble = Math.Log(candidate) / Math.Log(numericalBase);
            var power = (int)Math.Floor(powerDouble);

            var test = (long)Math.Pow(numericalBase, power);
            if (candidate > test)
                power++;

            var buffer = candidate;

            for (var i = power; i >= 0; i--)
            {
                var curtPower = (long)Math.Pow(numericalBase, i);
                var digit = buffer / curtPower;
                digits.Add((short)digit);

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

        internal static List<short> Decompose(BigInteger value)
        {
            var digits = new List<short>();

            if (value < 10)
            {
                digits.Add((short)value);
                return digits;
            }

            var power = (int)Math.Floor(BigInteger.Log10(value));
            BigInteger buffer = value;

            for (int i = power; i >= 0; i--)
            {
                var curtPower = BigInteger.Pow(10, i);
                var digit = (short)BigInteger.Divide(buffer, curtPower);
                digits.Add(digit);

                buffer = BigInteger.Subtract(buffer, BigInteger.Multiply(digit, curtPower));
            }

            return digits;
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
        {
            return numbers.SelectMany(number => number).Aggregate(0, (current, s) => current + s);
        }
    }
}