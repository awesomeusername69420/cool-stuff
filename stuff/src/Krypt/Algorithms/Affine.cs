using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypt.Algorithms
{
    public class Affine : CryptoAlgorithm
    {
        private List<string> PARAMETERS = new List<string>();

        public override string Encrypt()
        {
            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();
            int mkey = base.GetIntParameter(this.PARAMETERS, 1);
            int akey = base.GetIntParameter(this.PARAMETERS, 2);

            string ciphertext = string.Empty;

            foreach (char c in plaintext)
            {
                int index = base.GetLetterIndex(c);
                ciphertext = ciphertext + base.GetLetter(base.CryptModInt(mkey * (index + akey), base.ALPHABET.Count)) + " ";
            }

            return ciphertext.Trim();
        }

        public override string Decrypt()
        {
            return string.Empty;
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
