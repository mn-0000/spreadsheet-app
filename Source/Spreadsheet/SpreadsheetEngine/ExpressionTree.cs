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

        // create an instance of the node factory.
        NodeFactory factory = new NodeFactory();
        /// <summary>
        /// Constructor for the ExpressionTree class. Sets the rawExpression field and clears the variable dictionary.
        /// </summary>
        /// <param name="expression"> the expression itself. </param>
        public ExpressionTree(string expression)
        {
            this.rawExpression = expression;
            factory.variables.Clear();
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
            factory.variables.Add(variableName, variableValue);
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
                if ("()".Contains(component)) throw new Exception("The number of left and right parentheses are not the same.");
                // if the incoming string is a double, create a new constant node and push it to the stack.
                if (double.TryParse(component, out test))
                {
                    constantNode = factory.CreateConstantNode(Convert.ToDouble(component));
                    nodeStack.Push(constantNode);
                }
                // else if the incoming string is a variable name, create a new variable node and push it to the stack.
                else if (!double.TryParse(component, out test) && !"+-*/%^".Contains(component))
                {
                    variableNode = factory.CreateVariableNode(component);
                    nodeStack.Push(variableNode);
                }
                // else if the incoming string is an operator, create a new corresponding operator node, pops the last 2 nodes and have them
                // as the operator's children.
                else
                {
                    operatorNode = factory.CreateOperatorNode(char.Parse(component));
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
        public string ConvertToPostFix(string expression)
        {
            Stack<char> operatorStack = new Stack<char>(); // creates a stack to store operators
            StringBuilder postfixExpression = new StringBuilder(); // creates an amendable string
            foreach (char c in expression)
            {
                if (!char.IsWhiteSpace(c)) // only execute the following block of code if the incoming character is not a whitespace
                {
                    // if the character is not one of the operators, add it to the postfix string.
                    if (!"()+-*/%^".Contains(c.ToString())) postfixExpression.Append(c);
                    // if the character is one of the operators, execute the following block of code.
                    else
                    {
                        if (c == '(') operatorStack.Push(c); // if the operator is a left parenthesis, push to operator stack 

                        // if the operator is a right parenthesis, continuously pop the operator at the top of the stack off 
                        // and add it to the postfix string along with a whitespace before it, until the top of the stack is
                        // a left parenthesis.
                        else if (c == ')')
                        {
                            while (operatorStack.Peek() != '(') postfixExpression.Append(" " + operatorStack.Pop());
                            operatorStack.Pop(); // discards the left parenthesis
                        }
                        else
                        {
                            postfixExpression.Append(" "); //marks the end of the string representing a constant or variable.
                            // if there operator stack is empty, or there's a left parenthesis at the top of the stack, push this operator to the stack.
                            if (operatorStack.Count == 0 || operatorStack.Peek() == '(') operatorStack.Push(c);

                            // else if the precedence of the incoming operator is higher than that of the operator at the top of the stack, 
                            // or if the operator has the same precedence as the operator on the top of the stack and is right associative, 
                            // push the operator to the stack.
                            else if (factory.GetPrecedence(c) < factory.GetPrecedence(operatorStack.Peek())
                                 || (factory.GetPrecedence(c) == factory.GetPrecedence(operatorStack.Peek()) && factory.GetAssociativity(c) == OperatorNode.Associative.Right))
                                operatorStack.Push(c);

                            // else if the precedence of the incoming operator is lower than or equal to that of the operator at the top of the stack, 
                            // or if the operator has the same precedence as the operator on the top of the stack and is left associative
                            else if (factory.GetPrecedence(c) >= factory.GetPrecedence(operatorStack.Peek())
                                 || (factory.GetPrecedence(c) == factory.GetPrecedence(operatorStack.Peek()) && factory.GetAssociativity(c) == OperatorNode.Associative.Left))
                            {
                                // while there are still operators in the stack, and the precedence of the incoming operator is lower than 
                                // or equal to that of the operator at the top of the stack, pop the operator at the top of the stack off 
                                // and add it to the postfix string along with a whitespace.
                                while (operatorStack.Count > 0)
                                {
                                    if (factory.GetPrecedence(c) >= factory.GetPrecedence(operatorStack.Peek())
                                    || (factory.GetPrecedence(c) == factory.GetPrecedence(operatorStack.Peek()) && factory.GetAssociativity(c) == OperatorNode.Associative.Left))
                                    {
                                        postfixExpression.Append(operatorStack.Pop() + " ");
                                    }
                                    else break;
                                }
                                // when there are no operators left on the stack, push the incoming operator to stack.
                                operatorStack.Push(c);
                            }
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
        /// Evaluates the formula given by a spreadsheet's cell array.
        /// </summary>
        /// <param name="cellArray"> The cell array of a spreadsheet </param>
        /// <param name="cellRowIndex"> The provided cell's row index </param>
        /// <param name="cellColumnIndex"> The provided cell's column index </param>
        /// <returns> The result of the evaluation </returns>
        public string EvaluateSpreadsheetFormula(Cell[,] cellArray, int cellRowIndex, int cellColumnIndex)
        {
            // needed variables
            string postfixExpression = ConvertToPostFix(rawExpression);
            Stack<ExpressionTreeNode> nodeStack = new Stack<ExpressionTreeNode>();
            ConstantNode constantNode;
            VariableNode variableNode;
            OperatorNode operatorNode;
            double test = 0;

            string[] treeComponents = postfixExpression.Split(' '); // extracts all elements of the postfix expression and add them to a string array.

            foreach (string component in treeComponents)
            {
                // If the incoming string is a parenthesis, the input is incorrect.
                if ("()".Contains(component)) { return "!(Unbalanced parentheses)"; }
                // if the incoming string is a double, create a new constant node and push it to the stack.
                if (double.TryParse(component, out test))
                {
                    constantNode = factory.CreateConstantNode(Convert.ToDouble(component));
                    nodeStack.Push(constantNode);
                }
                // else if the incoming string is a variable name, create a new variable node and push it to the stack.
                else if (!double.TryParse(component, out test) && !"+-*/%^".Contains(component))
                {
                    try
                    {
                        int rowNumber = Convert.ToInt32(component.Substring(1)) - 1;
                        int columnNumber = component[0] - 65;
                        // If the row and column number of the incoming cell reference matches that of the cell's,
                        // return an error message notifying the user of self-referencing.
                        if (rowNumber == cellRowIndex && columnNumber == cellColumnIndex)
                        {
                            return "!(Self-reference)";
                        }
                        else
                        {
                            string cellValue = cellArray[rowNumber, columnNumber].Value;
                            constantNode = factory.CreateConstantNode(Double.Parse(cellValue));
                            nodeStack.Push(constantNode);
                        }
                    }
                    // If the cell being referenced does not have a text value, or has a string value,
                    // create a constant node of 0 and push it to the node stack.
                    catch (ArgumentNullException)
                    {
                        constantNode = factory.CreateConstantNode(0);
                        nodeStack.Push(constantNode);
                    }
                    catch (FormatException)
                    {
                        constantNode = factory.CreateConstantNode(0);
                        nodeStack.Push(constantNode);
                    }
                    // If the incoming tree component is an invalid reference, return an error message notifying the user of such.
                    catch (IndexOutOfRangeException)
                    {
                        return "!(Invalid reference)";
                    }
                }
                // else if the incoming string is an operator, create a new corresponding operator node, pops the last 2 nodes and have them
                // as the operator's children.
                else
                {
                    operatorNode = factory.CreateOperatorNode(char.Parse(component));
                    operatorNode.Right = nodeStack.Pop();
                    operatorNode.Left = nodeStack.Pop();
                    nodeStack.Push(operatorNode);
                }
            }
            return nodeStack.Peek().Evaluate().ToString(); // evaluates the tree and returns the result.
        }
    }
}

