using System.Collections.Generic;

namespace Krypt.Algorithms
{
    public class Keyword : CryptoAlgorithm
    {
        private List<string> PARAMETERS = new List<string>();
        private List<string> CUSTOM_ALPHABET = null;

        private void GenerateCustomAlphabet() // Creates a custom alphabet with the given keyword (Ex: Keyword = "secret" => S E C R T A B D F G H I J K L M N O P Q U V W X Y Z)
        {
            int resumeIndex = 0;
            int maxIndex = base.ALPHABET.Count - 1;
            string keyword = base.CleanString(base.GetStringParameter(this.PARAMETERS, 1)).ToUpper().Replace(" ", string.Empty); // Remove spaces

            List<string> newAlphabet = new List<string>();

            // This doesn't use StringNoRep because it needs some extra stuff

            foreach (char c in keyword) // Go through keyword letters
            {
                string s = c.ToString();

                if (newAlphabet.Contains(s)) // Letter already inserted
                {
                    continue;
                }

                newAlphabet.Add(s);
                resumeIndex = base.ALPHABET.IndexOf(s) + 1;
            }

            foreach (string s in base.ALPHABET) // Go through the remaining alphabet
            {
                if (newAlphabet.Contains(s))
                {
                    continue;
                }

                newAlphabet.Add(s);
            }

            this.CUSTOM_ALPHABET = newAlphabet;
        }

        public override string Encrypt()
        {
            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();
            string ciphertext = string.Empty;

            foreach (char c in plaintext)
            {
                int realIndex = base.GetLetterIndex(c);

                if (realIndex < 0)
                {
                    ciphertext = ciphertext + c;
                    continue;
                }

                ciphertext = ciphertext + base.GetLetter(realIndex, this.CUSTOM_ALPHABET);
            }

            return ciphertext.Trim();
        }

        public override string Decrypt()
        {
            string ciphertext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();
            string plaintext = string.Empty;

            foreach (char c in ciphertext)
            {
                int realIndex = base.GetLetterIndex(c, this.CUSTOM_ALPHABET);

                if (realIndex < 0)
                {
                    plaintext = plaintext + c;
                    continue;
                }

                plaintext = plaintext + base.GetLetter(realIndex);
            }

            return plaintext.Trim();
        }

        public override List<string> Bruteforce() // Not used, that'd be LOT of work
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

            this.GenerateCustomAlphabet();
        }
    }
}
