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
        }
    }
}
