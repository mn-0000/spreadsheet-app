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
        protected string value = "";
        protected int rowIndex;
        protected int columnIndex;
       
        public int RowIndex { get { return rowIndex; } }
        public int ColumnIndex { get { return columnIndex; } }

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
                OnPropertyChanged();
            }
        }
        public string Value
        {
            get { return value; }
        }

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
        }
    }
}
