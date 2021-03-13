using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class ExpressionTree : OperatorNodeFactory
    {
        public ExpressionTree(string expression)
        {
            //string[] parameters = expression.Split(new char[] { '+', '-' });
            //Stack<char> expressionStack = new Stack<char>();
            //foreach (char c in expression)
            //{
            //    if (!Char.IsLetterOrDigit(c))
            //    {
            //        expressionStack.Push(c);
            //    }
            //}
            //foreach (string parameter in parameters)
            //{
            //    if (double.TryParse(parameter, out double number))
            //    {
            //        new ConstantNode(Convert.ToDouble(parameter));
            //    }
            //    else
            //    {
            //        new VariableNode(parameter);
            //    }
            //}

            //var splitExpression = expression.Split(new char[] { '+', '-' });
            //foreach (string s in splitExpression) Console.WriteLine(s);
            ConvertToPostFix(expression);
        }

        public void SetVariable(string variableName, double variableValue)
        {
            variableName = variableValue.ToString();
        }

        public double Evaluate()
        {

            return 0;
        }

        private string ConvertToPostFix(string expression)
        {
            Stack<char> operatorStack = new Stack<char>();
            StringBuilder postfixExpression = new StringBuilder();
            foreach (char c in expression)
            {
                if (!"()+-*/".Contains(c.ToString())) postfixExpression.Append(c);
                else
                {
                    postfixExpression.Append(" ");
                    if (c == '(') operatorStack.Push(c);
                    else if (c == ')') ;
                    else
                    {
                        if (operatorStack.Count == 0 || operatorStack.Peek() == '(') operatorStack.Push(c);

                        else if (Precedence(c) > Precedence(operatorStack.Peek())) operatorStack.Push(c);
                        //|| (Precedence(c) == Precedence(operatorStack.Peek()) && CreateOperatorNode(c).Associativity = Right) - condition for next HW
                        else if (Precedence(c) <= Precedence(operatorStack.Peek()))
                        {
                            while (operatorStack.Count != 0 && Precedence(c) <= Precedence(operatorStack.Peek())) postfixExpression.Append(operatorStack.Pop() + " ");
                            operatorStack.Push(c);
                        }
                    }
                }
            }
            while (operatorStack.Count > 0)
            {
                postfixExpression.Append(" " + operatorStack.Pop());
            }
            return postfixExpression.ToString();
        }

        private int Precedence(char symbol)
        {
            switch (symbol)
            {
                case '+':
                    return 7;
                case '-':
                    return 7;
                case '*':
                    return 6;
                case '/':
                    return 6;
                default:
                    throw new NotSupportedException("This operator (" + symbol + ") is not supported.");
            }
        }
   
    }
}
