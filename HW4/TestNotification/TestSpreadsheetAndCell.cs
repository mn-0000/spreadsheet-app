// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CptS321.Tests
{
    [TestFixture]
    public class TestClass
    {
        Spreadsheet testSpreadsheet;

        [SetUp]
        public void SetUp()
        {
            Random rand = new Random();
            testSpreadsheet = new Spreadsheet(5, 5);
            for (int i = 0; i < testSpreadsheet.RowCount; i++)
            {
                for (int j = 0; j < testSpreadsheet.ColumnCount; j++)
                    testSpreadsheet.cellArray[i, j].Text = 10.ToString(); // sets every cell's value to 10.
            }
        }

        /// <summary>
        /// Test method for the Spreadsheet class, that checks if the values of the cells were stored properly.
        /// </summary>
        [Test]
        public void TestSpreadsheetValue()
        {
            Assert.AreEqual(10.ToString(), testSpreadsheet.cellArray[0, 0].Value);
            Assert.AreEqual(10.ToString(), testSpreadsheet.cellArray[0, 4].Value);
            Assert.AreEqual(10.ToString(), testSpreadsheet.cellArray[4, 0].Value);
            Assert.AreEqual(10.ToString(), testSpreadsheet.cellArray[4, 4].Value);
        }

        /// <summary>
        /// Test method for the Spreadsheet class, that checks if the updated value of the cell is stored properly.
        /// </summary>
        [Test]
        public void TestCellValueChanged()
        {
            testSpreadsheet.cellArray[0, 0].Text = "8";
            Assert.AreEqual(testSpreadsheet.cellArray[0, 0].Value, "8");
        }

        /// <summary>
        /// Test method for the Spreadsheet class, that checks if the cell referenced another value correctly.
        /// </summary>
        [Test]
        public void TestCellAssignmentValue()
        {
            testSpreadsheet.cellArray[0, 0].Text = "17";
            testSpreadsheet.cellArray[0, 1].Text = "=A1";
            Assert.AreEqual(17, Double.Parse(testSpreadsheet.cellArray[0, 1].Value));
        }

        /// <summary>
        /// Test method for the Spreadsheet class, that checks if the system throws an exception when a row or column index is out of bounds.
        /// </summary>
        [Test]
        public void TestCellValueOutOfBounds()
        {
            Assert.Throws<IndexOutOfRangeException>(() => testSpreadsheet.cellArray[0, 5].Text = "Error!");
        }

        /// <summary>
        /// Test method for the Spreadsheet class, that checks if a cell is updated properly when a cell
        /// it's referring to has updated.
        /// </summary>
        [Test]
        public void TestReassignValue()
        {
            testSpreadsheet.cellArray[0, 0].Text = "17";
            testSpreadsheet.cellArray[0, 1].Text = "=A1";
            testSpreadsheet.cellArray[0, 1].AddDependents(testSpreadsheet);   
            testSpreadsheet.cellArray[0, 0].Text = "40";
            testSpreadsheet.cellArray[0, 1].Update();
            Assert.AreEqual(40, Double.Parse(testSpreadsheet.cellArray[0, 1].Value));
        }

        /// <summary>
        /// Test method for the Spreadsheet class, that checks if the program correctly handles
        /// an invalid reference.
        /// </summary>
        [Test]
        public void TestInvalidReference()
        {
            testSpreadsheet.cellArray[0, 0].Text = "=X99";
            Assert.AreEqual(Double.NaN, Double.Parse(testSpreadsheet.cellArray[0, 0].Value));
        }

        [Test]
        public void TestAddActions()
        {
            Action action1 = () => { testSpreadsheet.cellArray[0, 0].Text = "1"; };
            Action action2 = () => { testSpreadsheet.cellArray[0, 0].Text = "A2"; };
            testSpreadsheet.AddUndo(action1, 4);
            testSpreadsheet.AddUndo(action2, 7);
            Assert.AreEqual(2, testSpreadsheet.UndoCount);
            Assert.AreEqual(7, testSpreadsheet.NextUndoClassification);
            Action action3 = () => { testSpreadsheet.cellArray[0, 1].Text = "B3"; };
            testSpreadsheet.AddRedo(action3, 2);
            Assert.AreEqual(1, testSpreadsheet.RedoCount);
            Assert.AreEqual(2, testSpreadsheet.NextRedoClassification);
        }

        [Test]
        public void TestUndoRedo()
        {
            Action action4 = () => { testSpreadsheet.cellArray[0, 0].Text = "X"; };
            Action action5 = () => { testSpreadsheet.cellArray[0, 0].Text = "Bye"; };
            testSpreadsheet.AddUndo(action4, 1);
            testSpreadsheet.AddUndo(action5, 2);
            testSpreadsheet.Undo();
            Assert.AreEqual(0, testSpreadsheet.UndoCount);
            Assert.AreEqual(2, testSpreadsheet.RedoCount);
            testSpreadsheet.Redo();
            Assert.AreEqual(2, testSpreadsheet.UndoCount);
            Assert.AreEqual(0, testSpreadsheet.RedoCount);
        }
    }
}
