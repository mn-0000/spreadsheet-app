using System;


namespace BST_Number_List__HW1_
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        static void Main()
        {
            Console.WriteLine("Enter a collection of numbers in the range [0, 100], separated by spaces: ");
            string userInput = Console.ReadLine();
            string[] numberCollection = userInput.Split(' ');
            foreach (string number in numberCollection)
            {
                Console.WriteLine(number);
            }
            Console.WriteLine("Program finished. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
