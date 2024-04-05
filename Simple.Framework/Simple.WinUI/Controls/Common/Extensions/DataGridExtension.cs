using Simple.WinUI.Controls.DataGridView.DataGridViewPrint;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Reflection;

namespace Simple.WinUI
{
    /// <summary>DataGrid的扩展</summary>
    public static class DataGridExtension
    {
        private static bool ClassHasNestedCollections(IEnumerable<PropertyInfo> properties)
        {
            return properties.ToList().Any(x => x.PropertyType != typeof(string) &&
                                                 typeof(IEnumerable).IsAssignableFrom(x.PropertyType) ||
                                                 typeof(IEnumerable<>).IsAssignableFrom(x.PropertyType));
        }

        private static object SetCellValue<T>(PropertyInfo property, T data) where T : class
        {
            var value = property.GetValue(data, null);
            return value is Guid ? value.ToString() : value;
        }

        private static string GetColumnName(PropertyInfo propertyInfo)
        {
            var descriptionAttribute = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute)).FirstOrDefault();
            if (descriptionAttribute == null)
            {
                return propertyInfo.Name;
            }
            var description = descriptionAttribute as DescriptionAttribute;
            return description == null ? propertyInfo.Name : description.Description;
        }

        /// <summary>
        /// 打印文档dg.PageTitle.HeaderStr = "Title for report";
        ///dg.PageTitle.SubTitle1 = "subtitle 1";
        ///    dg.PageTitle.SubTitle2 = "subtitle 2";
        ///   dg.PageTitle.TitleFont = new System.Drawing.Font("B Mitra", 12f);
        ///    dg.WrapText = false;
        ///    dg.PrinPreview();
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="printDocument"></param>
        public static DataGridViewPrint Print(this DataGridView dataGridView, PrintDocument printDocument = null)
        {
            DataGridViewPrint dgp = new DataGridViewPrint(dataGridView, printDocument);
            return dgp;
        }

        /// <summary>
        /// Retunrs a collection of data rows from a data grid view based on the specific data grid
        /// cell where clause. _dataGridView.FindRows(x =&gt; x.Value.ToString() == "Grant");
        /// </summary>
        /// <param name="dataGridView">The data grid view being searched</param>
        /// <param name="whereExpression">Where clause used to search data rows by cell values</param>
        /// <returns>A collection of data grid rows</returns>
        public static IEnumerable<DataGridViewRow> FindRows(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression)
        {
            var dataRowsFound = new List<DataGridViewRow>();
            dataGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(x =>
            {
                if (x != null && x.Cells != null)
                {
                    var cells = x.Cells.Cast<DataGridViewCell>().Where(cell => cell.Value != null)
                                                                .Where(whereExpression);
                    if (cells.Any())
                    {
                        dataRowsFound.Add(x);
                    }
                }
            });

            return dataRowsFound;
        }

        /// <summary>
        /// Updates cells value in a grid based on a specific where clause and an update value.
        /// _dataGridView.UpdateCells(x =&gt; x.Value.ToString() == "test@email.com", "ggtgivens24@gmail.com");
        /// </summary>
        /// <param name="dataGridView">The data grid to update</param>
        /// <param name="whereExpression">Where clause used to search data cells</param>
        /// <param name="value">Update value for collection of cells</param>
        public static void UpdateCells(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression, object value)
        {
            dataGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(x =>
            {
                if (x != null && x.Cells != null)
                {
                    var cells = x.Cells.Cast<DataGridViewCell>().Where(cell => cell.Value != null)
                                                                .Where(whereExpression);
                    if (cells.Any())
                    {
                        cells.ToList().ForEach(cell =>
                        {
                            cell.Value = value;
                        });
                    }
                }
            });
        }

        /// <summary>
        /// Removes rows from a data grid based on a specific data grid cell where clause.
        /// _dataGridView.RemoveRows(x =&gt; x.Value.ToString() == "test@email.com");
        /// </summary>
        /// <param name="dataGridView">The data grid to be updated</param>
        /// <param name="whereExpression">Where clause used to search data cells</param>
        public static void RemoveRows(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression)
        {
            var rowsFound = dataGridView.FindRows(whereExpression);
            if (rowsFound.Any())
            {
                rowsFound.ToList().ForEach(row =>
                {
                    dataGridView.Rows.RemoveAt(row.Index);
                });
            }
        }

        /// <summary>
        /// Sets the font and background color for a data grid's cells based on a specific where
        /// clause. _dataGridView.FormatCells(x =&gt; x.Value.ToString() == "test@email.com",
        /// Color.White, Color.Red);
        /// </summary>
        /// <param name="dataGridView">The data grid to format</param>
        /// <param name="whereExpression">Where caluse used to locate the cells for formatting</param>
        /// <param name="foreColor">Font color for cells</param>
        /// <param name="backColor">Background color for cells</param>
        public static void FormatCells(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression, System.Drawing.Color foreColor, System.Drawing.Color backColor)
        {
            dataGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(x =>
            {
                if (x != null && x.Cells != null)
                {
                    var cells = x.Cells.Cast<DataGridViewCell>().Where(cell => cell.Value != null)
                                                                .Where(whereExpression);
                    if (cells.Any())
                    {
                        cells.ToList().ForEach(cell =>
                        {
                            cell.Style.ForeColor = foreColor;
                            cell.Style.BackColor = backColor;
                        });
                    }
                }
            });
        }

        /// <summary>
        /// Binds data of any class without nested collections to the data grid. If a class property
        /// has a description attribute, that will be used as the column header.
        /// </summary>
        /// <param name="dataGrid">The data grid to apply the data</param>
        /// <typeparam name="T">Type of the class to bind</typeparam>
        /// <param name="dataToBindToGrid">Collection of data to bind to the data grid</param>
        public static void BindData<T>(this DataGridView dataGridView, IEnumerable<T> dataToBindToGrid) where T : class
        {
            var properties = typeof(T).GetProperties().ToList();
            if (ClassHasNestedCollections(properties))
            {
                throw new InvalidOperationException("Class cannot have nested collections.");
            }

            var columns = new Dictionary<PropertyInfo, string>();
            properties.ForEach(p => columns.Add(p, (GetColumnName(p))));
            columns.ToList().ForEach(column => dataGridView.Columns.Add(column.Key.Name, column.Value));
            dataGridView.Rows.Add(dataToBindToGrid.Count());
            var rowIndex = 0;
            dataToBindToGrid.ToList().ForEach(data =>
            {
                var columnIndex = 0;
                properties.ForEach(prop =>
                {
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Value = SetCellValue(prop, data);
                    columnIndex++;
                });
                rowIndex++;
            });
        }

        public static void CoolGrid(this DataGridView dgv)
        {
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            dgv.AllowDrop = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.BackgroundColor = System.Drawing.Color.White;
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            cellStyle.BackColor = System.Drawing.Color.White;
            cellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            cellStyle.ForeColor = System.Drawing.Color.Black;
            cellStyle.SelectionBackColor = System.Drawing.Color.Yellow;
            cellStyle.SelectionForeColor = System.Drawing.Color.Black;
            cellStyle.WrapMode = DataGridViewTriState.True;
            dgv.DefaultCellStyle = cellStyle;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.BackColor = System.Drawing.SystemColors.Window;
            headerStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            headerStyle.ForeColor = System.Drawing.Color.Black;
            headerStyle.SelectionBackColor = System.Drawing.Color.White;
            headerStyle.SelectionForeColor = System.Drawing.Color.Black;
            headerStyle.WrapMode = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle = headerStyle;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.TabIndex = 0;
            dgv.TabStop = false;
        }

        public static void MoveToUp(this DataGridView dgv)
        {
            if (dgv.Rows.Count > 0)
            {
                int indexUp = dgv.SelectedRows[0].Index;
                if (indexUp > 0)
                {
                    dgv.Rows[indexUp].Selected = false;
                    dgv.Rows[indexUp - 1].Selected = true;
                }
            }
        }

        public static void MoveToDown(this DataGridView dgv)
        {
            if (dgv.Rows.Count > 0)
            {
                int indexDown = dgv.SelectedRows[0].Index;
                if (indexDown >= 0 && indexDown < dgv.Rows.Count - 1)
                {
                    dgv.Rows[indexDown].Selected = false;
                    dgv.Rows[indexDown + 1].Selected = true;
                }
            }
        }

        public static void HideColumn(this DataGridView dgv, string columnName)
        {
            dgv.Columns[columnName].Visible = false;
        }
    }
}