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
        [Test]
        public void TestPostfixOutput()
        {
            ExpressionTree exp00 = new ExpressionTree("1 - 2 + 3");
            ExpressionTree exp000 = new ExpressionTree("(1-2) + 3-4/5");
            Assert.AreEqual("1 2 - 3 +", exp00.ConvertToPostFix(exp00.RawExpression));
            Assert.AreEqual("1 2 - 3 + 4 5 / -", exp000.ConvertToPostFix(exp000.RawExpression));
        }

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

        /// <summary>
        /// Test method for the ExpressionTree class, which compares the results of the expressions
        /// with only '*' operators to the expected values.
        /// </summary>
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

        /// <summary>
        /// Test method for the ExpressionTree class, which compares the results of the expressions
        /// with only '/' operators to the expected values.
        /// </summary>
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
            Assert.AreEqual(double.NaN, exp20.Evaluate());
            Assert.AreEqual(0.0, exp21.Evaluate());
        }

        /// <summary>
        /// Test method for the ExpressionTree class, which detects whether the code would throw
        /// a NotSupportedException when the user includes an operator that's not supported.
        /// </summary>
        [Test]
        public void TestUnsupportedOperator()
        {
            ExpressionTree exp0 = new ExpressionTree("10^3");
            Assert.That(() => exp0.Evaluate(), Throws.TypeOf<System.NotSupportedException>());
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

        /// <summary>
        /// Test method for the SetVariable method in the ExpressionTree class, which checks if the variable has been
        /// added to the dictionary and would cause the expression to evaluate to a new result.
        /// </summary>
        [Test]
        public void TestEvaluationWithVariableSet()
        {
            ExpressionTree exp22 = new ExpressionTree("4/A1");
            exp22.SetVariable("A1", 16.0);
            Assert.AreEqual(0.25, exp22.Evaluate());
        }

        /// <summary>
        /// Test method for the SetVariable method in the ExpressionTree class, which checks if the variable dictionary has been
        /// cleared prior to a new expression creation.
        /// </summary>
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

        [Test]
        public void TestEvaluationWithParentheses()
        {
            ExpressionTree exp25 = new ExpressionTree("(4+7)/(1-3)");
            ExpressionTree exp26 = new ExpressionTree("8+(4*3)");
            ExpressionTree exp27 = new ExpressionTree("A1*(A2-A3)-2");
            ExpressionTree exp28 = new ExpressionTree("A1/(A2+3)");
            Assert.AreEqual(-5.5, exp25.Evaluate());
            Assert.AreEqual(20.0, exp26.Evaluate());
            Assert.AreEqual(-2.0, exp27.Evaluate());
            Assert.AreEqual(0.0, exp28.Evaluate());
        } 
        
        [Test]
        public void TestEvaluationUnbalancedParentheses()
        {
            ExpressionTree exp29 = new ExpressionTree("((16/4)-2");
            Assert.That(() => exp29.Evaluate(), Throws.TypeOf<System.Exception>());
        }

        [Test]
        public void TestEvaluationWithWhitespace()
        {
            ExpressionTree exp30 = new ExpressionTree("8 - 5 * 7");
            ExpressionTree exp31 = new ExpressionTree("14-6* 2");
            ExpressionTree exp32 = new ExpressionTree("(X - 3) / (2 - 3)");
            Assert.AreEqual(-27.0, exp30.Evaluate());
            Assert.AreEqual(2.0, exp31.Evaluate());
            Assert.AreEqual(3.0, exp32.Evaluate());
        }

        [Test]
        public void TestCorrectPrecedence()
        {
            NodeFactory factory = new NodeFactory();
            Assert.AreEqual(7, factory.GetPrecedence('+'));
            Assert.AreEqual(7, factory.GetPrecedence('-'));
            Assert.AreEqual(6, factory.GetPrecedence('*'));
            Assert.AreEqual(6, factory.GetPrecedence('/'));
        }

        [Test]
        public void TestCorrectAssociativity()
        {
            NodeFactory factory = new NodeFactory();
            Assert.AreEqual(OperatorNode.Associative.Left, factory.GetAssociativity('+'));
            Assert.AreEqual(OperatorNode.Associative.Left, factory.GetAssociativity('-'));
            Assert.AreEqual(OperatorNode.Associative.Left, factory.GetAssociativity('*'));
            Assert.AreEqual(OperatorNode.Associative.Left, factory.GetAssociativity('/'));
        }
    }
}
