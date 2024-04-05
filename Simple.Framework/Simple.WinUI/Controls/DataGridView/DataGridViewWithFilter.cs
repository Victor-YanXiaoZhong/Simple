using System.ComponentModel;
using System.Data;

namespace Simple.WinUI.Controls.DataGridView
{
    public class DataGridViewWithFilter : System.Windows.Forms.DataGridView
    {
        /// <summary>是否显示过滤</summary>
        private bool showFilter = false;

        private List<FilterStatus> Filter = new List<FilterStatus>();

        private System.Windows.Forms.TextBox textBoxCtrl = new System.Windows.Forms.TextBox();

        private DateTimePicker DateTimeCtrl = new DateTimePicker();

        private CheckedListBox CheckCtrl = new CheckedListBox();

        private System.Windows.Forms.Button ApplyButtonCtrl = new System.Windows.Forms.Button();

        private System.Windows.Forms.Button ClearFilterCtrl = new System.Windows.Forms.Button();

        private ToolStripDropDown popup = new ToolStripDropDown();

        private string StrFilter = "";

        private string ButtonCtrlText = "过滤";

        private string ClearFilterCtrlText = "取消";

        private string CheckCtrlAllText = "<全部>";

        private string SpaceText = "<空>";

        public DataGridViewWithFilter()
        {
            AutoGenerateColumns = false;
            RowHeadersVisible = false;
        }

        // Текущий индекс ячейки
        private int columnIndex { get; set; }

        [Browsable(true)]
        [Description("是否显示过滤 默认否")]
        public bool ShowFilter
        {
            get
            {
                return showFilter;
            }
            set
            {
                showFilter = value;
            }
        }

        // Событие кнопки фильтрации
        private void header_FilterButtonClicked(object sender, ColumnFilterClickedEventArg e)
        {
            int widthTool = GetWhithColumn(e.ColumnIndex) + 50;
            if (widthTool < 130) widthTool = 130;

            columnIndex = e.ColumnIndex;

            textBoxCtrl.Clear();
            CheckCtrl.Items.Clear();

            //textBoxCtrl.Text = textBoxCtrlText;
            textBoxCtrl.Size = new System.Drawing.Size(widthTool, 30);
            textBoxCtrl.TextChanged -= textBoxCtrl_TextChanged;
            textBoxCtrl.TextChanged += textBoxCtrl_TextChanged;

            DateTimeCtrl.Size = new System.Drawing.Size(widthTool, 30);
            DateTimeCtrl.Format = DateTimePickerFormat.Custom;
            DateTimeCtrl.CustomFormat = "dd.MM.yyyy";
            DateTimeCtrl.TextChanged -= DateTimeCtrl_TextChanged;
            DateTimeCtrl.TextChanged += DateTimeCtrl_TextChanged;

            CheckCtrl.ItemCheck -= CheckCtrl_ItemCheck;
            CheckCtrl.ItemCheck += CheckCtrl_ItemCheck;
            CheckCtrl.CheckOnClick = true;

            try { GetChkFilter(); } catch (Exception ex) { }

            CheckCtrl.MaximumSize = new System.Drawing.Size(widthTool, GetHeightTable() - 120);
            CheckCtrl.Size = new System.Drawing.Size(widthTool, (CheckCtrl.Items.Count + 1) * 18);

            ApplyButtonCtrl.Text = ButtonCtrlText;
            ApplyButtonCtrl.Size = new System.Drawing.Size(widthTool, 30);
            ApplyButtonCtrl.Click -= ApplyButtonCtrl_Click;
            ApplyButtonCtrl.Click += ApplyButtonCtrl_Click;

            ClearFilterCtrl.Text = ClearFilterCtrlText;
            ClearFilterCtrl.Size = new System.Drawing.Size(widthTool, 30);
            ClearFilterCtrl.Click -= ClearFilterCtrl_Click;
            ClearFilterCtrl.Click += ClearFilterCtrl_Click;

            popup.Items.Clear();
            popup.AutoSize = true;
            popup.Margin = Padding.Empty;
            popup.Padding = Padding.Empty;

            ToolStripControlHost host1 = new ToolStripControlHost(textBoxCtrl);
            host1.Margin = Padding.Empty;
            host1.Padding = Padding.Empty;
            host1.AutoSize = false;
            host1.Size = textBoxCtrl.Size;

            ToolStripControlHost host2 = new ToolStripControlHost(CheckCtrl);
            host2.Margin = Padding.Empty;
            host2.Padding = Padding.Empty;
            host2.AutoSize = false;
            host2.Size = CheckCtrl.Size;

            ToolStripControlHost host3 = new ToolStripControlHost(ApplyButtonCtrl);
            host3.Margin = Padding.Empty;
            host3.Padding = Padding.Empty;
            host3.AutoSize = false;
            host3.Size = ApplyButtonCtrl.Size;

            ToolStripControlHost host4 = new ToolStripControlHost(ClearFilterCtrl);
            host4.Margin = Padding.Empty;
            host4.Padding = Padding.Empty;
            host4.AutoSize = false;
            host4.Size = ClearFilterCtrl.Size;

            ToolStripControlHost host5 = new ToolStripControlHost(DateTimeCtrl);
            host5.Margin = Padding.Empty;
            host5.Padding = Padding.Empty;
            host5.AutoSize = false;
            host5.Size = DateTimeCtrl.Size;

            switch (this.Columns[columnIndex].ValueType.ToString())
            {
                case "System.DateTime":
                    popup.Items.Add(host5);
                    break;

                default:
                    popup.Items.Add(host1);
                    break;
            }
            popup.Items.Add(host2);
            popup.Items.Add(host3);
            popup.Items.Add(host4);

            popup.Show(this, e.ButtonRectangle.X, e.ButtonRectangle.Bottom);
            host2.Focus();
        }

        // Выбор всех
        private void CheckCtrl_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    for (int i = 1; i < CheckCtrl.Items.Count; i++)
                        CheckCtrl.SetItemChecked(i, true);
                }
                else
                {
                    for (int i = 1; i < CheckCtrl.Items.Count; i++)
                        CheckCtrl.SetItemChecked(i, false);
                }
            }
        }

        // Очистить фильтры
        private void ClearFilterCtrl_Click(object sender, EventArgs e)
        {
            Filter.Clear();
            StrFilter = "";
            ApllyFilter();
            popup.Close();
        }

        // Событие при изменении текста в TextBox
        private void textBoxCtrl_TextChanged(object sender, EventArgs e)
        {
            var columnName = this.Columns[columnIndex].Name;
            if (!string.IsNullOrEmpty(this.Columns[columnIndex].DataPropertyName))
            {
                columnName = this.Columns[columnIndex].DataPropertyName;
            }
            (this.DataSource as DataTable).DefaultView.RowFilter = string.Format("convert([" + columnName + "], 'System.String') LIKE '%{0}%'", textBoxCtrl.Text);
        }

        private void DateTimeCtrl_TextChanged(object sender, EventArgs e)
        {
            var columnName = this.Columns[columnIndex].Name;
            if (!string.IsNullOrEmpty(this.Columns[columnIndex].DataPropertyName))
            {
                columnName = this.Columns[columnIndex].DataPropertyName;
            }

            (this.DataSource as DataTable).DefaultView.RowFilter = string.Format("convert([" + columnName + "], 'System.String') LIKE '%{0}%'", DateTimeCtrl.Text);
        }

        // Событие кнопки применить
        private void ApplyButtonCtrl_Click(object sender, EventArgs e)
        {
            StrFilter = "";
            SaveChkFilter();
            ApllyFilter();
            popup.Close();
        }

        // Получаем данные из выбранной колонки
        private List<string> GetDataColumns(int e)
        {
            List<string> ValueCellList = new List<string>();
            string Value;

            // Посик данных в столбце, исключая повторения
            foreach (DataGridViewRow row in this.Rows)
            {
                if (row.Cells[e].Value is null) continue;

                Value = row.Cells[e].Value?.ToString();
                if (Value == "") Value = SpaceText;

                if (!ValueCellList.Contains(Value))
                    ValueCellList.Add(Value);
            }
            return ValueCellList;
        }

        // Получаем высоту таблицы
        private int GetHeightTable()
        {
            return this.Height;
        }

        // Получаем ширину выбранной колонки
        private int GetWhithColumn(int e)
        {
            return this.Columns[e].Width;
        }

        // Запомнить чекбоксы фильтра
        private void SaveChkFilter()
        {
            string col = this.Columns[columnIndex].Name;
            if (!string.IsNullOrEmpty(this.Columns[columnIndex].DataPropertyName))
            {
                col = this.Columns[columnIndex].DataPropertyName;
            }
            string itemChk;
            bool statChk;

            Filter.RemoveAll(x => x.columnName == col);

            for (int i = 1; i < CheckCtrl.Items.Count; i++)
            {
                itemChk = CheckCtrl.Items[i].ToString();
                statChk = CheckCtrl.GetItemChecked(i);
                Filter.Add(new FilterStatus() { columnName = col, valueString = itemChk, check = statChk });
            }
        }

        // Загрузить чекбоксы
        private void GetChkFilter()
        {
            List<FilterStatus> CheckList = new List<FilterStatus>();
            List<FilterStatus> CheckListSort = new List<FilterStatus>();

            // Посик сохранённых данных
            foreach (FilterStatus val in Filter)
            {
                if (this.Columns[columnIndex].Name == val.columnName)
                {
                    if (val.valueString == "") val.valueString = SpaceText;
                    CheckList.Add(new FilterStatus() { columnName = "", valueString = val.valueString, check = val.check });
                }
            }

            // Поиск данных в таблице
            foreach (string ValueCell in GetDataColumns(columnIndex))
            {
                int index = CheckList.FindIndex(item => item.valueString == ValueCell);
                if (index == -1)
                {
                    CheckList.Add(new FilterStatus { valueString = ValueCell, check = true });
                }
            }

            CheckCtrl.Items.Add(CheckCtrlAllText, CheckState.Indeterminate);
            // Сортировка
            switch (this.Columns[columnIndex].ValueType.ToString())
            {
                case "System.Int32":
                    CheckListSort = CheckList.OrderBy(x => Int32.Parse(x.valueString)).ToList();
                    foreach (FilterStatus val in CheckListSort)
                    {
                        if (val.check == true)
                            CheckCtrl.Items.Add(val.valueString, CheckState.Checked);
                        else
                            CheckCtrl.Items.Add(val.valueString, CheckState.Unchecked);
                    }
                    break;

                case "System.DateTime":
                    CheckListSort = CheckList.OrderBy(x => DateTime.Parse(x.valueString)).ToList();
                    foreach (FilterStatus val in CheckListSort)
                    {
                        if (val.check == true)
                            CheckCtrl.Items.Add(DateTime.Parse(val.valueString).ToString("dd.MM.yyyy"), CheckState.Checked);
                        else
                            CheckCtrl.Items.Add(DateTime.Parse(val.valueString).ToString("dd.MM.yyyy"), CheckState.Unchecked);
                    }
                    break;

                default:
                    CheckListSort = CheckList.OrderBy(x => x.valueString).ToList();
                    foreach (FilterStatus val in CheckListSort)
                    {
                        if (val.check == true)
                            CheckCtrl.Items.Add(val.valueString, CheckState.Checked);
                        else
                            CheckCtrl.Items.Add(val.valueString, CheckState.Unchecked);
                    }
                    break;
            }
        }

        // Применить фильтр
        private void ApllyFilter()
        {
            foreach (FilterStatus val in Filter)
            {
                if (val.valueString == SpaceText) val.valueString = "";
                if (val.check == false)
                {
                    // Исключение если bool
                    string valueFilter = "'" + val.valueString + "' ";
                    if (valueFilter == "True")
                    {
                        valueFilter = "1";
                    }
                    if (valueFilter == "False")
                    {
                        valueFilter = "0";
                    }

                    if (StrFilter.Length == 0)
                    {
                        StrFilter = StrFilter + ("[" + val.columnName + "] <> " + valueFilter);
                    }
                    else
                    {
                        StrFilter = StrFilter + (" AND [" + val.columnName + "] <> " + valueFilter);
                    }
                }
            }
            var data = DataSource as DataTable;
            if (data != null)
            {
                data.DefaultView.RowFilter = StrFilter;
            }
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (showFilter)
            {
                var header = new DataGridFilterHeader();
                header.FilterButtonClicked += new EventHandler<ColumnFilterClickedEventArg>(header_FilterButtonClicked);
                e.Column.HeaderCell = header;
            }
            base.OnColumnAdded(e);
        }

        // Скролл после сортировки
        public override void Sort(DataGridViewColumn dataGridViewColumn, System.ComponentModel.ListSortDirection direction)
        {
            int scrl = this.HorizontalScrollBar.Value;
            int scrlOffset = this.HorizontalScrollingOffset;
            base.Sort(dataGridViewColumn, direction);
            this.HorizontalScrollBar.Value = scrl;
            this.HorizontalScrollingOffset = scrlOffset;
        }
    }
}