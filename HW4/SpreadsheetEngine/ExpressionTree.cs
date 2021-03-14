using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The class for the expression tree.
    /// </summary>
    public class ExpressionTree
    {
        // member variables
        private string rawExpression;
        private Dictionary<string, double> variables = new Dictionary<string, double>();
        
        /// <summary>
        /// Constructor for the ExpressionTree class. Sets the rawExpression field and clears the variable dictionary.
        /// </summary>
        /// <param name="expression"> the expression itself. </param>
        public ExpressionTree(string expression)
        {
            this.rawExpression = expression;
            this.variables.Clear();
        }

        public string RawExpression
        {
            get { return rawExpression; }
        }

        /// <summary>
        /// Assigns the provided value to a variable with the provided name, and adds the variable to the dictionary.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="variableValue"></param>
        public void SetVariable(string variableName, double variableValue)
        {
            variables.Add(variableName, variableValue);
        }

        /// <summary>
        /// Evaluates the expression.
        /// </summary>
        /// <returns> the result of the evaluation </returns>
        public double Evaluate()
        {
            // converts the tree's raw expression to postfix format.
            string postfixExpression = ConvertToPostFix(rawExpression);
            
            // needed variables
            Stack<ExpressionTreeNode> nodeStack = new Stack<ExpressionTreeNode>();
            ConstantNode constantNode;
            VariableNode variableNode;
            OperatorNode operatorNode;
            double test = 0;

            string[] treeComponents = postfixExpression.Split(' '); // extracts all elements of the postfix expression and add them to a string array.
            foreach (string component in treeComponents)
            {
                // if the incoming string is a double, create a new constant node and push it to the stack.
                if (double.TryParse(component, out test))
                {
                    constantNode = CreateConstantNode(Convert.ToDouble(component));
                    nodeStack.Push(constantNode);
                }
                // else if the incoming string is a variable name, create a new variable node and push it to the stack.
                else if (!double.TryParse(component, out test) && !"+-*/%^".Contains(component))
                {
                    variableNode = CreateVariableNode(component);
                    nodeStack.Push(variableNode);
                }
                // else if the incoming string is an operator, create a new corresponding operator node, pops the last 2 nodes and have them
                // as the operator's children.
                else
                {
                    operatorNode = CreateOperatorNode(char.Parse(component));
                    operatorNode.Right = nodeStack.Pop();
                    operatorNode.Left = nodeStack.Pop();
                    nodeStack.Push(operatorNode);
                }
            }
            return nodeStack.Peek().Evaluate(); // evaluates the tree and returns the result.
        }

        /// <summary>
        /// Converts an in-fix expression to a post-fix expression.
        /// </summary>
        /// <param name="expression"> the expression to be converted </param>
        /// <returns> the given expression, in post-fix format </returns>
        private string ConvertToPostFix(string expression)
        {
            Stack<char> operatorStack = new Stack<char>(); // creates a stack to store operators
            StringBuilder postfixExpression = new StringBuilder(); // creates an amendable string
            foreach (char c in expression)
            {
                // if the character is not one of the operators, add it to the postfix string.
                if (!"()+-*/%^".Contains(c.ToString())) postfixExpression.Append(c);
                // if the character is one of the operators, execute the following block of code.
                else
                {
                    postfixExpression.Append(" "); //marks the end of the string representing a constant or variable.
                    if (c == '(') operatorStack.Push(c); // if the operator is a left parenthesis, push to operator stack (should not be used for HW4)
                    else if (c == ')') ; // if the operator is a right parenthesis, discard it (should not be used for HW4)
                    else
                    {
                        // if there operator stack is empty, or there's a left parenthesis at the top of the stack, push this operator to the stack.
                        if (operatorStack.Count == 0 || operatorStack.Peek() == '(') operatorStack.Push(c);

                        // else if the precedence of the incoming operator is higher than that of the operator at the top of the stack, push the operator to the stack.
                        else if (Precedence(c) > Precedence(operatorStack.Peek())) operatorStack.Push(c);
                        //|| (Precedence(c) == Precedence(operatorStack.Peek()) && CreateOperatorNode(c).Associativity = Right) - condition for next HW

                        // else if the precedence of the incoming operator is lower than or equal to that of the operator at the top of the stack
                        else if (Precedence(c) <= Precedence(operatorStack.Peek()))
                        {
                            // while there are still operators in the stack, and the precedence of the incoming operator is lower than 
                            // or equal to that of the operator at the top of the stack, pop the operator at the top of the stack off 
                            // and add it to the postfix string along with a whitespace.
                            while (operatorStack.Count != 0 && Precedence(c) <= Precedence(operatorStack.Peek())) postfixExpression.Append(operatorStack.Pop() + " ");
                            // when there are no operators left on the stack, push the incoming operator to stack.
                            operatorStack.Push(c);
                        }
                    }
                }
            }
            // Upon finishing processing the raw expression, any operators left on the stack will be popped, then add a whitespace 
            // along with the popped operator to the postfix string.
            while (operatorStack.Count > 0)
            {
                postfixExpression.Append(" " + operatorStack.Pop());
            }
            // converts the postfix string to string format, and return it.
            return postfixExpression.ToString();
        }

        /// <summary>
        /// Determines the precedence of the operators. Any unsupported operators will throw a NotSupportedException.
        /// This method will exist until later homeworks that require precedence and associativity.
        /// </summary>
        /// <param name="symbol"> the operator to determine precedence of </param>
        /// <returns> the precedence value of the operator. </returns>
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

        /// <summary>
        /// Creates a new constant node.
        /// </summary>
        /// <param name="constant"> the incoming constant </param>
        /// <returns> the new ConstantNode </returns>
        protected ConstantNode CreateConstantNode(double constant)
        {
            return new ConstantNode(constant);
        }

        /// <summary>
        /// Creates a new variable node.
        /// </summary>
        /// <param name="variableName"> the name of the variable </param>
        /// <returns> the new VariableNode </returns>
        protected VariableNode CreateVariableNode(string variableName)
        {
            return new VariableNode(variableName, ref variables);
        }

        /// <summary>
        /// Creates a new operator node. Any unsupported operators will throw a NotSupportedException.
        /// </summary>
        /// <param name="c"> the provided operator </param>
        /// <returns> the new operator node dependent on which operator it was </returns>
        protected OperatorNode CreateOperatorNode(char c)
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
    }
}
