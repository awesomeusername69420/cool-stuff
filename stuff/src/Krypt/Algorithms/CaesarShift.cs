using System.Collections.Generic;

namespace Krypt.Algorithms
{
    public class CaesarShift : CryptoAlgorithm
    {
        private List<string> PARAMETERS = new List<string>();

        public override string Encrypt()
        {
            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();
            int shift = base.GetIntParameter(PARAMETERS, 1);

            string ciphertext = string.Empty;

            foreach (char c in plaintext)
            {
                int newIndex = GetLetterIndex(c);

                if (newIndex < 0) // Not a valid letter
                {
                    ciphertext = ciphertext + c;
                    continue;
                }

                ciphertext = ciphertext + base.GetLetter(base.CryptModInt(newIndex + shift, base.ALPHABET.Count)); // Wraps letters back around (Allows for looping and decryption)
            }

            return ciphertext.Trim();
        }

        public override string Decrypt() // Not used as negative values are used for decryption
        {
            return string.Empty;
        }

        public override List<string> Bruteforce()
        {
            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0));
            List<string> list = new List<string>();

            for (int i = -25; i <= 25; i++)
            {
                this.PassParameters(new List<string>() { plaintext, i.ToString() }); // I'm such a good programmer B-)
                list.Add(i + " => " + this.Encrypt());
            }

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
