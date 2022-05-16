using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Krypt.Tools
{
    public partial class FrequencyAnalysis : Form
    {
        public FrequencyAnalysis()
        {
            InitializeComponent();
        }

        private Dictionary<char, int> INDEXES = null;

        private void FrequencyAnalysis_Load(object sender, EventArgs e)
        {
            
        }

        private void OverrideClose(object sender, FormClosingEventArgs e) // Used to remember information in the form
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void Analyze_Click(object sender, EventArgs e)
        {
            INDEXES = new Dictionary<char, int>();

            string atext = AnalyiticalText.Text.Replace(" ", string.Empty);

            foreach (char c in atext)
            {
                if (!INDEXES.ContainsKey(c))
                {
                    INDEXES[c] = 0;
                }

                INDEXES[c] += 1;
            }

            List<char> order = new List<char>(INDEXES.Keys);

            order.Sort((a, b) => {
                return string.Compare(a.ToString(), b.ToString());
            });

            double total = atext.Length;

            foreach (char c in order)
            {
                Output.AppendText(c + " = " + (Math.Round((double)INDEXES[c] / total, 4) * 100.0) + "%" + Environment.NewLine);
            }

            Output.AppendText("~~~~~~~~~~~~~~~~~~~" + Environment.NewLine);
        }
    }
}
