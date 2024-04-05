using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simple.Dapper;
using Simple.WinUI.Forms;

namespace Simple.Tool.Pages.CodeGeneratorPages
{
    public partial class TableColumnEditor : BaseForm
    {
        private DbTable table;

        public TableColumnEditor(DbTable table)
        {
            InitializeComponent();
            this.table = table;
            txtTableName.Text = table.TableName;
            txtClassName.Text = table.ClassName;
            txtDescription.Text = table.Description;
            gdv_columns.DataSource = table.Columns;
        }

        public event Action SaveChanged;

        private void TableColumnEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            table.TableName = txtTableName.Text;
            table.ClassName = txtClassName.Text;
            table.Description = txtDescription.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanged?.Invoke();
        }
    }
}