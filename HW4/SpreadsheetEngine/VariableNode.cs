using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class VariableNode : ExpressionTreeNode
    {
        private readonly string name;
        private Dictionary<string, double> variables;

        public VariableNode(string name, ref Dictionary<string, double> variables)
        {
            this.name = name;
            this.variables = variables;
            //variables.Add(name, 0.0);
        }

        public override double Evaluate()
        {
            double value = 0.0;
            if (this.variables.ContainsKey(this.name))
            {
                value = this.variables[this.name];
            }
            return value;
        }

        //public VariableNode CreateVariableNode(string variableName)
        //{
        //    return new VariableNode(variableName, ref variables);
        //}
    }
}
