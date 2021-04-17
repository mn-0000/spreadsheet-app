using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data;
using System.Drawing;

namespace CptS321
{
    /// <summary>
    /// The abstract class for the spreadsheet cell.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        // member variables
        protected string text = "";
        protected string value;
        protected int rowIndex;
        protected int columnIndex;
        protected bool isEventFired = false;
        protected List<Cell> dependents = new List<Cell>();
        protected ExpressionTree tempExpressionTree;
        protected string postfixExpression;
        protected string[] treeComponents;
        protected uint color = 0xFFFFFFFF;


        public int RowIndex { get { return rowIndex; } }
        public int ColumnIndex { get { return columnIndex; } }

        public uint BGColor
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }

        public List<Cell> Dependents { get { return dependents; } }
        public string[] TreeComponents { get { return treeComponents; } }

        /// <summary>
        /// Constructor for the Cell class.
        /// </summary>
        public Cell()
        {
            rowIndex = 0;
            columnIndex = 0;
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text == value) { return; }
                text = value;

                if (value == null) { OnPropertyChanged(); }
                else
                {
                    // If the text is a formula, create a new expression tree with the equal sign omitted.
                    if (text.StartsWith("="))
                    {
                        tempExpressionTree = new ExpressionTree(text.Substring(1));
                    }
                    // Otherwise, create a new expression tree with the given text.
                    else
                    {
                        tempExpressionTree = new ExpressionTree(text);
                    }
                    // Convert the text to postfix notation for cell dependency searching
                    treeComponents = tempExpressionTree.ConvertToPostFix(tempExpressionTree.RawExpression).Split(' ');

                    OnPropertyChanged();
                }
            }
        }

        public string Value
        {
            get { return value; }
        }

        public bool IsEventFired { get { return isEventFired; } set { } }

        /// <summary>
        /// Sets the value for a cell.
        /// </summary>
        /// <param name="val"> the value to be set.</param>
        internal void SetValue(string val)
        {
            value = val;
        }

        /// <summary>
        /// Code that will execute when the text of a cell is updated.
        /// </summary>
        /// <param name="value"> the new text value </param>
        public void OnPropertyChanged([CallerMemberName] string value = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
            isEventFired = true; // signifies that the event has been fired.
        }

        /// <summary>
        /// Updates the cell, as well as all of the cell's dependents.
        /// </summary>
        public void Update()
        {
            foreach (Cell cell in dependents)
            {
                if (cell.IsEventFired == true)
                {
                    OnPropertyChanged();
                    cell.IsEventFired = false;
                }
            }
        }

        /// <summary>
        /// Adds the dependents of a cell to its dependents list.
        /// </summary>
        /// <param name="spreadsheet"> the spreadsheet the cell is in </param>
        public void AddDependents(Spreadsheet spreadsheet)
        {
            double test = 0; // for the TryParse method below

            foreach (string component in treeComponents)
            {
                // If the component is a variable referring to a cell
                if (!double.TryParse(component, out test) && !"+-*/%^".Contains(component))
                {
                    // Get the cell's row number and column number
                    int rowNumber = Convert.ToInt32(component.Substring(1)) - 1;
                    int columnNumber = component[0] - 65;

                    // Add the cell to the current cell's dependency list.
                    dependents.Add(spreadsheet.cellArray[rowNumber, columnNumber]);

                    // Also add all the cells that are dependent on each other to all depending cell's dependent lists.
                    foreach (Cell cell in dependents)
                    {
                        if (!cell.Dependents.Contains(this))
                        {
                            cell.Dependents.Add(this);
                        }
                    }
                    foreach (Cell dependentCell in spreadsheet.cellArray[rowNumber, columnNumber].Dependents)
                    {
                        if (!dependentCell.Dependents.Contains(this))
                        {
                            dependentCell.Dependents.Add(this);
                        }
                    }

                }
            }
            // Updates all involved cells.
            foreach (Cell cell in Dependents)
            {
                if (cell.IsEventFired == true)
                {
                    cell.IsEventFired = false;
                    cell.OnPropertyChanged();
                }
            }
        }
    }
}
