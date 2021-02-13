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
        /// <summary>
        /// Sets up an array filled with all integers from 0 to 9, 
        /// then add some duplicate values to see if the method can remove them properly.
        /// </summary>
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

        /// <summary>
        /// Test class for the TestDistinctHashMap() method.
        /// </summary>
        [Test]
        public void TestDistinctHashMap()
        {
            Assert.AreEqual(testSet.Distinct().Count(), Form1.RemoveDuplicatesHashMap(testSet));
        }

        /// <summary>
        /// Test class for the TestDistinctRestrictedSpace() method.
        /// </summary>
        [Test]
        public void TestDistinctRestrictedSpace()
        {
            Assert.AreEqual(testSet.Distinct().Count(), Form1.RemoveDuplicatesRestrictedSpace(testSet));
        }

        /// <summary>
        /// Test class for the TestDistinctSorted() method.
        /// </summary>
        [Test]
        public void TestDistinctSorted()
        {
            Assert.AreEqual(testSet.Distinct().Count(), Form1.RemoveDuplicatesSorted(testSet));
        }

        /// <summary>
        /// Test class for the TestDistinctSorted() method, in which there's a duplicate value at the end of the list.
        /// </summary>
        [Test]
        public void TestDistinctSortedDuplicateAtEnd()
        {
            testSet.Add(9);
            Assert.AreEqual(testSet.Distinct().Count(), Form1.RemoveDuplicatesSorted(testSet));
        }
    }
}
