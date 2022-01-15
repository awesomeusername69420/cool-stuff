// Don't ask questions

using JNogueira.Discord.Webhook.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebhookSpammer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void log(string s)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                console.Text = console.Text + s + Environment.NewLine;
            }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var Client = new DiscordWebhookClient(whurl.Text);

                var message = new DiscordMessage(
                    messag.Text,
                    username: username.Text,
                    avatarUrl: avurl.Text,
                    tts: false,
                    embeds: null
                );

                await Client.SendToDiscord(message);

                log("Probably sent a message");

            }
            catch (Exception ex)
            {
                log("Something broke. Exception: " + ex.ToString());
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception) { }
        }
    }
}
