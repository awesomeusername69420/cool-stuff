using System;
using System.Collections.Generic;

namespace Krypt.Algorithms
{
    public class Multiplicative : CryptoAlgorithm
    {
        private List<string> PARAMETERS = new List<string>();
        private Dictionary<int, int> INVERSES = null;

        private List<int> GetFactors(int n)
        {
            List<int> factors = new List<int>();

            for (int i = 2; i <= n; i++)
            {
                if (n % i == 0)
                {
                    factors.Add(i);
                }
            }

            return factors;
        }

        private void GenerateInverses()
        {
            int max = base.ALPHABET.Count;

            List<int> MAX_FACTORS = GetFactors(max);
            List<int> RELATIVE_PRIMES = new List<int>();

            for (int i = 2; i < max; i++)
            {
                bool isvalid = true;

                foreach (int x in GetFactors(i))
                {
                    if (MAX_FACTORS.Contains(x))
                    {
                        isvalid = false;
                        break;
                    }
                }

                if (isvalid)
                {
                    RELATIVE_PRIMES.Add(i);
                }
            }

            INVERSES = new Dictionary<int, int>();

            foreach (int v in RELATIVE_PRIMES)
            {
                foreach (int x in RELATIVE_PRIMES)
                {
                    if (!INVERSES.ContainsKey(v) && !INVERSES.ContainsKey(x))
                    {
                        if ((v * x) % max == 1)
                        {
                            INVERSES[v] = x;
                            INVERSES[x] = v;
                        }
                    }
                }
            }
        }

        public override string Encrypt()
        {
            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();
            int key = base.GetIntParameter(this.PARAMETERS, 1);

            if (!INVERSES.ContainsKey(key))
            {
                return "Error - Inverse Key Not Relatively Prime to " + base.ALPHABET.Count;
            }

            string ciphertext = string.Empty;

            foreach (char c in plaintext)
            {
                int index = base.GetLetterIndex(c);
                ciphertext = ciphertext + base.GetLetter(base.CryptModInt(index * key, base.ALPHABET.Count)) + " ";
            }

            return ciphertext.Trim();
        }

        public override string Decrypt()
        {
            if (INVERSES == null)
            {
                return string.Empty;
            }

            string ciphertext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();
            int key = base.GetIntParameter(this.PARAMETERS, 1);

            if (!INVERSES.ContainsKey(key))
            {
                return "Error - Inverse Key Not Found";
            }

            int ikey = INVERSES[key];
            string plaintext = string.Empty;

            foreach (char c in ciphertext)
            {
                int index = base.GetLetterIndex(c);
                plaintext = plaintext + base.GetLetter(base.CryptModInt(index * ikey, base.ALPHABET.Count)) + " ";
            }

            return plaintext.Trim();
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

            this.GenerateInverses();
        }
    }
}
