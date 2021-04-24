using CptS321;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spreadsheet_Minh_Nguyen
{
    /// <summary>
    /// An extension class to the main Form1, supplementing it with
    /// some additional functionality.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Changes the back color of a cell.
        /// </summary>
        /// <param name="cell"> The DataGridViewCell that will have its back color changed. </param>
        /// <param name="form1"> The form the cell belongs to. </param>
        /// <param name="color"> The new back color of the cell. </param>
        /// <returns> A block of code that will be executed when called from the given form. </returns>
        public static Func<DataGridViewCell> CellColorChange(this DataGridViewCell cell, Form1 form1, Color color)
        {
            return () =>
            {
                form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].BGColor = (uint)color.ToArgb();
                return cell;
            };
        }

        /// <summary>
        /// Takes the new text of a cell and updates its value.
        /// </summary>
        /// <param name="cell"> The DataGridViewCell that had its text changed. </param>
        /// <param name="form1"> The form that the cell belongs to. </param>
        /// <param name="dataGridView"> The DataGridView the cell belongs to. </param>
        /// <param name="text"> The new text of the cell. </param>
        /// <returns> A block of code that will be executed when called from the given form. </returns>
        public static Func<DataGridViewCell> CellTextChange(this DataGridViewCell cell, Form1 form1, DataGridView dataGridView, string text)
        {
            double test; // for the TryParse function
            return () =>
            {
                // Sets the text of the cell.
                form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Text = text;
                // If the cell is empty or is both not a double and not a formula, update the dependent cells.
                if (String.IsNullOrEmpty(form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Text)
                    || (!double.TryParse(form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Text, out test) && !form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Text.StartsWith("=")))
                {
                    foreach (Cell dependent in form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Dependents) dependent.Update();
                }
                else
                {
                    if (form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Value.StartsWith("!"))
                    {
                        dataGridView.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Value;
                    }
                    else
                    {
                        form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].AddDependents(form1.userSpreadsheet);
                        foreach (Cell dependentCell in form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Dependents)
                            dependentCell.Update();

                        if ((double.TryParse(form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Value, out test) && Double.IsPositiveInfinity(Double.Parse(form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Value))))
                        {
                            dataGridView.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = "!(Circular reference)";
                        }
                        else
                        {
                            dataGridView.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = form1.userSpreadsheet.cellArray[cell.RowIndex, cell.ColumnIndex].Value;
                        }
                    }
                }
                return cell;
            };
        }
    }
}
