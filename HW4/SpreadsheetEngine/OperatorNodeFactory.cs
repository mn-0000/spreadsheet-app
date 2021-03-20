using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CptS321
{
    /// <summary>
    /// The actual factory to create nodes. Will be unused until a later implementation.
    /// </summary>
    public class NodeFactory
    {
        // member variables
        private Dictionary<char, Type> operators = new Dictionary<char, Type>();
        internal Dictionary<string, double> variables = new Dictionary<string, double>();

        /// <summary>
        /// Constructor that will set the result of the evaluation.
        /// </summary>
        /// <param name="postfixExpression"></param>
        public NodeFactory()
        {
            TraverseAvailableOperators((op, type) => operators.Add(op, type));
        }

        private delegate void OnOperatorNode(char op, Type type);

        private void TraverseAvailableOperators(OnOperatorNode onNode)
        {
            Type operatorNodeType = typeof(OperatorNode);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<Type> operatorTypes = assembly.GetTypes()
                    .Where(type => type.IsSubclassOf(operatorNodeType));

                foreach (var type in operatorTypes)
                {
                    PropertyInfo operatorField = type.GetProperty("Operator");
                    if (operatorField != null)
                    {
                        object value = operatorField.GetValue(type);

                        if (value is char)
                        {
                            char operatorSymbol = (char)value;
                            onNode(operatorSymbol, type);
                        }
                    }
                }
            }
        }
        
        public List<char> GetSupportedOperators()
        {
            return new List<char>(operators.Keys);
        }


        public OperatorNode CreateOperatorNode(char op)
        {
            if (IsOperator(op))
            {
                object operatorNodeObject = System.Activator.CreateInstance(operators[op]);
                if (operatorNodeObject is OperatorNode)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }
            throw new NotSupportedException("One or more operators is not supported yet.");
        }

        public ushort GetPrecedence(char op)
        {
            ushort precedenceLevel = 0;
            if (IsOperator(op))
            {
                Type type = operators[op];
                PropertyInfo propertyInfo = type.GetProperty("Precedence");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type);
                    if (propertyValue is ushort)
                    {
                        precedenceLevel = (ushort)propertyValue;
                    }
                }
            }
            return precedenceLevel;
        }

        public OperatorNode.Associative GetAssociativity(char op)
        {
            OperatorNode.Associative associativity = OperatorNode.Associative.Right;
            if (IsOperator(op))
            {
                Type type = operators[op];
                PropertyInfo propertyInfo = type.GetProperty("Associativity");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type);
                    if (propertyValue is OperatorNode.Associative)
                    {
                        associativity = (OperatorNode.Associative)propertyValue;
                    }
                }
            }
            return associativity;
        }

        public bool IsOperator(char c)
        {
            return operators.ContainsKey(c);
        }

        /// <summary>
        /// Creates a new constant node.
        /// </summary>
        /// <param name="constant"> the incoming constant </param>
        /// <returns> the new ConstantNode </returns>
        internal ConstantNode CreateConstantNode(double constant)
        {
            return new ConstantNode(constant);
        }

        /// <summary>
        /// Creates a new variable node.
        /// </summary>
        /// <param name="variableName"> the name of the variable </param>
        /// <returns> the new VariableNode </returns>
        internal VariableNode CreateVariableNode(string variableName)
        {
            return new VariableNode(variableName, ref variables);
        }
    }
}
