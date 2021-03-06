using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UnRussian
{
    public partial class Form1 : Form // Form1 B-)
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Dictionary<string, string> russian = new Dictionary<string, string>() // Gay
        {
            {"А", "A"},
            {"Б", "6"},
            {"В", "B"},
            {"Г", "r"},
            {"Д", "A"},
            {"Е", "E"},
            {"Ж", "X"},
            {"З", "E"},
            {"И", "N"},
            {"Й", "N"},
            {"К", "K"},
            {"Л", "n"},
            {"М", "M"},
            {"Н", "H"},
            {"О", "O"},
            {"П", "N"},
            {"Р", "P"},
            {"С", "C"},
            {"Т", "T"},
            {"У", "y"},
            {"Ф", "o"},
            {"Х", "X"},
            {"Ц", "U"},
            {"Ч", "u"},
            {"Ш", "w"},
            {"Щ", "w"},
            {"Ъ", "b"},
            {"Ы", "bl"},
            {"Ь", "b"},
            {"Э", "3"},
            {"Ю", "io"},
            {"Я", "R"},
            {"а", "a"},
            {"б", "6"},
            {"в", "B"},
            {"г", "r"},
            {"д", "A"},
            {"е", "e"},
            {"ж", "x"},
            {"з", "3"},
            {"и", "N"},
            {"й", "N"},
            {"к", "K"},
            {"л", "N"},
            {"м", "M"},
            {"н", "H"},
            {"о", "O"},
            {"п", "n"},
            {"р", "p"},
            {"с", "c"},
            {"т", "T"},
            {"у", "y"},
            {"ф", "o"},
            {"х", "x"},
            {"ц", "u"},
            {"ч", "u"},
            {"ш", "w"},
            {"щ", "w"},
            {"ъ", "b"},
            {"ы", "bl"},
            {"ь", "b"},
            {"э", "3"},
            {"ю", "io"},
            {"я", "R"},
            {"Ѡ", "O"},
            {"ѡ", "w"},
            {"Ѣ", "b"},
            {"ѣ", "b"},
            {"Ѥ", "iE"},
            {"ѥ", "iE"},
            {"Ѧ", "A"},
            {"ѧ", "A"},
            {"Ѩ", "A"},
            {"ѩ", "A"},
            {"Ѫ", "A"},
            {"ѫ", "A"},
            {"Ѭ", "A"},
            {"ѭ", "A"},
            {"Ѯ", "3"},
            {"ѯ", "3"},
            {"Ѱ", "U"},
            {"ѱ", "u"},
            {"Ѳ", "O"},
            {"ѳ", "o"},
            {"Ѵ", "V"},
            {"ѵ", "v"},
            {"Ѷ", "V"},
            {"ѷ", "v"},
            {"Ѹ", "Oy"},
            {"ѹ", "oy"},
            {"Ѻ", "O"},
            {"ѻ", "o"},
            {"Ѽ", "O"},
            {"ѽ", "o"},
            {"Ѿ", "O"},
            {"ѿ", "w"},
            {"Ҁ", "C"},
            {"ҁ", "c"},
            {"҂", "#"},
            {"Ҍ", "b"},
            {"ҍ", "b"},
            {"Ҏ", "p"},
            {"ҏ", "p"},
            {"Ґ", "r"},
            {"ґ", "r"},
            {"Ғ", "F"},
            {"ғ", "f"},
            {"Ҕ", "h"},
            {"ҕ", "h"},
            {"Җ", "x"},
            {"җ", "x"},
            {"Ҙ", "3"},
            {"ҙ", "3"},
            {"Қ", "K"},
            {"қ", "K"},
            {"Ҝ", "K"},
            {"ҝ", "K"},
            {"Ҟ", "K"},
            {"ҟ", "K"},
            {"Ҡ", "K"},
            {"ҡ", "K"},
            {"Ң", "H"},
            {"ң", "H"},
            {"Ҥ", "H"},
            {"ҥ", "H"},
            {"Ҧ", "rb"},
            {"ҧ", "rb"},
            {"Ҩ", "a"},
            {"ҩ", "a"},
            {"Ҫ", "c"},
            {"ҫ", "c"},
            {"Ҭ", "T"},
            {"ҭ", "T"},
            {"Ү", "Y"},
            {"ү", "Y"},
            {"Ұ", "Y"},
            {"ұ", "Y"},
            {"Ҳ", "X"},
            {"ҳ", "x"},
            {"Ҵ", "Tl"},
            {"ҵ", "Tl"},
            {"Ҷ", "4"},
            {"ҷ", "4"},
            {"Ҹ", "4"},
            {"ҹ", "4"},
            {"Һ", "h"},
            {"һ", "h"},
            {"Ҽ", "e"},
            {"ҽ", "e"},
            {"Ҿ", "e"},
            {"ҿ", "e"},
            {"Ӏ", "I"},
            {"Ӂ", "x"},
            {"ӂ", "x"},
            {"Ӄ", "k"},
            {"ӄ", "k"},
            {"Ӈ", "H"},
            {"ӈ", "H"},
            {"Ӌ", "y"},
            {"ӌ", "y"},
            {"Ӑ", "A"},
            {"ӑ", "a"},
            {"Ӓ", "A"},
            {"ӓ", "a"},
            {"Ӕ", "AE"},
            {"ӕ", "ae"},
            {"Ӗ", "E"},
            {"ӗ", "e"},
            {"Ә", "e"},
            {"ә", "e"},
            {"Ӛ", "e"},
            {"ӛ", "e"},
            {"Ӝ", "x"},
            {"ӝ", "x"},
            {"Ӟ", "3"},
            {"ӟ", "3"},
            {"Ӡ", "3"},
            {"ӡ", "3"},
            {"Ӣ", "N"},
            {"ӣ", "N"},
            {"Ӥ", "N"},
            {"ӥ", "N"},
            {"Ӧ", "o"},
            {"ӧ", "o"},
            {"Ө", "0"},
            {"ө", "0"},
            {"Ӫ", "0"},
            {"ӫ", "0"},
            {"Ӭ", "3"},
            {"ӭ", "3"},
            {"Ӯ", "y"},
            {"ӯ", "y"},
            {"Ӱ", "y"},
            {"ӱ", "y"},
            {"Ӳ", "y"},
            {"ӳ", "y"},
            {"Ӵ", "4"},
            {"ӵ", "4"},
            {"Ӹ", "bl"},
            {"ӹ", "bl"},
            {"Ё", "E"},
            {"Ѕ", "S"},
            {"І", "I"},
            {"ё", "e"},
            {"ѕ", "s"},
            {"і", "i"}
        };

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string og = textBox1.Text;

            textBox2.Text = "";

            foreach (string s in russian.Keys) // Replace characters
            {
                og = og.Replace(s, russian[s]);
            }

            textBox2.Text = og;
        }
    }
}
