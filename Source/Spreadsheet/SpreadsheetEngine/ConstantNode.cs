using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The class for the constant node.
    /// </summary>
    public class ConstantNode : ExpressionTreeNode
    {
        private readonly double value;

        /// <summary>
        /// Constructor that sets the value for the constant node.
        /// </summary>
        /// <param name="value"> the incoming value </param>
        public ConstantNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Returns the value for the constant node.
        /// </summary>
        /// <returns> the value for the constant node </returns>
        public override double Evaluate()
        {
            return value;
        }
    } 
}
