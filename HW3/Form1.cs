// Minh The Nguyen - 011741737
using System;
using System.IO;
using System.Windows.Forms;

namespace HW3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadText(TextReader sr)
        {
            textBox1.Text = sr.ReadToEnd();
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, textBox1.Text);

            }
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        LoadText(sr);
                    }
                }
            }
        }

        private void loadFibonacciNumbersfirst50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader ftr = new FibonacciTextReader(50);
            textBox1.Text = ftr.ReadToEnd();
        }

        private void loadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader ftr = new FibonacciTextReader(100);
            textBox1.Text = ftr.ReadToEnd();
        }

    }
}
