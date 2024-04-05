using ICSharpCode.TextEditor;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Simple.Dapper;
using Simple.Generator;
using Simple.Generator.Models;
using Simple.Tool.Models;
using Simple.Tool.Pages.CodeGeneratorPages;
using Simple.Utils;
using Simple.WinUI.Forms;
using Simple.WinUI.Helper;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;

namespace Simple.Tool.Pages
{
    public partial class CodeGeneratorPage : BaseForm
    {
        private ContextMenuStrip rightMenuStrip;

        private AppProfileDataDb genaraltorSet = AppProfileDataDb.GetInstance("genaraltorModel");

        private EngineCore engine;

        private List<DbTable> dbTables = new List<DbTable>();

        /// <summary>模板对应的编辑器，用于编辑器赋值取值</summary>
        private Dictionary<string, TextEditorControl> templateEditorDic = new();

        /// <summary>存储生成的模型的sqliteDb</summary>
        protected DbComponent generalSqlLiteDb;

        /// <summary>连接的数据库db</summary>
        protected DbComponent currentDb;

        public CodeGeneratorPage()
        {
            InitializeComponent();

            engine = new EngineCore();
        }

        /// <summary>加载右键菜单</summary>
        private void LoadGdvMenu()
        {
            rightMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem menuItem1 = new ToolStripMenuItem("表模型配置");
            menuItem1.Click += EditDbTable_Click;
            rightMenuStrip.Items.Add(menuItem1);
            ToolStripMenuItem menuItem2 = new ToolStripMenuItem("生成文件");
            menuItem2.Click += GeneralFile_Click;
            rightMenuStrip.Items.Add(menuItem2);
            ToolStripMenuItem menuItem3 = new ToolStripMenuItem("保存模型");
            menuItem2.Click += SaveModel_Click;
            rightMenuStrip.Items.Add(menuItem3);
            gdv_tables.CellMouseUp += new DataGridViewCellMouseEventHandler((s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.gdv_tables.Rows[e.RowIndex].Selected = true;
                    this.gdv_tables.CurrentCell = this.gdv_tables.Rows[e.RowIndex].Cells[1];
                    this.rightMenuStrip.Show(this.gdv_tables, e.Location);
                    rightMenuStrip.Show(Cursor.Position);
                }
            });
        }

        private void LoadTemplateGroup()
        {
            var rootTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            var groups = Directory.GetDirectories(rootTemplatePath);
            foreach (var group in groups)
            {
                var tmp = new DirectoryInfo(group);
                cmbTemplateRoot.Items.Add(tmp.Name);
            }
            cmbTemplateRoot.SelectedIndex = 0;
            GeneralTab(cmbTemplateRoot.Text);
        }

        /// <summary>加载本地存储的模型数据</summary>
        private void LoadModelData()
        {
            UIHelper.RunWithLoading(this, action: () =>
            {
                var tables = genaraltorSet.TempData.Get<List<DbTable>>("dbTables");
                dbTables = tables;
                gdv_tables.DataSource = tables;
            });
        }

        /// <summary>加载数据库数据</summary>
        private void LoadDbData()
        {
            UIHelper.RunWithLoading(this, action: () =>
            {
                var tables = currentDb.GetDbtables();
                foreach (var table in tables)
                {
                    table.Columns = currentDb.GetDbColumns(table.TableName);
                }
                dbTables = tables;
                gdv_tables.DataSource = tables;
            });
        }

        private void GeneralFile_Click(object sender, EventArgs e)
        {
            this.AlertToast("未完成");
        }

        private void OpenSlqDb_Click(object sender, EventArgs e)
        {
            var win = new SQLConfig();
            win.TestConnectSql += (connect) =>
            {
                var db = DbComponent.Instance(connect);
                if (db.TestConection()) this.AlertToast("测试成功");
            };
            win.ConnectSql += (connect) =>
            {
                LoadDbData();
            };
            win.ShowDialog();
        }

        private void gdv_tables_DoubleClick(object sender, EventArgs e)
        {
            var currentRow = gdv_tables.CurrentRow.DataBoundItem as DbTable;
            if (currentRow == null) return;

            UIHelper.RunWithLoading(this, "正在生成", async () =>
            {
                dynamic model = new ExpandoObject();
                model.Table = currentRow;
                dynamic viewBage = new ExpandoObject();
                templateEditorDic.Each(async tempDic =>
                {
                    try
                    {
                        var templateProp = GetTemplateProp(tempDic.Key);
                        model.Config = new Config { NameSpase = templateProp.NameSpace };
                        var result = await engine.GenerateOutput(tempDic.Key, model, viewBage);
                        tempDic.Value.Text = result;
                    }
                    catch (Exception ex)
                    {
                        tempDic.Value.Text = $"生成失败，ex:{ex.Message}";
                    }
                });
            });
        }

        private void EditDbTable_Click(object sender, EventArgs e)
        {
            var currentRow = gdv_tables.CurrentRow.DataBoundItem as DbTable;
            if (currentRow == null) return;

            var editor = new TableColumnEditor(currentRow);
            editor.SaveChanged += SaveModel;
            editor.ShowDialog();
        }

        private void SaveModel()
        {
            genaraltorSet.TempData.Set("dbTables", dbTables);
            this.AlertToast("保存成功");
        }

        private void SaveModel_Click(object sender, EventArgs e)
        {
            SaveModel();
        }

        private void cmbTemplateRoot_SelectedValueChanged(object sender, EventArgs e)
        {
            engine.loadTemplate(cmbTemplateRoot.Text);
            GeneralTab(cmbTemplateRoot.Text);
        }

        private void EditTemplate_Click(object sender, EventArgs e)
        {
            if (uTabControl.TabPages.Count == 0)
            {
                this.AlertToast("未有选中的模板");
                return;
            }

            var templateName = uTabControl.TabPages[uTabControl.SelectedIndex].Text;

            var templateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", cmbTemplateRoot.Text, templateName);

            Process.Start("notepad.exe", templateFile);
        }

        private void ImportTemplate_Click(object sender, EventArgs e)
        {
            var success = UIHelper.SelectFiles(this, $"请选择要导入到[{cmbTemplateRoot.Text}]组中的模板文件", "所有文件(*.*)|*.*", out string[] files);

            if (!success || files.Length == 0) return;

            files.Each(f =>
            {
                var tmpFile = new FileInfo(f);
                var desFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", cmbTemplateRoot.Text, tmpFile.Name);
                File.Copy(f, desFile, true);
            });

            cmbTemplateRoot_SelectedValueChanged(null, null);
        }

        private void uTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uTabControl.TabPages.Count == 0)
            {
                return;
            }

            var templateName = uTabControl.TabPages[uTabControl.SelectedIndex].Text;

            var setting = GetTemplateProp(templateName);
            pptg_template.SelectedObject = setting;
        }

        /// <summary>获取模板配置</summary>
        private TemplateSetting GetTemplateProp(string templateName)
        {
            var setting = genaraltorSet.TempData.Get<TemplateSetting>($"TemplateSetting:{cmbTemplateRoot.Text}_{templateName}");
            if (setting == null)
            {
                setting = new TemplateSetting
                {
                    TemplateName = templateName
                };
            }
            return setting;
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            var prop = pptg_template.SelectedObject as TemplateSetting;
            genaraltorSet.TempData.Set($"TemplateSetting:{cmbTemplateRoot.Text}_{prop.TemplateName}", prop);
            this.AlertToast("保存成功");
        }

        protected override void OnShown(EventArgs e)
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.Sqlite", Microsoft.Data.Sqlite.SqliteFactory.Instance);
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);

            generalSqlLiteDb = DbComponent.Instance("defaultDb");
            currentDb = DbComponent.Instance("currentDb");

            LoadGdvMenu();
            LoadModelData();
            LoadTemplateGroup();
        }

        #region 根据模板生成Tab

        /// <summary>模板生成tab控件</summary>
        /// <param name="groupName"></param>
        private void GeneralTab(string groupName)
        {
            if (groupName.IsNullOrEmpty()) return;
            var rootTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", groupName);
            var templateFiles = Directory.GetFiles(rootTemplatePath);

            uTabControl.TabPages.Clear();
            templateEditorDic.Clear();
            for (int i = 0; i < templateFiles.Length; i++)
            {
                var template = templateFiles[i];
                var tmpInfo = new FileInfo(template);

                var editor = new TextEditorControl { Dock = DockStyle.Fill };
                editor.SetHighlighting("C#");
                var tab = new TabPage
                {
                    Location = new Point(4, 4),
                    Padding = new Padding(3),
                    TabIndex = i,
                    Text = tmpInfo.Name,
                };
                tab.Controls.Add(editor);
                templateEditorDic[tmpInfo.Name.Replace(tmpInfo.Extension, "")] = editor;

                uTabControl.TabPages.Add(tab);
            }
            uTabControl.TabIndex = 0;
            uTabControl_SelectedIndexChanged(null, null);
        }

        #endregion 根据模板生成Tab
    }
}