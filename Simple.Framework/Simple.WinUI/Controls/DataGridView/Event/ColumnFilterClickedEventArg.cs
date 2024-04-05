namespace Simple.WinUI.Controls.DataGridView
{
    public class ColumnFilterClickedEventArg : EventArgs
    {
        public ColumnFilterClickedEventArg(int colIndex, Rectangle btnRect)
        {
            this.ColumnIndex = colIndex;
            this.ButtonRectangle = btnRect;
        }

        public int ColumnIndex { get; private set; }
        public Rectangle ButtonRectangle { get; private set; }
    }
}