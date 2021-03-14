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
            Assert.AreEqual(18.0, exp1.Evaluate());
            Assert.AreEqual(40.0, exp2.Evaluate());
            Assert.AreEqual(10.0, exp3.Evaluate());
            Assert.AreEqual(0.0, exp4.Evaluate());
            Assert.AreEqual(3.0, exp5.Evaluate());
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
            Assert.AreEqual(5.0, exp6.Evaluate());
            Assert.AreEqual(0.0, exp7.Evaluate());
            Assert.AreEqual(-1.0, exp8.Evaluate());
            Assert.AreEqual(0.0, exp9.Evaluate());
            Assert.AreEqual(-2.0, exp10.Evaluate());
        }

        [Test]
        public void TestEvaluateMultiply()
        {
            ExpressionTree exp11 = new ExpressionTree("7*8");
            ExpressionTree exp12 = new ExpressionTree("3*4*5*2");
            ExpressionTree exp13 = new ExpressionTree("3*0");
            ExpressionTree exp14 = new ExpressionTree("one*two");
            ExpressionTree exp15 = new ExpressionTree("A3*four*5");
            Assert.AreEqual(56.0, exp11.Evaluate());
            Assert.AreEqual(120.0, exp12.Evaluate());
            Assert.AreEqual(0.0, exp13.Evaluate());
            Assert.AreEqual(0.0, exp14.Evaluate());
            Assert.AreEqual(0.0, exp15.Evaluate());
        }

        [Test]
        public void TestEvaluateDivide()
        {
            ExpressionTree exp16 = new ExpressionTree("26/4");
            ExpressionTree exp17 = new ExpressionTree("5/8");
            ExpressionTree exp18 = new ExpressionTree("0/99");
            ExpressionTree exp19 = new ExpressionTree("44/0");
            ExpressionTree exp20 = new ExpressionTree("x/y");
            ExpressionTree exp21 = new ExpressionTree("z/4");
            Assert.AreEqual(6.5, exp16.Evaluate());
            Assert.AreEqual(0.625, exp17.Evaluate());
            Assert.AreEqual(0.0, exp18.Evaluate());
            Assert.AreEqual(double.PositiveInfinity, exp19.Evaluate());
            Assert.AreEqual(double.PositiveInfinity, exp20.Evaluate());
            Assert.AreEqual(0.0, exp21.Evaluate());
        }

        /// <summary>
        /// Test method for the ExpressionTree class, which detects whether the code would throw
        /// a NotSupportedException when the user includes an operator that's not supported.
        /// </summary>
        [Test]
        public void TestUnsupportedOperator()
        {
            ExpressionTree exp0 = new ExpressionTree("10%3");
            Assert.That(() => exp0, Throws.TypeOf<System.NotSupportedException>());
        }

        /// <summary>
        /// Test method for the ExpressionTree class, which determines if the code will evaluate
        /// to infinity if the result goes over the double data type's MaxValue.
        /// </summary>
        [Test]
        public void TestInfinity()
        {
            var halfMaxValue = (double.MaxValue / 2.0).ToString("F", CultureInfo.InvariantCulture);
            Assert.True(double.IsInfinity(new ExpressionTree($"{halfMaxValue}+{halfMaxValue}").Evaluate()));
        }

        [Test]
        public void TestEvaluationWithVariableSet()
        {
            ExpressionTree exp22 = new ExpressionTree("4/A1");
            exp22.SetVariable("A1", 16.0);
            Assert.AreEqual(0.25, exp22.Evaluate());
        }

        [Test]
        public void TestEvaluationOverlappingVariables()
        {
            ExpressionTree exp23 = new ExpressionTree("A1/A2");
            exp23.SetVariable("A1", 16.0);
            exp23.SetVariable("A2", 4.0);
            Assert.AreEqual(4.0, exp23.Evaluate());
            ExpressionTree exp24 = new ExpressionTree("A1/6");
            exp24.SetVariable("A1", 6.0);
            Assert.AreEqual(1.0, exp24.Evaluate());
        }
    }
}
