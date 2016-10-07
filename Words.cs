using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler.Core
{
    public class Words
    {
        private const string path = @"Z:\p042_words.txt";

        private readonly static GeometricNumbersProvider source;

        static Words()
        {
            source = new GeometricNumbersProvider(200);
        }

        public static int CountTriangleWords()
        {
            var candidateList = ExtractStringEnumerable();
            var result = candidateList.Count(IsTriangleWord);

            return result;
        }

        private static IEnumerable<string> ExtractStringEnumerable()
        {
            var toAnalyze = System.IO.File.ReadAllText(path);

            var rawResults = toAnalyze.Split('"');

            foreach (var item in rawResults)
            {
                var toRead = item.Replace("\"", string.Empty).Replace(",", string.Empty);

                if (!string.IsNullOrEmpty(toRead))
                    yield return toRead;
            }
        }

        public static bool IsTriangleWord(string candidate)
        {
            int wordWeigth = WeightWord(candidate);

            return GeometricNumbersProvider.IsTriangle(wordWeigth);
        }

        internal static int WeightWord(string candidate)
        {
            return candidate.Sum(c => WeightChar(c));
        }

        private static int WeightChar(char c)
        {
            return (int)c - 64;
        }
    }
}