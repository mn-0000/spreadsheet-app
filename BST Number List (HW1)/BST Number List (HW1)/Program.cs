using System;
using BST_Number_List__HW1_;
using System.Collections.Generic;
using System.Linq;

namespace BST_Number_List__HW1_
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static void Main()
        {
            Console.WriteLine("Enter a collection of unique numbers in the range [0, 100], separated by spaces: ");
            string userInput = Console.ReadLine();
            string[] numberCollectionAsString = userInput.Split(' ');
            
            while (numberCollectionAsString.Length != numberCollectionAsString.Distinct().Count())
            {
                Console.WriteLine("Contained duplicates, please try again.");
                Console.WriteLine("Enter a collection of unique numbers in the range [0, 100], separated by spaces: ");
                userInput = Console.ReadLine();
                numberCollectionAsString = userInput.Split(' ');
            }

            int[] numberCollection = Array.ConvertAll(numberCollectionAsString, int.Parse);

            Console.WriteLine("Program finished. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
