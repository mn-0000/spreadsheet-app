using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class ExpressionTree
    {
        private string rawExpression;
        private Dictionary<string, double> variables = new Dictionary<string, double>();
        

        public ExpressionTree(string expression)
        {
            this.rawExpression = expression;
            
        }

        public string RawExpression
        {
            get { return rawExpression; }
        }

        public void SetVariable(string variableName, double variableValue)
        {
            variableName = variableValue.ToString();
        }

        public double Evaluate()
        {
            string postfixExpression = ConvertToPostFix(rawExpression);
            Stack<ExpressionTreeNode> nodeStack = new Stack<ExpressionTreeNode>();
            ConstantNode cNode;
            VariableNode vNode;
            OperatorNode oNode;
            double test = 0;
            string[] treeComponents = postfixExpression.Split(' ');
            foreach (string component in treeComponents)
            {
                if (double.TryParse(component, out test))
                {
                    cNode = CreateConstantNode(Convert.ToDouble(component));
                    nodeStack.Push(cNode);
                }
                else if (!double.TryParse(component, out test) && !"+-*/".Contains(component))
                {
                    vNode = CreateVariableNode(component);
                    nodeStack.Push(vNode);
                }
                else
                {
                    oNode = CreateOperatorNode(char.Parse(component));
                    oNode.Right = nodeStack.Pop();
                    oNode.Left = nodeStack.Pop();
                    nodeStack.Push(oNode);
                }
            }
            return nodeStack.Peek().Evaluate();
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

        private ConstantNode CreateConstantNode(double constant)
        {
            return new ConstantNode(constant);
        }

        private VariableNode CreateVariableNode(string variableName)
        {
            return new VariableNode(variableName, ref variables);
        }

        public OperatorNode CreateOperatorNode(char c)
        {
            switch (c)
            {
                case '+':
                    return new PlusOperatorNode();
                case '-':
                    return new MinusOperatorNode();
                case '*':
                    return new MultiplyOperatorNode();
                case '/':
                    return new DivideOperatorNode();
                default:
                    throw new NotSupportedException("This operator (" + c + ") is currently not supported");
            }
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
