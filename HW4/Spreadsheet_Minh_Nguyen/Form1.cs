using CptS321;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private Action mostRecentAction;
        private Action initialState;
        private Action initialText;
        private Action mostRecentText;

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

            string previousText = userSpreadsheet.cellArray[e.RowIndex, e.ColumnIndex].Text; // the previous state's text.
            // Create a backup of the cell's previous state, and pushes it to the spreadsheet's undoStack.
            initialText = () =>
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].CellTextChange(this, dataGridView1, previousText)();
            };
            // Add the backup to the spreadsheet's undoStack.
            userSpreadsheet.AddUndo(initialText, 1);

            string currentText = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value; // the current text.
            // Create a block of actions to be executed when the text has been changed.
            mostRecentText = () =>
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].CellTextChange(this, dataGridView1, currentText)();
            };
            // Add the block of actions to the spreadsheet's undoStack.
            userSpreadsheet.AddUndo(mostRecentText, 1);
            // Execute the actions block.
            mostRecentText.Invoke();

            // Enable the Undo option and set its text.
            undoToolStripMenuItem.Enabled = true;
            undoToolStripMenuItem.Text = "Undo text change";
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
            // Create a new ColorDialog and set the default color choice to the cell's current color.
            ColorDialog cellColorSelection = new ColorDialog();
            cellColorSelection.Color = dataGridView1.CurrentCell.Style.BackColor;

            // If the user actually decides to change the cell's back color, execute the following code.
            if (cellColorSelection.ShowDialog() == DialogResult.OK)
            {
                DataGridViewSelectedCellCollection selectedCells = dataGridView1.SelectedCells; // get list of cells selected for color change.

                // Create a list containing the colors of the selected cells.
                List<Color> originalBackground = new List<Color>();
                foreach (DataGridViewCell cell in selectedCells)
                {
                    // (In the case of an undo) If a cell is previously empty and had its color changed,
                    // add the color white to the list as opposed to Color.Empty (which will cause
                    // undesired changes)
                    if (cell.Style.BackColor == Color.Empty)
                    {
                        originalBackground.Add(Color.White);
                    }
                    else
                    {
                        originalBackground.Add(cell.Style.BackColor);
                    }
                }

                // Create a backup of the selected cell(s)' previous states and add it to the spreadsheet's undoStack.
                initialState = () =>
                {
                    for (int i = 0; i < selectedCells.Count; i++)
                    {
                        selectedCells[i].CellColorChange(this, originalBackground[i])();
                    }
                };
                userSpreadsheet.AddUndo(initialState, 0);

                // Create a block of code that will be executed in order to change the cell(s)' back color,
                // and add it to the spreadsheet's undoStack.
                mostRecentAction = () =>
                {
                    foreach (DataGridViewCell cell in selectedCells)
                        cell.CellColorChange(this, cellColorSelection.Color)();
                };
                userSpreadsheet.AddUndo(mostRecentAction, 0);
                mostRecentAction.Invoke(); // execute the above block of code.

                // Enable the Undo option and set its text.
                undoToolStripMenuItem.Enabled = true;
                undoToolStripMenuItem.Text = "Undo color change";
            }
        }

        /// <summary>
        /// Code that will be executed when the user opts to undo an action.
        /// </summary>
        /// <param name="sender"> The Undo option. </param>
        /// <param name="e"> The Undo option's properties. </param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redoToolStripMenuItem.Enabled = true; // performing an undo means that a redo is possible, so enable the Redo control
            userSpreadsheet.Undo();
            // If there's no actions left in the spreadsheet's undoStack, disable it.
            if (userSpreadsheet.UndoCount == 0)
            {
                undoToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Text = "Undo";
            }
            else
            {
                // Determines what the undo option's text would be after the undo.
                DetermineActionText(userSpreadsheet.NextUndoClassification, undoToolStripMenuItem);
            }
            // Determines what the redo option's text would be after the undo.
            DetermineActionText(userSpreadsheet.NextRedoClassification, redoToolStripMenuItem);
        }

        /// <summary>
        /// Code that will be executed when the user opts to redo an action.
        /// </summary>
        /// <param name="sender"> The Redo option. </param>
        /// <param name="e"> The Redo option's properties. </param>
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = true; // performing a redo means that an undo is possible, so enable the Undo control
            userSpreadsheet.Redo();
            // If there's no actions left in the spreadsheet's redoStack, disable it.
            if (userSpreadsheet.RedoCount == 0)
            {
                redoToolStripMenuItem.Enabled = false;
                redoToolStripMenuItem.Text = "Redo";
            }
            else
            {
                // Determines what the redo option's text would be after the redo.
                DetermineActionText(userSpreadsheet.NextRedoClassification, redoToolStripMenuItem);
            }
            // Determines what the undo option's text would be after the redo.
            DetermineActionText(userSpreadsheet.NextUndoClassification, undoToolStripMenuItem);
        }

        /// <summary>
        /// Sets the undo/redo options' text.
        /// </summary>
        /// <param name="classification"> An integer representing the type of action. </param>
        /// <param name="item"> The menu item to have its text changed. </param>
        private void DetermineActionText(int classification, ToolStripMenuItem item)
        {
            switch (classification)
            {
                case 0:
                    item.Text = item.Tag + " color change";
                    break;
                case 1:
                    item.Text = item.Tag + " text change";
                    break;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            saveFileDialog.RestoreDirectory = true;
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = saveFileDialog.FileName;
                userSpreadsheet.Save(userSpreadsheet, savePath);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1;
            
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.RestoreDirectory = true; // restores the last directory the user have accessed.

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < userSpreadsheet.RowCount; i++)
                {
                    for (int j = 0; j < userSpreadsheet.ColumnCount; j++)
                    {
                        userSpreadsheet.cellArray[i, j].Text = "";
                        userSpreadsheet.cellArray[i, j].BGColor = 0xFFFFFFFF;
                    }
                }
                FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                userSpreadsheet.Load(fileStream, userSpreadsheet);
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
