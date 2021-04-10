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
    /// <summary>
    /// The main form.
    /// </summary>
    public partial class Form1 : Form
    {
        public Spreadsheet userSpreadsheet;
        private Stack<Func<object>> undoStack = new Stack<Func<object>>();
        private Stack<Func<object>> redoStack = new Stack<Func<object>>();
        private Action mostRecentAction;
        private Action initialState;
        private Action initialText;
        private Action mostRecentText;
        private DataGridViewSelectedCellCollection previouslySelectedCells;

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
        /// When the user begins editing a cell, display that cell's text.
        /// </summary>
        /// <param name="sender"> the current DataGridView cell </param>
        /// <param name="e"> the properties of the current DataGridView cell </param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = userSpreadsheet.cellArray[e.RowIndex, e.ColumnIndex].Text;
        }

        /// <summary>
        /// Upon finishing editing a DataGridView cell's text, assign that text to the Cell's Text property at the corresponding position,
        /// or evaluate the formula if it starts with an equal sign.
        /// </summary>
        /// <param name="sender"> the DataGridView cell that has its text changed. </param>
        /// <param name="e"> the data of the DataGridView cell that has its text changed. </param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string previousText = userSpreadsheet.cellArray[e.RowIndex, e.ColumnIndex].Text;
            double test = 0; // for the TryParse method
            initialText = () =>
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].CellTextChange(this, dataGridView1, previousText)();
            };
            userSpreadsheet.AddUndo(initialText, 1);
            string currentText = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            mostRecentText = () =>
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].CellTextChange(this, dataGridView1, currentText)();
            };
            userSpreadsheet.AddUndo(mostRecentText, 1);
            mostRecentText.Invoke();

            undoToolStripMenuItem.Enabled = true;
            undoToolStripMenuItem.Text = "Undo cell text change";
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
            dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Style.BackColor = Color.FromArgb((int)cell.BGColor);
        }

        private void changeCellBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cellColorSelection = new ColorDialog();
            cellColorSelection.Color = dataGridView1.CurrentCell.Style.BackColor;

            if (cellColorSelection.ShowDialog() == DialogResult.OK)
            {
                DataGridViewCell currentCell = dataGridView1.CurrentCell;

                DataGridViewSelectedCellCollection selectedCells = dataGridView1.SelectedCells;
                List<Color> originalBackground = new List<Color>();
                foreach (DataGridViewCell cell in selectedCells)
                {
                    if (cell.Style.BackColor == Color.Empty)
                    {
                        originalBackground.Add(Color.White);
                    }
                    else
                    {
                        originalBackground.Add(cell.Style.BackColor);
                    }
                }

                initialState = () =>
                {
                    for (int i = 0; i < selectedCells.Count; i++)
                    {
                        selectedCells[i].CellColorChange(this, originalBackground[i])();
                    }
                };
                userSpreadsheet.AddUndo(initialState, 0);
                previouslySelectedCells = selectedCells;

                mostRecentAction = () =>
                {
                    foreach (DataGridViewCell cell in selectedCells)
                        cell.CellColorChange(this, cellColorSelection.Color)();
                };
                userSpreadsheet.AddUndo(mostRecentAction, 0);
                mostRecentAction.Invoke();

                undoToolStripMenuItem.Enabled = true;
                undoToolStripMenuItem.Text = "Undo cell color change";
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redoToolStripMenuItem.Enabled = true;
            userSpreadsheet.Undo();
            if (userSpreadsheet.UndoCount == 0)
            {
                undoToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Text = "Undo";
            }
            else
            {        
                DetermineActionText(userSpreadsheet.NextUndoClassification, undoToolStripMenuItem);
            }
            DetermineActionText(userSpreadsheet.NextRedoClassification, redoToolStripMenuItem);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = true;
            userSpreadsheet.Redo();
            if (userSpreadsheet.RedoCount == 0)
            {
                redoToolStripMenuItem.Enabled = false;
                redoToolStripMenuItem.Text = "Redo";
            }
            else
            {
                DetermineActionText(userSpreadsheet.NextRedoClassification, redoToolStripMenuItem);       
            }
            DetermineActionText(userSpreadsheet.NextUndoClassification, undoToolStripMenuItem);
        }

        private void DetermineActionText(int classification, ToolStripMenuItem item)
        {
            switch (classification)
            {
                case 0:
                    item.Text = item.Tag + " cell color change";
                    break;
                case 1:
                    item.Text = item.Tag + " cell text change";
                    break;
            }
        }
        /// <summary>
        /// When clicked, activates the demo. (Currently unused, but still left here since 
        /// I'm not sure if I'm supposed to remove it)
        /// </summary>
        /// <param name="sender"> The button </param>
        /// <param name="e"> THe data of the button </param>
        //private void btnDemo_Click(object sender, EventArgs e)
        //{
        //    Random rand = new Random();
        //    // Demo part 1 - Set text in 50 random cells
        //    for (int i = 0; i < 50; i++)
        //    {
        //        int randomRow = rand.Next(0, 50);
        //        int randomColumn = rand.Next(0, 26);
        //        userSpreadsheet.cellArray[randomRow, randomColumn].Text = "Scattered!";
        //    }

        //    // Demo part 2 - Set text in every cell in column B and assign value of cells in column B to column A
        //    for (int i = 0; i < userSpreadsheet.RowCount; i++)
        //    {
        //        userSpreadsheet.cellArray[i, 1].Text = "This is cell B" + (i + 1).ToString();
        //        userSpreadsheet.cellArray[i, 0].Text = "=B" + (i + 1).ToString();
        //    }
        //}

    }

}
