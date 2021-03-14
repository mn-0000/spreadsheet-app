using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The class for the operator node.
    /// </summary>
    public abstract class OperatorNode : ExpressionTreeNode
    {
        public enum Associative
        {
            Right,
            Left
        }

        public ExpressionTreeNode Left { get; set; }
        public ExpressionTreeNode Right { get; set; }
    }
}
