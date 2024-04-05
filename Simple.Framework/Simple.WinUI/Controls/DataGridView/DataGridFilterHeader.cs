namespace Simple.WinUI.Controls.DataGridView
{
    public class DataGridFilterHeader : DataGridViewColumnHeaderCell
    {
        private System.Windows.Forms.VisualStyles.ComboBoxState currentState = System.Windows.Forms.VisualStyles.ComboBoxState.Normal;

        private Point cellLocation;

        private Rectangle buttonRect;

        public DataGridFilterHeader() : base()
        {
        }

        public event EventHandler<ColumnFilterClickedEventArg> FilterButtonClicked;

        private bool IsMouseOverButton(Point e)
        {
            Point p = new Point(e.X + cellLocation.X, e.Y + cellLocation.Y);
            if (p.X >= buttonRect.X && p.X <= buttonRect.X + buttonRect.Width &&
                p.Y >= buttonRect.Y && p.Y <= buttonRect.Y + buttonRect.Height)
            {
                return true;
            }
            return false;
        }

        protected override void OnDataGridViewChanged()
        {
            try
            {
                if (this.RowIndex != -1)
                {
                    Padding dropDownPadding = new Padding(0, 0, 20, 0);
                    this.Style.Padding = Padding.Add(this.InheritedStyle.Padding, dropDownPadding);
                }
            }
            catch { }
            base.OnDataGridViewChanged();
        }

        protected override void Paint(Graphics graphics,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex,
                                      DataGridViewElementStates dataGridViewElementState,
                                      object value,
                                      object formattedValue,
                                      string errorText,
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds,
                       cellBounds, rowIndex,
                       dataGridViewElementState, value,
                       formattedValue, errorText,
                       cellStyle, advancedBorderStyle, paintParts);

            int width = 20; // 20 px
            buttonRect = new Rectangle(cellBounds.X + cellBounds.Width - width, cellBounds.Y, width, cellBounds.Height);
            cellLocation = cellBounds.Location;
            ComboBoxRenderer.DrawDropDownButton(graphics, buttonRect, currentState);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (this.IsMouseOverButton(e.Location))
                currentState = System.Windows.Forms.VisualStyles.ComboBoxState.Pressed;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (this.IsMouseOverButton(e.Location))
            {
                currentState = System.Windows.Forms.VisualStyles.ComboBoxState.Normal;
                this.OnFilterButtonClicked();
            }
            base.OnMouseUp(e);
        }

        protected virtual void OnFilterButtonClicked()
        {
            if (this.FilterButtonClicked != null)
            {
                this.FilterButtonClicked(this, new ColumnFilterClickedEventArg(this.ColumnIndex, this.buttonRect));
            }
        }
    }
}