using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SuchenUndErsetzen
{
    public partial class Form1 : Form
    {
        private static int counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            label5.Text = "";
            label7.Text = "";
            this.button1.BackColor = Color.LightGray;

            string strDir = textBox2.Text;
            string ersatzText = textBox1.Text;
            string suchenText = textBox3.Text;

            if (string.IsNullOrEmpty(strDir) || string.IsNullOrEmpty(suchenText) || string.IsNullOrEmpty(ersatzText))
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.", "Hinweis");
                return;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            DateisystemDurchlaufen(strDir, suchenText, ersatzText);
            sw.Stop();

            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", sw.Elapsed.Hours, sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10);
            label5.Text = elapsedTime;
            label6.Text = "Fertig!";
            this.button1.BackColor = Color.Green;
            this.label7.Text = counter.ToString();
        }

        public static void DateisystemDurchlaufen(string strDir, string suchenText, string ersatzText)
        {
            string pattern = "(\\w*)";

            if (suchenText.Contains("*"))
            {
                int foundAsterisk = suchenText.IndexOf("*");
                suchenText = suchenText.Remove(foundAsterisk, 1);
                suchenText = suchenText.Insert(foundAsterisk, pattern);

                DateisystemDurchlaufen(new DirectoryInfo(strDir), suchenText, ersatzText);
            }
            else
                DateisystemDurchlaufen(new DirectoryInfo(strDir), suchenText, ersatzText);
        }

        private static void DateisystemDurchlaufen(DirectoryInfo di, string suchenText, string ersatzText)
        {
            try
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    string text = File.ReadAllText(fi.FullName, Encoding.UTF8);
                    text = Regex.Replace(text, suchenText, m => { counter++; return ersatzText; });
                    File.WriteAllText(fi.FullName, text);
                }

                foreach (DirectoryInfo diSub in di.GetDirectories())
                {
                    DateisystemDurchlaufen(diSub, suchenText, ersatzText);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Info");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            this.button1.BackColor = Color.LightGray;

            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                DialogResult result = ofd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textBox2.Text = ofd.SelectedPath;
                }
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simon Wroblewski\nFachinformatiker Anwendungsentwicklung", "Einstellungstest", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void feedbackSendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Emailadresse: bewerbungsmappe@kabelmail.de", "Feedback", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
