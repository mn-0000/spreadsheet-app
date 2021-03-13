using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class OperatorNodeFactory : OperatorNode
    {
        public OperatorNode CreateOperatorNode(char c)
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
            return 0;
        }
    }
}
