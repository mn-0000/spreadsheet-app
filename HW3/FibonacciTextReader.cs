// Minh The Nguyen - 011741737
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HW3
{
    class FibonacciTextReader : TextReader
    {
        int upto;
        int callCount = 0;
        BigInteger first = new BigInteger(0);
        BigInteger second = new BigInteger(1);
        BigInteger nextFibonacciNumber = new BigInteger(0);
        
        public FibonacciTextReader(int maxNumLinesAvailable)
        {
            upto = maxNumLinesAvailable;
        }

        public override string ReadLine()
        {
            return null;
        }

        public override string ReadToEnd()
        {
            return null;
        }
    }
}
