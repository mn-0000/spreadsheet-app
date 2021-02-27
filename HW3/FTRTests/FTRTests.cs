// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace HW3.Tests
{
    [TestFixture]
    public class FTRTests
    {
        FibonacciTextReader ftr = new FibonacciTextReader(50);

        [SetUp]
        public void SetUp() {
                   
        }

        /// <summary>
        /// Test method for the ReadToEnd method in FibonacciTextReader class, up to the 50th number.
        /// </summary>
        [Test]
        public void TestFibReadToEnd50()
        {
            
            Assert.AreEqual(ftr.ReadToEnd().Length, 556);
        }

        /// <summary>
        /// Test method for the ReadToEnd method in FibonacciTextReader class, up to the 100th number.
        /// </summary>
        [Test]
        public void TestFibReadToEnd100()
        {
            FibonacciTextReader ftr2 = new FibonacciTextReader(100);
            Assert.AreEqual(ftr2.ReadToEnd().Length, 1643);
        }

        /// <summary>
        /// Test method for the ReadToEnd method in FibonacciTextReader class, up to the 1st and 2nd number, respectively.
        /// </summary>
        [Test]
        public void TestFibReadToEndLessThan3()
        {
            FibonacciTextReader ftr3 = new FibonacciTextReader(1);
            FibonacciTextReader ftr4 = new FibonacciTextReader(2);
            FibonacciTextReader ftr5 = new FibonacciTextReader(0);
            Assert.AreEqual(ftr3.ReadToEnd().Length, 6);
            Assert.AreEqual(ftr4.ReadToEnd().Length, 12);
            Assert.AreEqual(ftr5.ReadToEnd().Length, 0);
        }

        /// <summary>
        /// Test method for the ReadFile method in FibonacciTextReader class.
        /// </summary>
        public void TestFibReadLine()
        {
            Assert.AreEqual(ftr.ReadLine(), "1");
        }
    }
}
