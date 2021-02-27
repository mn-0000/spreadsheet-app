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
    public class FibonacciTextReader : TextReader
    {
        int upto;
        int callCount = 0; //Counts number of times ReadLine() has been called, to return null afterwards.

        // BigIntegers needed for Fibonacci algorithm
        BigInteger first = new BigInteger(0);
        BigInteger second = new BigInteger(1);
        BigInteger nextFibonacciNumber = new BigInteger(0);
        
        public FibonacciTextReader(int maxNumLinesAvailable)
        {
            upto = maxNumLinesAvailable;
        }

        /// <summary>
        /// Overrides original ReadLine() method for the FibonacciTextReader class.
        /// </summary>
        /// <returns></returns>
        public override string ReadLine()
        {
            if (callCount == upto) return null;
            nextFibonacciNumber = first + second;
            first = second;
            second = nextFibonacciNumber;
            callCount++;
            return nextFibonacciNumber.ToString();
        }


        public override string ReadToEnd()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1: 0");
            sb.AppendLine("2: 1");
            
            for (int i = 3; i < upto + 1; i++)
            {
                sb.AppendLine(i.ToString() + ": " + ReadLine());
            }

            return sb.ToString();
        }
    }
}
