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
        private List<int> testSet;
        [SetUp]
        public void SetUp()
        {
            testSet = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                testSet.Add(i);
            }
            testSet.Add(5);
            testSet.Add(7);
        }

        [Test]
        public void TestDistinctHashMap()
        {
            Assert.AreEqual(testSet.Distinct().Count(), Form1.RemoveDuplicatesHashMap(testSet));
        }

        [Test]
        public void TestDistinctO1Auxiliary()
        {
            Assert.AreEqual(testSet.Distinct().Count(), Form1.RemoveDuplicatesO1Auxiliary(testSet));
        }
    }
}
