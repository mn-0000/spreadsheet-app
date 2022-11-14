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
            double variableTest = 0;
            ExpressionTree currentExpression = new ExpressionTree("A1*(A2-A3)-2");
            do
            {
                Program.MenuPrompt(currentExpression.RawExpression);
                string input = Console.ReadLine();

                while (!int.TryParse(input, out choice))
                {
                    input = Console.ReadLine();
                }
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the new expression: ");
                        currentExpression = new ExpressionTree(Console.ReadLine());
                        break;
                    case 2:
                        Console.Write("Enter variable name: ");
                        string variableName = Console.ReadLine();
                        Console.Write("Enter variable value: ");
                        string variableValue = Console.ReadLine();
                        if (!double.TryParse(variableValue, out variableTest))
                        {
                            Console.WriteLine("Your input was invalid. Please try again!");
                            goto case 2;
                        }
                        currentExpression.SetVariable(variableName, Convert.ToDouble(variableValue));
                        break;
                    case 3:
                        double result = currentExpression.Evaluate();
                        Console.WriteLine(result.ToString());
                        break;
                    case 4:
                        Console.Write("Done");
                        Environment.Exit(0);
                        break;
                }
            } while (choice != 4);
        }

        /// <summary>
        /// Prints the menu out to the console.
        /// </summary>
        /// <param name="expression"> the expression to be displayed </param>
        private static void MenuPrompt(string expression)
        {
            Console.WriteLine("Menu (Current expression: " + expression + ")");
            Console.WriteLine("1 - Enter a new expression");
            Console.WriteLine("2 - Set a variable value");
            Console.WriteLine("3 - Evaluate tree");
            Console.WriteLine("4 - Quit");
        }
    }
}
