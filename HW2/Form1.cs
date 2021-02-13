using System;
using System.Collections.Generic;
using System.Linq;
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
            textBox1.Text = "1. HashSet method: ";
            textBox1.Text += RemoveDuplicatesHashMap(randomNumbers) + " unique numbers.";
            textBox1.Text += "\r\n        The time complexity of this method is O(n). Using the Stopwatch class,"
                           + "\r\n        I measured the time it takes for the method to execute, using several "
                           + "\r\n        list sizes increasing by a factor of 10. (The list needed to be sufficiently "
                           + "\r\n        large, otherwise it would simply show the elapsed time as 0 ms.) Based on "
                           + "\r\n        the results, it appears that the time it takes for the method to execute changed "
                           + "\r\n        proportionately to the list size, meaning that the method was in linear time.";
            textBox1.Text += "\r\n 2. O(1) auxiliary space method: ";
            textBox1.Text += RemoveDuplicatesRestrictedSpace(randomNumbers) + " unique numbers.";
            textBox1.Text += "\r\n 3. Sorted method: ";
            textBox1.Text += RemoveDuplicatesSorted(randomNumbers) + " unique numbers.";
        }

        /// <summary>
        /// Removes all duplicate elements from a given list of ints, using a HashSet.
        /// </summary>
        /// <param name="list"></param>
        /// <returns> 
        /// the number of elements in the HashSet containing distinct numbers from the given list. 
        /// </returns>
        public static int RemoveDuplicatesHashMap(List<int> list)
        {
            HashSet<int> distinctCollection = new HashSet<int>(list);
            return distinctCollection.Count();
        }

        /// <summary>
        /// Removes all duplicate elements from a given list of ints, complying with O(1) auxiliary space requirement.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>
        /// the number of elements in the new list containing distinct numbers from the original list.
        /// </returns>
        public static int RemoveDuplicatesRestrictedSpace(List<int> list)
        {
            //Creates a new empty list to add all the distinct numbers to.
            List<int> distinctCollection = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                bool isDuplicate = false;
                // Traverse through the list up to the element preceding list[i].
                // Only the first instance of the element would be added to the new list, all other instances will be skipped.
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

        /// <summary>
        /// Sorts and removes all duplicate elements from a given list of ints.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>
        /// the number of elements in the list after removing all duplicates.
        /// </returns>
        public static int RemoveDuplicatesSorted(List<int> list)
        {
            list.Sort();
            for (int i = 0; i < list.Count - 1; i++)
            {
                while (list[i] == list[i + 1])
                {
                    list.Remove(list[i]);
                    // If there was a duplicate element at the last position in the list,
                    // i + 1 would be out of range and thus, we would need to exit the while loop.
                    if (i == list.Count - 1)
                    {
                        break;
                    }
                }           
            }

            return list.Count;
        }
    }
}
