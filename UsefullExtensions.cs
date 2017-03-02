using System;
using System.Collections.Generic;
using System.Numerics;

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

        public static BigInteger SumBig(this IEnumerable<BigInteger> collection)
        {
            BigInteger buffer = 0;

            foreach (var item in collection)
                buffer += item;

            return buffer;
        }

		/// <summary>
		/// Careful ! Deep Copy only for value types !
		/// </summary>
		/// <param name="toCopy"></param>
		/// <returns></returns>
		public static List<T> DeepCopy<T>(this List<T> toCopy)
		{
			var result = new List<T>();

			foreach (var item in toCopy)
				result.Add(item);

			return result;
		}
    }
}
