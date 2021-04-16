using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// The concrete class for the spreadsheet cell.
    /// </summary>
    public class ConcreteCell : Cell
    {
        /// <summary>
        /// Constructor for the ConcreteCell class that sets the row and column index for the cells.
        /// </summary>
        /// <param name="RowIndex"> row index of the cell </param>
        /// <param name="ColumnIndex"> column index of the cell </param>
        public ConcreteCell(int RowIndex, int ColumnIndex)
        {
            rowIndex = RowIndex;
            columnIndex = ColumnIndex;

        }
    }
}
