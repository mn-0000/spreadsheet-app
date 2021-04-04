using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CptS321
{
    /// <summary>
    /// The actual factory to create nodes.
    /// </summary>
    public class NodeFactory
    {
        // member variables
        private Dictionary<char, Type> operators = new Dictionary<char, Type>();
        internal Dictionary<string, double> variables = new Dictionary<string, double>();

        /// <summary>
        /// Constructor that adds all supported operators to the dictionary of operators.
        /// </summary>
        public NodeFactory()
        {
            SearchAllAvailableOperators((op, type) => operators.Add(op, type));
        }

        private delegate void OnOperatorNode(char op, Type type); // event when an operator is detected

        /// <summary>
        /// Gets all available operator classes from the current namespace.
        /// </summary>
        /// <param name="onOperatorNode"> event when operator is detected. </param>
        private void SearchAllAvailableOperators(OnOperatorNode onOperatorNode)
        {
            Type operatorNodeType = typeof(OperatorNode); // get the operator type of operator

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // creates an iterable collection of all the currently supported operator types
                IEnumerable<Type> operatorTypes = assembly.GetTypes()
                    .Where(type => type.IsSubclassOf(operatorNodeType));

                foreach (var type in operatorTypes)
                {
                    PropertyInfo operatorField = type.GetProperty("Operator"); // get the operator property
                    if (operatorField != null)
                    {
                        object value = operatorField.GetValue(type); // get the operator

                        if (value is char)
                        {
                            char operatorSymbol = (char)value;
                            operators.Add(operatorSymbol, type); // signals the constructor to add the operator to the dictionary
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets all the currently supported operators.
        /// </summary>
        /// <returns> the list of all supported operators </returns>
        public List<char> GetSupportedOperators()
        {
            return new List<char>(operators.Keys);
        }

        /// <summary>
        /// Creates a new operator node based on the incoming operator.
        /// If the operator is not supported, throws a NotSupportedException.
        /// </summary>
        /// <param name="op"> the operator </param>
        /// <returns> the new operator node </returns>
        public OperatorNode CreateOperatorNode(char op)
        {
            if (IsOperator(op))
            {
                object operatorNodeObject = System.Activator.CreateInstance(operators[op]); // creates an instance of the operator
                if (operatorNodeObject is OperatorNode)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }
            throw new NotSupportedException("One or more operators is not supported yet.");
        }

        /// <summary>
        /// Gets the precedence level of the provided operator.
        /// </summary>
        /// <param name="op"> the operator to get the precedence from /param>
        /// <returns> the precedence level of the operator </returns>
        public ushort GetPrecedence(char op)
        {
            ushort precedenceLevel = 0;
            if (IsOperator(op))
            {
                Type type = operators[op];
                PropertyInfo propertyInfo = type.GetProperty("Precedence"); // get the precedence property
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type); // get the precedence level
                    if (propertyValue is ushort)
                    {
                        precedenceLevel = (ushort)propertyValue;
                    }
                }
            }
            return precedenceLevel;
        }

        /// <summary>
        /// Gets the associativity of an operator.
        /// </summary>
        /// <param name="op"> the operator to get the associativity from </param>
        /// <returns> the operator's associativity </returns>
        public OperatorNode.Associative GetAssociativity(char op)
        {
            OperatorNode.Associative associativity = OperatorNode.Associative.Right; // sets a default associativity
            if (IsOperator(op))
            {
                Type type = operators[op];
                PropertyInfo propertyInfo = type.GetProperty("Associativity"); // gets the associativity property
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type); // gets the associativity
                    if (propertyValue is OperatorNode.Associative)
                    {
                        associativity = (OperatorNode.Associative)propertyValue;
                    }
                }
            }
            return associativity;
        }

        /// <summary>
        /// Checks to see if the incoming character is a supported operator.
        /// </summary>
        /// <param name="c"> the character to check </param>
        /// <returns> a boolean value based on whether the character is a supported operator or not.</returns>
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
