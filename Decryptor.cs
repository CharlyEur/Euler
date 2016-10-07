using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler.Core
{
    public class Decryptor
    {
        public static long ReadTextAndSumUpLetters()
        {
            var ciphered = System.IO.File.ReadAllText(@"Z:\p059_cipher.txt");

            byte[] candidate = ciphered.Split(',').Select(x => byte.Parse(x)).ToArray();

            var keys = GenerateKeys();

            var decryptedMessage = string.Empty;

            foreach (var key in keys)
            {
                var xoredBytes = ApplyKey(candidate, key);

                if (AssessEnglish(xoredBytes, out decryptedMessage))
                    break;
            }

            return decryptedMessage.Select(x => (int)x).Sum();
        }
        private static bool AssessEnglish(byte[] message, out string clearMessage)
        {
            clearMessage = string.Empty;

            byte byteMin = message.Min();
            byte byteMax = message.Max();

            if (byteMin < 31)
                return false;

            if (byteMax > 127)
                return false;

            clearMessage = string.Concat(Encoding.ASCII.GetChars(message));

            if (clearMessage.Contains("@")
             || clearMessage.Contains("~")
             || clearMessage.Contains("#")
             || clearMessage.Contains("%")
             || clearMessage.Contains("&"))
                return false;

            if (clearMessage.Contains("is")
             || clearMessage.Contains("to")
             || clearMessage.Contains("he")
             || clearMessage.Contains("the")
             || clearMessage.Contains("was"))
                return true;

            return false;
        }

        private static byte[] ApplyKey(byte[] candidate, byte[] key)
        {
            byte[] result = new byte[candidate.Length];

            for (var i = 0; i < candidate.Length; i = i + 3)
                result[i] = (byte)(candidate[i] ^ key[0]);

            for (var i = 1; i < candidate.Length; i = i + 3)
                result[i] = (byte)(candidate[i] ^ key[1]);

            for (var i = 2; i < candidate.Length; i = i + 3)
                result[i] = (byte)(candidate[i] ^ key[2]);

            return result;
        }

        private static IEnumerable<byte[]> GenerateKeys()
        {
            byte shift = 97;

            for (byte i = 0; i < 26; i++)
            {
                for (byte j = 0; j < 26; j++)
                {
                    for (byte k = 0; k < 26; k++)
                    {
                        var letter1 = Convert.ToChar(shift + i);
                        var letter2 = Convert.ToChar(shift + j);
                        var letter3 = Convert.ToChar(shift + k);

                        var observer = String.Concat(letter1, letter2, letter3);

                        if (observer.Length > 18)
                            throw new AccessViolationException();

                        yield return new byte[] { (byte)(shift + i), (byte)(shift + j), (byte)(shift + k) };
                    }
                }
            }
        }

        internal static char XorChars(char a, char b)
        {
            var aBytes = Encoding.ASCII.GetBytes(new char[] { a });
            var bBytes = Encoding.ASCII.GetBytes(new char[] { b });

            var xorResult = new byte[aBytes.Length];

            for (var i = 0; i < aBytes.Length; i++)
                xorResult[i] = (byte)(aBytes[i] ^ bBytes[i]);

            return Encoding.ASCII.GetChars(xorResult)[0];
        }
    }
}