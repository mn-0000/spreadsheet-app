using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CptS321
{
    /// <summary>
    /// The class for the spreadsheet.
    /// </summary>
    public class Spreadsheet : Cell
    {
        // Member variables
        private Stack<Tuple<Action, int>> undoStack = new Stack<Tuple<Action, int>>();
        private Stack<Tuple<Action, int>> redoStack = new Stack<Tuple<Action, int>>();
        private Action currentUndo;
        private Action currentRedo;
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { };
        public Cell[,] cellArray;

        public int RowCount { get; }
        public int ColumnCount { get; }

        public int NextUndoClassification { get { return undoStack.Peek().Item2; } }
        public int NextRedoClassification { get { return redoStack.Peek().Item2; } }

        public int UndoCount { get { return undoStack.Count; } }
        public int RedoCount { get { return redoStack.Count; } }

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

        /// <summary>
        /// Adds an undoable action to the undo stack.
        /// </summary>
        /// <param name="action"> The action to be added to the undo stack. </param>
        /// <param name="classification"> An integer representing what type of action is provided. </param>
        public void AddUndo(Action action, int classification)
        {
            undoStack.Push(Tuple.Create(action, classification));
        }

        /// <summary>
        /// Undoes an action.
        /// </summary>
        public void Undo()
        {
            // Pop the last action from undoStack and push to redoStack
            redoStack.Push(undoStack.Pop());
            // Get the previous action and execute it.
            currentUndo = undoStack.Peek().Item1;
            currentUndo.Invoke();
            // Pop the previous action from undoStack and push to redoStack
            redoStack.Push(undoStack.Pop());
        }

        /// <summary>
        /// Adds an redoable action to the undo stack.
        /// </summary>
        /// <param name="action"> The action to be added to the redo stack. </param>
        /// <param name="classification"> An integer representing what type of action is provided. </param>
        public void AddRedo(Action action, int classification)
        {
            redoStack.Push(Tuple.Create(action, classification));
        }

        /// <summary>
        /// Redoes an action.
        /// </summary>
        public void Redo()
        {
            // Pop the previous action from undoStack and push to undoStack
            undoStack.Push(redoStack.Pop());
            // Get the last undo action and execute it.
            currentRedo = redoStack.Peek().Item1;
            currentRedo.Invoke();
            // Pop the last undo action from undoStack and push to undoStack
            undoStack.Push(redoStack.Pop());
        }

        public void Save(Spreadsheet spreadsheet, string filePath)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            XmlWriter xmlWriter = XmlWriter.Create(filePath, settings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("spreadsheet");
            foreach (ConcreteCell cell in spreadsheet.cellArray)
            {
                if (cell.Text != "" || cell.BGColor != 0xFFFFFFFF)
                {
                    StringBuilder cellName = new StringBuilder();
                    cellName.Append(Convert.ToChar(cell.ColumnIndex + 65));
                    cellName.Append((cell.RowIndex + 1).ToString());

                    xmlWriter.WriteStartElement("cell");

                    xmlWriter.WriteAttributeString("name", cellName.ToString());
                    xmlWriter.WriteStartElement("text");
                    xmlWriter.WriteString(cell.Text);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("bgcolor");
                    xmlWriter.WriteString(cell.BGColor.ToString("X"));
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public void Load(FileStream fileStream, Spreadsheet spreadsheet)
        {
            undoStack.Clear();
            redoStack.Clear();
            int rowIndex = 0;
            int columnIndex = 0;
            string cellText = "";
            uint color = 0xFFFFFFFF;
            using (XmlReader reader = XmlReader.Create(fileStream))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "cell":
                                    string cellName = reader.GetAttribute("name");
                                    columnIndex = cellName[0] - 65;
                                    rowIndex = cellName[1] - 49;
                                    break;

                                case "text":
                                    cellText = reader.ReadElementContentAsString();
                                    spreadsheet.cellArray[rowIndex, columnIndex].Text = cellText;
                                    break;

                                case "bgcolor":
                                    color = uint.Parse(reader.ReadElementContentAsString(), System.Globalization.NumberStyles.HexNumber);
                                    spreadsheet.cellArray[rowIndex, columnIndex].BGColor = color;
                                    break;
                            }
                            break;
                    }
                }
            }
        }
    }
}
