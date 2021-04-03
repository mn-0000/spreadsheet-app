// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

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
            Assert.AreEqual(testSpreadsheet.cellArray[0, 0].Value, 10.ToString());
            Assert.AreEqual(testSpreadsheet.cellArray[0, 4].Value, 10.ToString());
            Assert.AreEqual(testSpreadsheet.cellArray[4, 0].Value, 10.ToString());
            Assert.AreEqual(testSpreadsheet.cellArray[4, 4].Value, 10.ToString());
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
        /// Test method for the Spreadsheet class, that checks if the program notifies user if their input was incorrect.
        /// </summary>
        [Test]
        public void TestReferenceError()
        {
            testSpreadsheet.cellArray[0, 3].Text = "=XX99";
            Assert.AreEqual(testSpreadsheet.cellArray[0, 3].Value, "Invalid reference");
        }
    }
}
