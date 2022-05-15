using System;
using System.Collections.Generic;
using System.Linq;

using ClosedXML.Excel; // I originally made this with IronXL before realizing it needed a license so I had to recode it with ClosedXML,,,,,,,,,,

namespace Krypt.Algorithms
{
    public class ADFGVX : CryptoAlgorithm
    {
        private List<string> PARAMETERS = new List<string>();
        private List<List<string>> WORDBOX = null;
        private List<string> ORDER = new List<string>() { "A", "D", "F", "G", "V", "X" }; // Cleans things up a little
        private string WORKBOOK_PATH = string.Empty;

        private List<List<string>> CreateExcelList(string start, string end) // Creates a list of Excel Cells
        {
            start = start.ToUpper();
            end = end.ToUpper();

            List<List<string>> cells = new List<List<string>>();

            int cindex = 0;
            int startindex = base.ALPHABET.IndexOf(start[0].ToString());

            int min = 1;
            int max = 1;

            int.TryParse(start[1].ToString(), out min);
            int.TryParse(end[1].ToString(), out max);

            for (int i = min; i <= max; i++, cindex++)
            {
                cells.Add(new List<string>());

                for (int ii = startindex; ii <= startindex + (max - min); ii++)
                {
                    cells[cindex].Add(base.ALPHABET[ii] + i);
                }
            }

            return cells;
        }

        public bool BuildXLSXData()
        {
            WORKBOOK_PATH = base.GetStringParameter(this.PARAMETERS, 0);

            if (WORKBOOK_PATH.Equals(string.Empty))
            {
                return false;
            }

            try
            {
                int index = 0;
                int row = 0;

                WORDBOX = new List<List<string>>();
                WORDBOX.Add(new List<string>());

                XLWorkbook workbook = new XLWorkbook(WORKBOOK_PATH);
                var worksheet = workbook.Worksheets.First(); // Has to be a var because protection level garbage I don't understand

                List<List<string>> cells = CreateExcelList("B2", "G7");

                foreach (List<string> list in cells) // Turn Excel data into List data
                {
                    foreach (string s in list)
                    {
                        WORDBOX[row].Add(base.CleanString(worksheet.Cell(s).Value.ToString()).Substring(0, 1).ToUpper());

                        index = index + 1;

                        if (index % 6 == 0)
                        {
                            WORDBOX.Add(new List<string>());
                            row = row + 1;
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                WORDBOX = null;
                WORKBOOK_PATH = String.Empty;

                return false;
            }
        }

        public override string Encrypt()
        {
            if (WORDBOX == null || WORKBOOK_PATH.Equals(string.Empty))
            {
                return "XLSX NOT INITIALIZED";
            }

            string keyword = base.StringNoRep(base.CleanString(base.GetStringParameter(this.PARAMETERS, 1)).ToUpper());
            string plaintext = base.CleanString(base.GetStringParameter(this.PARAMETERS, 0)).ToUpper();
            string ciphertext = string.Empty;

            foreach (char c in plaintext)
            {
                string s = c.ToString();

                for (int row = 0; row < WORDBOX.Count; row++)
                {
                    for (int col = 0; col < WORDBOX[row].Count; col++)
                    {
                        if (WORDBOX[row][col].Equals(s))
                        {
                            ciphertext = ciphertext + ORDER[row] + ORDER[col];
                        }
                    }
                }
            }

            int curindex = 0;
            char curkey = keyword[curindex];

            Dictionary<char, List<string>> cipherdict = new Dictionary<char, List<string>>();
            
            foreach (char c in keyword) // Generate empty dictionary
            {
                cipherdict[c] = new List<string>();
            }

            int max = 0;

            for (int i = 0; i < ciphertext.Length; i++, curindex++)
            {
                curindex = base.CryptModInt(curindex, keyword.Length);
                curkey = keyword[curindex];

                cipherdict[curkey].Add(ciphertext[i].ToString());

                if (cipherdict[curkey].Count > max)
                {
                    max = cipherdict[curkey].Count;
                }
            }

            foreach (char c in cipherdict.Keys) // Fill empty spaces with 'X'
            {
                while (cipherdict[c].Count < max)
                {
                    cipherdict[c].Add("X");
                }
            }

            string sKeyword = base.SortString(keyword);
            ciphertext = string.Empty;

            for (int i = 0; i < max; i++) // Sort the columns into ABC order
            {
                foreach (char c in sKeyword)
                {
                    ciphertext = ciphertext + cipherdict[c][i];
                }
            }

            ciphertext = ciphertext.Trim();
            
            if (Main.ADFGVX_Status) // Append new data to previously loaded xlsx
            {
                if (!WORKBOOK_PATH.Equals(string.Empty))
                {
                    try
                    {
                        XLWorkbook workbook = new XLWorkbook(WORKBOOK_PATH);

                        string worksheetname = "ADFGVX Encrypt - " + keyword;

                        try // Overwrite worksheet if it exists
                        {
                            workbook.Worksheets.Delete(worksheetname);
                        } catch (Exception) { }
                        
                        var worksheet = workbook.Worksheets.Add(worksheetname);

                        int curcol = 0;
                        int currow = 1;

                        foreach (char c in ciphertext)
                        {
                            if (!char.IsLetterOrDigit(c)) // Invalid character
                            {
                                continue;
                            }

                            worksheet.Cell(base.ALPHABET[curcol] + currow).Value = c.ToString();

                            curcol = curcol + 1;

                            if (curcol >= sKeyword.Length)
                            {
                                curcol = 0;
                                currow = currow + 1;
                            }
                        }

                        workbook.SaveAs(WORKBOOK_PATH);
                    } catch (Exception) { }  
                }
            }
            

            return ciphertext.Trim();
        }

        public override string Decrypt()
        {
            if (WORDBOX == null || WORKBOOK_PATH.Equals(string.Empty))
            {
                return "XLSX NOT INITIALIZED";
            }

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
