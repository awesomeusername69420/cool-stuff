using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Krypt
{
    public abstract class CryptoAlgorithm
    {
        public List<string> ALPHABET = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public Regex REGEX_ALPHANUMERIC = new Regex(@"[^a-zA-Z0-9]", RegexOptions.None);
        public Regex REGEX_NUMERIC = new Regex(@"[^a-zA-Z0-9]", RegexOptions.None);

        public string SortString(string s) // Sorts the characters of a string in abc order
        {
            char[] chars = s.ToCharArray();

            Array.Sort(chars);

            return new string(chars);
        }

        public string StringNoRep(string s) // Removes repeating letters in a string
        {
            List<char> letters = new List<char>();

            foreach (char c in s)
            {
                if (letters.Contains(c))
                {
                    continue;
                }

                letters.Add(c);
            }

            return string.Join("", letters.ToArray());
        }

        public int CryptClamp(int x = 0, int min = -2147483648, int max = 2147483647) // Used to clamp values
        {
            if (x < min)
            {
                return min;
            }

            if (x > max)
            {
                return max;
            }

            return x;
        }

        public int CryptModInt(object a, object b) // C# modulo isn't real modulo, it's a remainder. Negative numbers = explode
        {
            double num1 = 1;
            double num2 = 1;

            double.TryParse(a.ToString(), out num1);
            double.TryParse(b.ToString(), out num2);

            return (int)(num1 - num2 * Math.Floor(num1 / num2));
        }

        public string GetStringParameter(List<string> parameters, int index) // Cleans up the code by removing a bunch of error catching shenanigans all over the place
        {
            if (index < 0 || index >= parameters.Count)
            {
                return string.Empty;
            }

            return parameters[index];
        }

        public int GetIntParameter(List<string> parameters, int index)
        {
            int result = 0;

            if (index < 0 || index >= parameters.Count)
            {
                return result;
            }

            int.TryParse(parameters[index], out result);

            return result;
        }

        public int GetLetterIndex(string s, List<string> alphabet = null) // Turns a letter into a number (A = 0, B = 1, C = 2, ...)
        {
            if (alphabet == null)
            {
                alphabet = this.ALPHABET; // Allows changing of the letter index order (As used in the keyword cipher)
            }

            s = s.ToUpper();

            for (int i = 0; i < alphabet.Count; i++)
            {
                if (ALPHABET[i].Equals(s))
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetLetterIndex(char c, List<string> alphabet = null) // Same as above but uses a character instead of a string
        {
            if (alphabet == null)
            {
                alphabet = this.ALPHABET;
            }

            c = char.ToUpper(c);

            for (int i = 0; i < alphabet.Count; i++)
            {
                if (char.Parse(alphabet[i]) == c)
                {
                    return i;
                }
            }

            return -1;
        }

        public string GetLetter(int n, List<string> alphabet = null) // Turns a number into a letter
        {
            if (alphabet == null)
            {
                alphabet = this.ALPHABET;
            }

            return alphabet[n];
        }

        public string CleanString(string s) // Keep only alphanumeric characters
        {
            return REGEX_ALPHANUMERIC.Replace(s, string.Empty);
        }

        // Overrides

        public abstract string Encrypt();
        public abstract string Decrypt();
        public abstract List<string> Bruteforce(); // Hardly used
        public abstract void PassParameters(List<string> parameters, bool reset = true); // There's probably a better way of implementing this but I've never used abstract classes before. Reset isn't used but it might be at one point
    }
}
