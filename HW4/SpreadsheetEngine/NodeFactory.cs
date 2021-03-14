using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The actual factory to create nodes. Will be unused until a later implementation.
    /// </summary>
    public class NodeFactory : ExpressionTreeNode
    {
        // member variables
        Dictionary<string, double> variables = new Dictionary<string, double>();
        private double result;

        /// <summary>
        /// Constructor that will set the result of the evaluation.
        /// </summary>
        /// <param name="postfixExpression"></param>
        public NodeFactory(string postfixExpression)
        {
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
            result = nodeStack.Peek().Evaluate(); // evaluates the tree and returns the result.
        }

        protected ConstantNode CreateConstantNode(double constant)
        {
            return new ConstantNode(constant);
        }

        protected VariableNode CreateVariableNode(string variableName)
        {
            return new VariableNode(variableName, ref variables);
        }

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

        public override double Evaluate()
        {
            return value;
        }
    }
}
