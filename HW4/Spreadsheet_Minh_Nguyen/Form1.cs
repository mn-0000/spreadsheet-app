using CptS321;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spreadsheet_Minh_Nguyen
{
    public partial class Form1 : Form
    {
        public Spreadsheet userSpreadsheet;
        public Form1()
        {
            userSpreadsheet = new Spreadsheet(50, 26); // initialize spreadsheet
            userSpreadsheet.CellPropertyChanged += OnCellPropertyChanged;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();  //clears any columns that may exist prior to creating rows and columns for the spreadsheet

            // Creates columns and rows for the spreadsheet
            for (int i = 0; i < userSpreadsheet.ColumnCount; i++)
                dataGridView1.Columns.Add("Column" + i, Convert.ToChar(i + (int)'A').ToString());
            for (int i = 0; i < userSpreadsheet.RowCount; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        /// <summary>
        /// Upon finishing editing a DataGridView cell's text, assign that text to the Cell's Text property at the corresponding position.
        /// </summary>
        /// <param name="sender"> the DataGridView cell that has its text changed. </param>
        /// <param name="e"> the data of the DataGridView cell that has its text changed. </param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            userSpreadsheet.cellArray[e.RowIndex, e.ColumnIndex].Text = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        /// <summary>
        /// When a cell's value is changed, assign the value of that cell to the DataGridView cell's Value property at the corresponding position.
        /// </summary>
        /// <param name="sender"> the Cell that has its Value changed. </param>
        /// <param name="e"> the data of the Cell that has its Value changed. </param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell cell = (sender as Cell);
            dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
        }

        /// <summary>
        /// When clicked, activates the demo.
        /// </summary>
        /// <param name="sender"> The button </param>
        /// <param name="e"> THe data of the button </param>
        private void btnDemo_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            // Demo part 1 - Set text in 50 random cells
            for (int i = 0; i < 50; i++)
            {
                int randomRow = rand.Next(0, 50);
                int randomColumn = rand.Next(0, 26);
                userSpreadsheet.cellArray[randomRow, randomColumn].Text = "Scattered!";
            }

            // Demo part 2 - Set text in every cell in column B and assign value of cells in column B to column A
            for (int i = 0; i < userSpreadsheet.RowCount; i++)
            {
                userSpreadsheet.cellArray[i, 1].Text = "This is cell B" + (i + 1).ToString();
                userSpreadsheet.cellArray[i, 0].Text = "=B" + (i + 1).ToString();
            }
        }
    }
}
