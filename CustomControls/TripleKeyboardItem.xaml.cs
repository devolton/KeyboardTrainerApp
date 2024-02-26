using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyboardTrainerApp.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для TripleKeyboardItem.xaml
    /// </summary>
    public partial class TripleKeyboardItem : UserControl
    {
        public string TopLeftContent { get; set; }
        public string BottomLeftContent { get; set; }   
        public string BottomRightContent { get; set; }
        public SolidColorBrush BackColor { get; set; }
        public TripleKeyboardItem()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
