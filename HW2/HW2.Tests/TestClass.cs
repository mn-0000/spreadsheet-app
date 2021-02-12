// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HW2
{
    [TestFixture]
    public class TestClass
    {
        [SetUp]
        public void SetUp() {}

        [Test]
        public void TestDiscreteHashMap()
        {
            List<int> testSet = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                testSet.Add(i);
            }
            testSet.Add(5);
            testSet.Add(7);
            Assert.AreEqual(testSet.Distinct(), Form1.RemoveDuplicates<int>(testSet));
            
        }
    }
}
