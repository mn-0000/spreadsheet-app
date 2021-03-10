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
        /// Retrieves the cell given at the specified row and column index.
        /// </summary>
        /// <param name="rowIndex"> the row index of the cell </param>
        /// <param name="columnIndex"> the column index of the cell</param>
        /// <returns>
        /// the cell at the specified row and column index.
        /// </returns>
        private Cell GetCell(int rowIndex, int columnIndex)
        {
            return cellArray[rowIndex, columnIndex];
        }

        /// <summary>
        /// Code that will be executed once a cell's property has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell cell = (sender as Cell);
            int rowNumber = 0;
            // If the sender cell's text is empty, assign null to the text of the cell given by the sender cell's row and column index.
            if (cell.Text == null)
            {
                cellArray[cell.RowIndex, cell.ColumnIndex].Text = null;
            }
            // If sender cell's text does not start with '=', 
            // assign the cell's text to the text of the cell given by the sender cell's row and column index.
            else if (!cell.Text.StartsWith("="))
            {
                cellArray[cell.RowIndex, cell.ColumnIndex].Text = cell.Text;
            }
            // If the sender cell's text starts with '=', perform evaluation.
            else
            {
                // If the input was not in the correct format, informs user about it.

                // Takes the numeric part of the text and subtract by 1 to return 0-based row index.
                try
                {
                    rowNumber = Convert.ToInt32(cell.Text.Substring(2)) - 1;
                }
                catch (FormatException wrongFormat)
                {
                    cell.Text = "Invalid reference";
                    return;
                }
                // Takes the column name and subtract its ASCII value by 65 to return 0-based column index.
                int columnNumber = cell.Text[1] - 65;
                if (rowNumber > RowCount || columnIndex > ColumnCount)
                {
                    cell.Text = "Invalid reference";
                    return;
                }
                // Get the cell at the sender cell's position, and assign its text to the cell array's cell at the equivalent position.
                cellArray[cell.RowIndex, cell.ColumnIndex].Text = GetCell(rowNumber, columnNumber).Text;
            }

            // Set the value for the cell.
            cellArray[cell.RowIndex, cell.ColumnIndex].SetValue(cellArray[cell.RowIndex, cell.ColumnIndex].Text);
            CellPropertyChanged(sender, e);
        }
    }
}
