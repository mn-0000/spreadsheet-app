using System;
using System.Linq;

namespace HW1
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            // Prompts for user input.
            Console.WriteLine("Enter a collection of unique numbers in the range [0, 100], separated by spaces: ");
            string userInput = Console.ReadLine();
            string[] numberCollectionAsString = userInput.Split(' ');

            // Checks for any duplicates that the user may have entered, then prompts them to reenter if duplicate(s) were found.
            while (numberCollectionAsString.Length != numberCollectionAsString.Distinct().Count())
            {
                Console.WriteLine("Contained duplicates, please try again.");
                Console.WriteLine("Enter a collection of unique numbers in the range [0, 100], separated by spaces: ");
                userInput = Console.ReadLine();
                numberCollectionAsString = userInput.Split(' ');
            }

            // Converts array of strings into array of ints.
            int[] numberCollection = Array.ConvertAll(numberCollectionAsString, int.Parse);

            // Creates an instance of BST, then add elements from array of ints into BST.
            BinarySearchTree sortedCollection = new BinarySearchTree();
            foreach (int number in numberCollection)
            {
                sortedCollection.Insert(number);
            }

            // Prints elements in BST in sorted order.
            Console.Write("The elements in the tree are: ");
            BinarySearchTree.PrintSortedBST();

            // Tree statistics.
            Console.WriteLine("\nTree statistics:");
            Console.Write("    Number of elements: ");
            int numberOfElements = sortedCollection.Count();
            Console.WriteLine(numberOfElements);
            Console.Write("    Number of levels: ");
            Console.WriteLine(sortedCollection.DetermineLevel());
            Console.Write("    Minimum number of levels for a tree with " + numberOfElements + " nodes: ");
            Console.WriteLine(1 + Math.Floor(Math.Log(numberOfElements, 2)));

            // Prompts user that the program has finished running.
            Console.WriteLine("Program finished. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
