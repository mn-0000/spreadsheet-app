using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            List<int> randomNumbers = new List<int>(10000);
            Enumerable.Range(0, 20000);
            int number;
            for (int i = 0; i < 10000; i++)
            {
                number = rand.Next(0, 20000);
                randomNumbers.Add(number);
            }
            textBox1.Text = string.Join(" ", randomNumbers);
        }
    }
}
