using System;
using System.Collections.Generic;

namespace Euler.Core
{
    public static class UsefullExtensions
    {
        public static bool IsInteger(this double toTest)
        {
            return Math.Abs(toTest - (int)toTest) < double.Epsilon;
        }

        public static void Multiply(this Dictionary<long, long> product, Dictionary<long, long> factor)
        {
            foreach (var factorWithPower in factor)
            {
                if (product.ContainsKey(factorWithPower.Key))
                    product[factorWithPower.Key] += factorWithPower.Value;
                else
                    product.Add(factorWithPower.Key, factorWithPower.Value);
            }
        }

        public static void Divide(this Dictionary<long, long> product, Dictionary<long, long> factor)
        {
            foreach (var factorWithPower in factor)
            {
                if (product.ContainsKey(factorWithPower.Key))
                    product[factorWithPower.Key] -= factorWithPower.Value;
                else
                    product.Add(factorWithPower.Key, -factorWithPower.Value);
            }
        }

        public static void Populate<T>(this T[] collection, T value)
        {
            for (int i = 0; i < collection.Length; i++)
                collection[i] = value;
        }
    }
}
