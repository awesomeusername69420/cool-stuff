using System.Collections.Generic;
using System.Linq;

namespace Krypt.Algorithms
{
    public class Vigenere : CryptoAlgorithm
    {
        private List<string> PARAMETERS = new List<string>();
        private List<List<string>> WORDBOX = null; // Vigenere square

        private string GenerateLengthKeyword(string plaintext, string keyword) // Repeats a keyword under the plaintext to create poly alphabetic lineup
        {
            keyword = keyword.Substring(0, base.CryptClamp(keyword.Length, 0, plaintext.Length)); // Make sure key isn't longer than ciphertext

            int startlen = keyword.Length;
            int curpos = 0;

            while (keyword.Length < plaintext.Length) // Repeat key over and over again
            {
                keyword = keyword + keyword[curpos];
                curpos = base.CryptModInt(curpos + 1, startlen); // Loop back to beginning of key
            }

            return keyword;
        }

        private void GenerateWordBox() // The vigenere square is just a square of caesar shifts
        {
            WORDBOX = new List<List<string>>();

            string text = string.Join(" ", base.ALPHABET.ToArray()); // The original, unmodified alphabet for reference

            CaesarShift cs = new CaesarShift();

            for (int i = 0; i <= base.ALPHABET.Count; i++)
            {
                cs.PassParameters(new List<string>() { text, i.ToString() }); // Shift the alphabet however many times (26)

                List<string> curlist = new List<string>();

                foreach (char c in cs.Encrypt())
                {
                    curlist.Add(c.ToString());
                }

                WORDBOX.Add(curlist);
            }
        }

        public override string Encrypt()
        {
            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper().Replace(" ", string.Empty);
            string keyword = GenerateLengthKeyword(plaintext, base.CleanString(base.GetStringParameter(this.PARAMETERS, 1)).ToUpper().Replace(" ", string.Empty));
            string ciphertext = string.Empty;

            int column = -1;
            int row = -1;

            for (int i = 0; i < plaintext.Length; i++)
            {
                char plainchar = plaintext[i];

                if (!char.IsLetterOrDigit(plainchar)) // Invalid character
                {
                    ciphertext = ciphertext + plainchar;
                    continue;
                }

                char keychar = keyword[i];

                for (int columnindex = 0; columnindex < WORDBOX[0].Count; columnindex++)
                {
                    System.Console.WriteLine(WORDBOX[0][columnindex]);
                    char c = char.Parse(WORDBOX[0][columnindex]);

                    if (c == plainchar)
                    {
                        column = columnindex;
                        break;
                    }
                }

                for (int rowindex = 0; rowindex < WORDBOX.Count; rowindex++)
                {
                    char c = char.Parse(WORDBOX[rowindex][0]);

                    if (c == keychar)
                    {
                        row = rowindex;
                        break;
                    }
                }

                if (column == -1 || row == -1) // Invalid character
                {
                    ciphertext = ciphertext + plainchar;

                    column = -1;
                    row = -1;

                    continue;
                }

                ciphertext = ciphertext + WORDBOX[row][column];

                column = -1;
                row = -1;
            }

            return ciphertext.Trim();
        }

        public override string Decrypt()
        {
            string ciphertext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper().Replace(" ", string.Empty);
            string keyword = GenerateLengthKeyword(ciphertext, base.CleanString(base.GetStringParameter(this.PARAMETERS, 1)).ToUpper().Replace(" ", string.Empty));
            string plaintext = string.Empty;

            for (int i = 0; i < keyword.Length; i++)
            {
                char keychar = keyword[i];
                char cipherchar = ciphertext[i];

                int row = -1;

                for (int rowindex = 0; rowindex < WORDBOX.Count; rowindex++)
                {
                    char c = char.Parse(WORDBOX[rowindex][0]);

                    if (c == keychar)
                    {
                        row = rowindex;
                        break;
                    }
                }

                if (row == -1) // Invalid character
                {
                    plaintext = plaintext + cipherchar;
                    continue;
                }

                char plainchar = char.Parse(WORDBOX[0][WORDBOX[row].IndexOf(cipherchar.ToString())]);

                if (!char.IsLetterOrDigit(plainchar))
                {
                    plaintext = plaintext + cipherchar;
                    continue;
                }

                plaintext = plaintext + plainchar;
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

            GenerateWordBox();
        }
    }
}
