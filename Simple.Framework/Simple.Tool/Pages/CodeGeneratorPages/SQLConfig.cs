using Simple.Utils;
using Simple.WinUI.Controls.ComboBox;
using Simple.WinUI.Forms;
using System.Configuration;

namespace Simple.Tool.Pages.CodeGeneratorPages
{
    public partial class SQLConfig : BaseForm
    {
        private AppProfileDataDb appSet = AppProfileDataDb.GetInstance();

        private List<ComBoxListItem> sqlType = new() {
            new ComBoxListItem{ CmbText="MSSQL",CmbValue = "Microsoft.Data.SqlClient"},
            new ComBoxListItem{ CmbText="MySQL",CmbValue = "MySql.Data.MySqlClient"},
            new ComBoxListItem{ CmbText="Sqlite",CmbValue = "Microsoft.Data.Sqlite"},
        };

        public SQLConfig()
        {
            InitializeComponent();
            cmbDataType.DataSource = sqlType;
            cmbDataType.DisplayMember = "CmbText";
            cmbDataType.ValueMember = "CmbValue";
            cmbDataType.SelectedIndex = 0;

            LoadConnection();
        }

        public event Action<ConnectionStringSettings> ConnectSql;

        public event Action<ConnectionStringSettings> TestConnectSql;

        private ConnectionStringSettings connectSet
        {
            get
            {
                var connectStr = "";
                switch (cmbDataType.Text)
                {
                    case "MSSQL":
                        connectStr = $"Server={txtServer.Text};Initial Catalog={txtDatabase.Text};";

                        if (txtUser.Text.IsNotEmpty())
                        {
                            connectStr += $"User Id={txtUser.Text};pwd={txtPwd.Text};";
                        }
                        else
                        {
                            connectStr += "Integrated Security=True;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=true";
                        }

                        break;

                    case "MySQL":
                        connectStr = $"Server={txtServer.Text};Database={txtDatabase.Text};Uid={txtUser.Text};Pwd={txtPwd.Text};";
                        break;

                    case "Sqlite":
                        connectStr = $"Data Source={txtServer.Text};";
                        break;

                    default:
                        break;
                }

                var connectSet = new ConnectionStringSettings
                {
                    ConnectionString = connectStr,
                    ProviderName = cmbDataType.SelectedValue as string
                };
                txtConnectStr.Text = connectStr;
                return connectSet;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestConnectSql.Invoke(connectSet);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConnection();
            ConnectSql?.Invoke(connectSet);
            Close();
        }

        private void LoadConnection()
        {
            var set = appSet.TempData.Get<SqlConnectionSet>("SQL_CONNECTION");
            if (set != null)
            {
                txtServer.Text = set.Server;
                cmbDataType.Text = set.DbType;
                txtUser.Text = set.UserId;
                txtPwd.Text = set.Pwd;
                txtDatabase.Text = set.DataBase;
            }
        }

        private void SaveConnection()
        {
            var set = new SqlConnectionSet
            {
                Server = txtServer.Text,
                DbType = cmbDataType.Text,
                UserId = txtUser.Text,
                Pwd = txtPwd.Text,
                DataBase = txtDatabase.Text,
            };

            appSet.TempData.Set("SQL_CONNECTION", set);
        }
    }

    public class SqlConnectionSet
    {
        public string Server { get; set; }
        public string DbType { get; set; }
        public string UserId { get; set; }
        public string DataBase { get; set; }
        public string Pwd { get; set; }
    }
}