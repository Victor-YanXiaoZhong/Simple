using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;

namespace Simple.WpfUI.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class displaySwichModel
    {
        public Visibility displaySwich { get; set; } = Visibility.Hidden;
        public string dispalyText { get; set; }
        public Brush displayColor { get; set; }

        public void btnStartEv()
        {
            dispalyText = "就好";
        }

        public void btnSopEv()
        {
            dispalyText = "就好stop";
        }
    }
}
