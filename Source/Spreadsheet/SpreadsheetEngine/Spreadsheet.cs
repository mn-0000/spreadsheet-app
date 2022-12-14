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
                cellArray[cell.RowIndex, cell.ColumnIndex].SetValue(expressionTree.EvaluateSpreadsheetFormula(cellArray, cell.RowIndex, cell.ColumnIndex).ToString());
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

        /// <summary>
        /// Saves the current spreadsheet to an XML filw with the given file name.
        /// </summary>
        /// <param name="spreadsheet"> The spreadsheet to be saved </param>
        /// <param name="filePath"> The file path (containing the file name) where the file will be stored </param>
        public void Save(Spreadsheet spreadsheet, string filePath)
        {
            // Create settings for the XML file.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            XmlWriter xmlWriter = XmlWriter.Create(filePath, settings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("spreadsheet"); // start spreadsheet node
            foreach (ConcreteCell cell in spreadsheet.cellArray)
            {
                // Check if the cell's two main properties differ from their default values.
                // Only those that have their properties changed will be added to the XML file.
                if (cell.Text != "" || cell.BGColor != 0xFFFFFFFF)
                {
                    // Get the cell name
                    StringBuilder cellName = new StringBuilder();
                    cellName.Append(Convert.ToChar(cell.ColumnIndex + 65));
                    cellName.Append((cell.RowIndex + 1).ToString());

                    xmlWriter.WriteStartElement("cell"); // start cell node
                    xmlWriter.WriteAttributeString("name", cellName.ToString()); // name of the cell

                    // Write text node for the current cell
                    xmlWriter.WriteStartElement("text");
                    xmlWriter.WriteString(cell.Text);
                    xmlWriter.WriteEndElement();

                    // Write background color node for the current cell
                    xmlWriter.WriteStartElement("bgcolor");
                    xmlWriter.WriteString(cell.BGColor.ToString("X")); // store color in hex format
                    xmlWriter.WriteEndElement();

                    // End the current cell's node
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement(); // end spreadsheet node
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        /// <summary>
        /// Loads a spreadsheet stored in an XML file to the current spreadsheet.
        /// All changes made prior to the loading will be removed.
        /// </summary>
        /// <param name="fileStream"> The incoming file </param>
        /// <param name="spreadsheet"> The spreadsheet to load to </param>
        public void Load(FileStream fileStream, Spreadsheet spreadsheet)
        {
            // Clear the undo and redo stacks.
            undoStack.Clear();
            redoStack.Clear();

            // Necessary variables.
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
                        // Only process nodes that are elements.
                        case XmlNodeType.Element:
                            // Try-catch statement is used to safeguard bad cells (ex: A66) in XML file
                            try
                            {
                                // Only process the necessary attributes and elements.
                                // All unnecessary attributes and elements will be ignored.
                                switch (reader.Name)
                                {
                                    // Get cell position
                                    case "cell":
                                        string cellName = reader.GetAttribute("name");
                                        columnIndex = cellName[0] - 65;
                                        rowIndex = Convert.ToInt32(cellName.Substring(1)) - 1;
                                        break;

                                    // Get cell text
                                    case "text":
                                        cellText = reader.ReadElementContentAsString();
                                        spreadsheet.cellArray[rowIndex, columnIndex].Text = cellText;
                                        break;

                                    // Get cell background color.
                                    case "bgcolor":
                                        color = uint.Parse(reader.ReadElementContentAsString(), System.Globalization.NumberStyles.HexNumber);
                                        spreadsheet.cellArray[rowIndex, columnIndex].BGColor = color;
                                        break;
                                }
                                break;
                            } catch (FormatException)
                            {
                                break;
                            } catch (IndexOutOfRangeException)
                            {
                                break;
                            }
                    }
                }
            }
        }
    }
}
