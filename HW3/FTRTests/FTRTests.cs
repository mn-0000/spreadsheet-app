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
        BigInteger first = new BigInteger(8);
        BigInteger second = new BigInteger(14);
        BigInteger nextFibonacciNumber = new BigInteger(0);

        [SetUp]
        public void SetUp() {
                   
        }
        [Test]
        public void TestFibReadLine()
        {
            Assert.AreEqual(ftr.ReadLine(), "22");
        }
    }
}
