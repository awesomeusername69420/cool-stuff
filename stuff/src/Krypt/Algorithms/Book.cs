using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Krypt.Algorithms
{
    public class Book : CryptoAlgorithm
    {
        Random rng = new Random();

        private List<string> PARAMETERS = new List<string>();
        private Dictionary<char, List<Int32>> INDEXES = null;

        private Regex REGEX_ALPHASPACE = new Regex(@"[^A-Z\s]", RegexOptions.None);
        private Regex REGEX_NUMERICSPACE = new Regex(@"[^0-9\s]", RegexOptions.None);

        public bool BuildIndexes()
        {
            try
            {
                string filepath = base.GetStringParameter(this.PARAMETERS, 0);

                if (filepath.Equals(string.Empty))
                {
                    return false;
                }

                string filedata = REGEX_ALPHASPACE.Replace(File.ReadAllText(filepath).ToUpper(), string.Empty); // Keep ONLY letters and spaces
                string[] words = filedata.Split(' ');

                INDEXES = new Dictionary<char, List<Int32>>();

                foreach (string s in base.ALPHABET)
                {
                    INDEXES[char.Parse(s)] = new List<Int32>();
                }

                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Trim().Equals(string.Empty)) // Stupid replacement trash garbage regex
                    {
                        continue;
                    }

                    char c = words[i][0];

                    if (!char.IsLetter(c)) // Invalid character
                    {
                        continue;
                    }

                    INDEXES[c].Add(i);
                }

                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public override string Encrypt()
        {
            if (INDEXES == null)
            {
                return String.Empty;
            }

            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();

            List<string> cipherlist = new List<string>(); // Cleans up code

            foreach (char c in plaintext)
            {
                if (!char.IsLetter(c))
                {
                    continue;
                }

                cipherlist.Add(INDEXES[c][rng.Next(INDEXES[c].Count)].ToString());
            }

            return string.Join(", ", cipherlist.ToArray());
        }

        public override string Decrypt()
        {
            if (INDEXES == null)
            {
                return String.Empty;
            }

            string ciphertext = REGEX_NUMERICSPACE.Replace(base.GetStringParameter(this.PARAMETERS, 0), string.Empty); // Keep ONLY numbers and the space between them
            string[] numbers = ciphertext.Split(' ');
            string plaintext = string.Empty;

            foreach (string s in numbers)
            {
                int n = -1;

                int.TryParse(s, out n);

                if (n == -1) // Invalid index
                {
                    continue;
                }

                foreach (char c in INDEXES.Keys)
                {
                    bool breakouter = false;

                    foreach (int ls in INDEXES[c])
                    {
                        if (ls == n)
                        {
                            plaintext = plaintext + c;
                            breakouter = true;
                            break;
                        }
                    }

                    if (breakouter)
                    {
                        break;
                    }
                }
            }

            return plaintext;
        }

        public override List<string> Bruteforce()
        {
            List<string> list = new List<string>();

            return list;
        }

        public override void PassParameters(List<string> parameters, bool reset = true)
        {
            if (reset)
            {
                this.PARAMETERS.Clear();
            }

            foreach (string s in parameters)
            {
                this.PARAMETERS.Add(s);
            }
        }
    }
}
