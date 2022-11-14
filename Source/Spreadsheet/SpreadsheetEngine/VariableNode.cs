using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The class for the variable node.
    /// </summary>
    public class VariableNode : ExpressionTreeNode
    {
        private readonly string name;
        private Dictionary<string, double> variables;

        /// <summary>
        /// Constructor, that sets the name of the variable node and the reference to the dictionary.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variables"></param>
        public VariableNode(string name, ref Dictionary<string, double> variables)
        {
            this.name = name;
            this.variables = variables;
        }

        /// <summary>
        /// Checks to see if there is a value associated with the provided name in the dictionary.
        /// If there is, returns the value. If there isn't, sets the variable's value to 0.0.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            if (this.variables.ContainsKey(this.name))
            {
                return this.variables[this.name];
            }
            else
            {
                return double.NaN;
            }
        }

    }
}
