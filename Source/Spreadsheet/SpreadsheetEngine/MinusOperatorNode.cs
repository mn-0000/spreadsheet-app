using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The class for the minus operator.
    /// </summary>
    public class MinusOperatorNode : OperatorNode
    {
        public static char Operator => '-';
        public static ushort Precedence => 7;

        public static Associative Associativity => Associative.Left;

        public override double Evaluate()
        {
            return Left.Evaluate() - Right.Evaluate();
        }
    }
}
