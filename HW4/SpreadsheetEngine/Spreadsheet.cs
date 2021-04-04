using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The class for the spreadsheet.
    /// </summary>
    public class Spreadsheet : Cell
    {
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { };

        // Member variables
        public Cell[,] cellArray;
        public int RowCount { get; }
        public int ColumnCount { get; }

        /// <summary>
        /// Constructor for the spreadsheet class.
        /// </summary>
        /// <param name="Rows"> the number of rows for the spreadsheet</param>
        /// <param name="Columns"> the number of columns for the spreadsheet</param>
        public Spreadsheet(int Rows, int Columns)
        {
            RowCount = Rows;
            ColumnCount = Columns;
            cellArray = new Cell[RowCount, ColumnCount];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    cellArray[i, j] = new ConcreteCell(i, j);
                    cellArray[i, j].PropertyChanged += OnCellPropertyChanged; // subscribe the spreadsheet to each cell's PropertyChanged event
                }
            }

        }

        /// <summary>
        /// Code that will be executed once a cell's property has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell cell = (sender as Cell);
            // If the sender cell's text is empty, assign null to the text of the cell given by the sender cell's row and column index.
            if (cell.Text == null)
            {
                cellArray[cell.RowIndex, cell.ColumnIndex].Text = null;
                cellArray[cell.RowIndex, cell.ColumnIndex].SetValue(null);
            }
            // If sender cell's text does not start with '=', 
            // assign the cell's text to the text of the cell given by the sender cell's row and column index.
            else if (!cell.Text.StartsWith("="))
            {
                cellArray[cell.RowIndex, cell.ColumnIndex].Text = cell.Text;
                cellArray[cell.RowIndex, cell.ColumnIndex].SetValue(cellArray[cell.RowIndex, cell.ColumnIndex].Text);
            }
            // If the sender cell's text starts with '=', perform evaluation.
            else
            {   
                // Create a new expression tree with the given formula to access its methods.
                ExpressionTree expressionTree = new ExpressionTree(cell.Text.Substring(1));
                // Evaluates the formula, sets the value for the cell.
                cellArray[cell.RowIndex, cell.ColumnIndex].SetValue(expressionTree.EvaluateSpreadsheetFormula(cellArray).ToString());
            }

            CellPropertyChanged(sender, e);
        }

        
    }
}
