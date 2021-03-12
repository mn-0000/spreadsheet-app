// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace CptS321.Tests
{
    [TestFixture]
    public class ExpressionTreeTests
    {
        /// <summary>
        /// Test method for the ExpressionTree class, which compares the results of the expressions
        /// with only '+' operators to the expected values.
        /// </summary>
        [Test]
        public void TestEvaluatePlus()
        {
            ExpressionTree exp1 = new ExpressionTree("10+8");
            ExpressionTree exp2 = new ExpressionTree("15+20+5");
            ExpressionTree exp3 = new ExpressionTree("hi+10");
            ExpressionTree exp4 = new ExpressionTree("hello+world");
            ExpressionTree exp5 = new ExpressionTree("B3+nice+3");
            Assert.AreEqual(exp1, 18.0);
            Assert.AreEqual(exp2, 40.0);
            Assert.AreEqual(exp3, 10.0);
            Assert.AreEqual(exp4, 0.0);
            Assert.AreEqual(exp5, 3.0);
        }

        /// <summary>
        /// Test method for the ExpressionTree class, which compares the results of the expressions
        /// with only '-' operators to the expected values.
        /// </summary>
        [Test]
        public void TestEvaluateMinus()
        {
            ExpressionTree exp6 = new ExpressionTree("8-3");
            ExpressionTree exp7 = new ExpressionTree("24-5-19");
            ExpressionTree exp8 = new ExpressionTree("16-17");
            ExpressionTree exp9 = new ExpressionTree("hello-world");
            ExpressionTree exp10 = new ExpressionTree("A1-bye-2");
            Assert.AreEqual(exp6, 5.0);
            Assert.AreEqual(exp7, 0.0);
            Assert.AreEqual(exp8, -1.0);
            Assert.AreEqual(exp9, 0.0);
            Assert.AreEqual(exp10, -2.0);
        }

        /// <summary>
        /// Test method for the ExpressionTree class, which detects whether the code would throw
        /// a NotSupportedException when the user includes an operator that's not supported.
        /// </summary>
        public void TestUnsupportedOperator()
        {
            ExpressionTree exp11 = new ExpressionTree("10%3");
            Assert.That(() => exp11, Throws.TypeOf<System.NotSupportedException>());
        }

        /// <summary>
        /// Test method for the ExpressionTree class, which determines if the code will evaluate
        /// to infinity if the result goes over the double data type's MaxValue.
        /// </summary>
        public void TestInfinity()
        {
            var halfMaxValue = (double.MaxValue / 2.0).ToString("F", CultureInfo.InvariantCulture);
            Assert.True(double.IsInfinity(new ExpressionTree($"{halfMaxValue}+{halfMaxValue}").Evaluate()));
        }
    }
}
