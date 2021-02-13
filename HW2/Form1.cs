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
            List<int> randomNumbers = new List<int>();
            int number;

            // Adds 10000 random numbers in the range [0, 20000] to the list.
            for (int i = 0; i < 10000; i++)
            {
                number = rand.Next(0, 20000);
                randomNumbers.Add(number);

            }

            // Shows results of the 3 sorting methods, as well as time complexity and explanation for the first one.
            textBox1.Text  = "1. HashSet method: ";
            textBox1.Text += RemoveDuplicatesHashMap(randomNumbers) + " unique numbers.";
            textBox1.Text += "\r\n        The time complexity of this method is O(n). Using the Stopwatch class,"
                           + "\r\n        I measured the time it takes for the method to execute, using several "
                           + "\r\n        list sizes increasing by a factor of 10. (The list needed to be sufficiently "
                           + "\r\n        large, otherwise it would simply show the elapsed time as 0 ms.) Based on "
                           + "\r\n        the results, the time it takes for the method to execute changed "
                           + "\r\n        proportionately to the list size, meaning that the method was in linear time.";
            textBox1.Text += "\r\n 2. O(1) auxiliary space method: ";
            textBox1.Text += RemoveDuplicatesO1Auxiliary(randomNumbers) + " unique numbers.";
        }

        public static int RemoveDuplicatesHashMap(List<int> list)
        {
            HashSet<int> distinctCollection = new HashSet<int>(list);
            return distinctCollection.Count();
        }

        public static int RemoveDuplicatesO1Auxiliary(List<int> list)
        {
            List<int> distinctCollection = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                bool isDuplicate = false;
                for (int j = 0; j < i; j++)
                {
                    if (list[j] == list[i])
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate)
                {
                    distinctCollection.Add(list[i]);
                }
            }
            return distinctCollection.Count;
        }
    }
}
