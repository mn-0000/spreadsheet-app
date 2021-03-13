using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    class ConstantNode : ExpressionTreeNode
    {
        private readonly double value;

        public ConstantNode(double value)
        {
            this.value = value;
        }

        public override double Evaluate()
        {
            return value;
        }
    } 
}
