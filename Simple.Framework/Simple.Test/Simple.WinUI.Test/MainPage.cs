using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Test
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();

            uComboBoxDropPanel1.DropDownControl = uPanel1;
            // https://blog.csdn.net/shizhen_2012/article/details/52385576
        }
    }
}