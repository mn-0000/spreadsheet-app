using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The class for the multiply operator.
    /// </summary>
    public class MultiplyOperatorNode : OperatorNode
    {
        public static char Operator => '*';
        public static ushort Precedence => 6;

        public static Associative Associativity => Associative.Left;

        public override double Evaluate()
        {
            return Left.Evaluate() * Right.Evaluate();
        }
    }
}
