using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class NodeFactory : ExpressionTreeNode
    {
        Dictionary<string, double> variables = new Dictionary<string, double>();
        private double value;

        public NodeFactory(string postfixExpression)
        {
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
                } else if (!double.TryParse(component, out test) && !component.Contains("+-*/"))
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
            value = nodeStack.Pop().Evaluate();
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
