using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Krypt.Algorithms;

namespace Krypt
{
    public partial class Main : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Main()
        {
            InitializeComponent();
        }

        // Cipher things (Dumb way of doing this, but I'm lazy)

        private CaesarShift cs = new CaesarShift();
        private Keyword kw = new Keyword();
        private Vigenere vg = new Vigenere();
        private ADFGVX ad = new ADFGVX();
        private Book bk = new Book();
        private RandomAlphabetic ra = new RandomAlphabetic();
        private Affine af = new Affine();
        private Multiplicative mp = new Multiplicative();

        public static bool ADFGVX_Status = false;

        public void Log(string header, string body)
        {
            Global_Output.AppendText(header + Environment.NewLine + body + Environment.NewLine);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            AllocConsole();

            ADFGVX_Status = ADFGVX_ExportXLSX.Checked; // Majik?
            Multiplicative_Key.Maximum = cs.ALPHABET.Count - 1;
        }

        // Also dumb

        private void Caesar_Crypt_Click(object sender, EventArgs e)
        {
            string val = Caesar_Shift.Value.ToString();
            cs.PassParameters(new List<string>() { Caesar_Input.Text.Trim(), val});

            Log("Caesar Shift (" + val + "):", cs.Encrypt());
        }

        private void Caesar_Bruteforce_Click(object sender, EventArgs e)
        {
            cs.PassParameters(new List<string>() { Caesar_Input.Text.Trim() });

            List<string> results = cs.Bruteforce();

            Log("Caesar Shift (Bruteforce):", string.Join(Environment.NewLine, results.ToArray()));
        }

        private void Keyword_Encrypt_Click(object sender, EventArgs e)
        {
            string keyword = Keyword_Keyword.Text.Trim();

            kw.PassParameters(new List<string>() { Keyword_Input.Text.Trim(), keyword });
            Log("Keyword (" + keyword + "):", kw.Encrypt());
        }

        private void Keyword_Decrypt_Click(object sender, EventArgs e)
        {
            string keyword = Keyword_Keyword.Text.Trim();

            kw.PassParameters(new List<string>() { Keyword_Input.Text.Trim(), keyword });
            Log("Keyword (" + keyword + "):", kw.Decrypt());
        }

        private void Vigenere_Encrypt_Click(object sender, EventArgs e)
        {
            string keyword = Vigenere_Keyword.Text.Trim();

            vg.PassParameters(new List<string>() { Vigenere_Input.Text.Trim(), keyword });
            Log("Vigenere (" + keyword + "):", vg.Encrypt());
        }

        private void Vigenere_Decrypt_Click(object sender, EventArgs e)
        {
            string keyword = Vigenere_Keyword.Text.Trim();

            vg.PassParameters(new List<string>() { Vigenere_Input.Text.Trim(), keyword });
            Log("Vigenere (" + keyword + "):", vg.Decrypt());
        }

        private void ADFGVX_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Microsoft Excel Spreadsheet|*.xlsx";
            sfd.Title = "Export Default XLSX";
            sfd.ShowDialog();

            if (sfd.FileName != string.Empty)
            {
                try
                {
                    Assembly assem = Assembly.GetExecutingAssembly();

                    string root = Path.GetPathRoot(assem.Location);

                    using (Stream input = assem.GetManifestResourceStream("Krypt.Resources.ADFGVX.xlsx"))
                    {
                        using (Stream output = sfd.OpenFile())
                        {
                            input.CopyTo(output);
                        }
                    }

                    Log("ADFGVX (Export):", "File successfully exported");
                }
                catch (Exception ex)
                {
                    Log("ADFGVX (Export):", "File failed to export - Error: " + ex.ToString());
                }
            } else
            {
                Log("ADFGVX (Export):", "File failed to export - Export was cancelled");
            }
        }

        private void ADFGVX_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Microsoft Excel Spreadsheet|*.xlsx";
            ofd.Title = "Import XLSX";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ad.PassParameters(new List<string>() { ofd.FileName });

                if (ad.BuildXLSXData())
                {
                    Log("ADFGVX (Import):", "File successfully imported");
                } else
                {
                    Log("ADFGVX (Import):", "File failed to import");
                }
            } else
            {
                Log("ADFGVX (Import):", "File failed to import - Import was cancelled");
            }
        }

        private void ADFGVX_Encrypt_Click(object sender, EventArgs e)
        {
            string keyword = ADFGVX_Keyword.Text.Trim();

            ad.PassParameters(new List<string>() { ADFGVX_Input.Text.Trim(), keyword });
            Log("ADFGVX (" + keyword + "):", ad.Encrypt());
        }

        private void ADFGVX_Decrypt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This does not exist yet", "Whoops", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdateADFGVXStatus(object sender, EventArgs e) // Used to transmit checkbox information
        {
            this.Invoke((MethodInvoker) delegate
            {
                ADFGVX_Status = ADFGVX_ExportXLSX.Checked; // Ok this can't access it normally but Main_Load can yep yeah mhm sure thing yeah yes makes sense thanks Microsoft
            });
        }

        private void Book_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            ofd.Title = "Import TXT";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                bk.PassParameters(new List<string>() { ofd.FileName });

                if (bk.BuildIndexes())
                {
                    Log("Book (Import):", "File successfully imported");
                }
                else
                {
                    Log("Book (Import):", "File failed to import");
                }
            }
            else
            {
                Log("Book (Import):", "File failed to import - Import was cancelled");
            }
        }

        private void Book_Encrypt_Click(object sender, EventArgs e)
        {
            bk.PassParameters(new List<string>() { Book_Input.Text.Trim() });
            Log("Book:", bk.Encrypt());
        }

        private void Book_Decrypt_Click(object sender, EventArgs e)
        {
            bk.PassParameters(new List<string>() { Book_Input.Text.Trim() });
            Log("Book:", bk.Decrypt());
        }

        private void RandomAlphabet_Encrypt_Click(object sender, EventArgs e)
        {
            string keyword = RandomAlphabet_Keyword.Text.Trim();

            ra.PassParameters(new List<string>() { RandomAlphabet_Input.Text.Trim(), keyword });
            Log("Random Alphabetic (" + keyword + "):", ra.Encrypt());
        }

        private void RandomAlphabet_Decrypt_Click(object sender, EventArgs e)
        {
            string keyword = RandomAlphabet_Keyword.Text.Trim();

            ra.PassParameters(new List<string>() { RandomAlphabet_Input.Text.Trim(), keyword });
            Log("Random Alphabetic (" + keyword + "):", ra.Decrypt());
        }

        private void Affine_Encrypt_Click(object sender, EventArgs e)
        {
            string mkey = Affine_MultiplicitiveKey.Value.ToString();
            string akey = Affine_AdditiveKey.Value.ToString();

            af.PassParameters(new List<string>() { Affine_Input.Text.Trim(), mkey, akey });
            Log("Affine (" + mkey + ", " + akey + "):", af.Encrypt());
        }

        private void Affine_Decrypt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This does not exist yet", "Whoops", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Multiplicative_Encrypt_Click(object sender, EventArgs e)
        {
            string key = Multiplicative_Key.Value.ToString();

            mp.PassParameters(new List<string>() { Multiplicative_Input.Text, key });
            Log("Multplicative (" + key + "):", mp.Encrypt());
        }

        private void Multiplicative_Decrypt_Click(object sender, EventArgs e)
        {
            string key = Multiplicative_Key.Value.ToString();

            mp.PassParameters(new List<string>() { Multiplicative_Input.Text, key });
            Log("Multplicative (" + key + "):", mp.Decrypt());
        }
    }
}
