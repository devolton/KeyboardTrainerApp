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

namespace KeyboardTrainerApp
{
    /// <summary>
    /// Логика взаимодействия для DoubleKeyboardItem.xaml
    /// </summary>
    public partial class DoubleKeyboardItem : UserControl
    {
        public string TopLeftContent { get; set; }
        public string BottomRightContent { get; set; }
        public SolidColorBrush BackColor {  get; set; }
        
        public DoubleKeyboardItem()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
