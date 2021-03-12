using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice = 0;
            ExpressionTree currentExpression = new ExpressionTree("A1+B1+C1");
            Program.MenuPrompt(currentExpression);
            string input = Console.ReadLine();
            
            while (!int.TryParse(input, out choice))
            {
                input = Console.ReadLine();
            }

            while (!(choice == 4))
            switch (choice)
            {
                case 1:
                    Console.Write("Enter the new expression: ");
                    // Console.ReadLine();
                    // TODO: Assign new expression to currentExpression
                    break;
                case 2:
                    Console.Write("Enter variable name: ");
                    // Console.ReadLine();
                    // TODO: checks if variable is in expression
                    Console.Write("Enter variable value: ");
                    // Console.ReadLine();
                    // TODO: Assigns value to variable
                    break;
                case 3:
                    // TODO: prints out evaluation results.
                    break;
                case 4:
                    Console.Write("Done");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
            };
        }

        private static void MenuPrompt(ExpressionTree expression)
        {
            Console.WriteLine("Menu (Current expression: " + expression);
            Console.WriteLine("1 - Enter a new expression");
            Console.WriteLine("2 - Set a variable value");
            Console.WriteLine("3 - Evaluate tree");
            Console.WriteLine("4 - Quit");
        }
    }
}
