using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data;

namespace CptS321
{
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

        public int RowIndex { get { return rowIndex; } }
        public int ColumnIndex { get { return columnIndex; } }

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
       
                if (value == null) { return; } // if the new value is null, simply return
                else
                {
                    // If the text is a formula, create a new expression tree with the equal sign omitted.
                    if (text.StartsWith("="))
                    {
                        tempExpressionTree = new ExpressionTree(text.Substring(1));
                    }
                    else
                    {
                        tempExpressionTree = new ExpressionTree(text);
                    }
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
        /// <returns> the cell, with its value updated. </returns>
        internal void SetValue(string val)
        {
            value = val;
        }

        public void OnPropertyChanged([CallerMemberName] string value = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
            isEventFired = true;
        }

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

        public void AddDependents(Spreadsheet spreadsheet)
        {
            double test = 0;
            foreach (string component in treeComponents)
            {
                if (!double.TryParse(component, out test) && !"+-*/%^".Contains(component))
                {
                    int rowNumber = Convert.ToInt32(component.Substring(1)) - 1;
                    int columnNumber = component[0] - 65;
                    dependents.Add(spreadsheet.cellArray[rowNumber, columnNumber]);
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
