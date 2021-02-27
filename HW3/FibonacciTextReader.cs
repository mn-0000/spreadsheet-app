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

        /// <summary>
        /// Constructor, takes in an integer as the maximum number of numbers in the Fibonacci sequence that should be printed.
        /// </summary>
        /// <param name="maxNumLinesAvailable"> the maximum number of numbers in the Fibonacci sequence that should be printed </param>
        public FibonacciTextReader(int maxNumLinesAvailable)
        {
            upto = maxNumLinesAvailable;
        }

        /// <summary>
        /// Overrides original ReadLine() method for the FibonacciTextReader class.
        /// Calculates the next number in the Fibonacci sequence and returns it under the string format.
        /// </summary>
        /// <returns>
        /// the next number in the Fibonacci sequence, under the string format.
        /// </returns>
        public override string ReadLine()
        {
            if (callCount == upto) return null;
            nextFibonacciNumber = first + second;
            first = second;
            second = nextFibonacciNumber;
            callCount++;
            return nextFibonacciNumber.ToString();
        }

        /// <summary>
        /// Calls the overridden ReadLine() method, with the number of times being called indicated 
        /// by the instance of the FibonacciTextReader class.
        /// </summary>
        /// <returns> the concatenated string of all the ReadLine() calls. </returns>
        public override string ReadToEnd()
        {
            StringBuilder sb = new StringBuilder();
            // The string of the first two numbers of the sequence are hard-coded, and are returned appropriately if upto < 3.
            if (upto == 0) return sb.ToString();
            sb.AppendLine("1: 0");
            if (upto == 1) return sb.ToString();
            sb.AppendLine("2: 1");
            if (upto == 2) return sb.ToString();

            // Appends each return result of the ReadLine() method to the sb string.
            // starting int is to align with printed index.
            // since the printed index is 1-based, i < upto + 1.
            for (int i = 3; i < upto + 1; i++)
            {
                sb.AppendLine(i.ToString() + ": " + ReadLine());
            }

            return sb.ToString();
        }
    }
}
