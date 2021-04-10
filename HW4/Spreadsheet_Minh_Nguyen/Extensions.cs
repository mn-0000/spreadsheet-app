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
    public static class Extensions
    {

        public static Func<DataGridViewCell> CellColorChange(this DataGridViewCell dgvCell, Form1 form1, Color color)
        {
            return () =>
            {
                form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].BGColor = (uint)color.ToArgb();
                return dgvCell;
            };
        }

        public static Func<DataGridViewCell> CellTextChange(this DataGridViewCell dgvCell, Form1 form1, DataGridView dataGridView, string text)
        {
            double test = 0;
            return () =>
            {
                form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Text = text;
                if (String.IsNullOrEmpty(form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Text)
                    || (!double.TryParse(form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Text, out test) && !form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Text.StartsWith("=")))
                {
                    foreach (Cell dependent in form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Dependents) dependent.Update();
                }
                else
                {
                    // If the formula is incorrect, notify the user with a MessageBox showing potential errors.
                    if (Double.IsNaN(Double.Parse(form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Value)))
                    {
                        MessageBox.Show("Your formula has an error. This could be due to the following reasons:" +
                            "\n    - One or more cells you're referring to is invalid or currently empty." +
                            "\n    - The number of opening and closing parentheses are not the same." +
                            "\n    - There are no arguments in your formula." +
                            "\nPlease try again!");
                    }
                    // Otherwise, evaluate the formula and show the results.
                    else
                    {
                        form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].AddDependents(form1.userSpreadsheet);
                        form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Update();
                        dataGridView.Rows[dgvCell.RowIndex].Cells[dgvCell.ColumnIndex].Value = form1.userSpreadsheet.cellArray[dgvCell.RowIndex, dgvCell.ColumnIndex].Value;
                    }
                }
                return dgvCell;
            };
        }
    }
}
