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
    /// Логика взаимодействия для SingleKeyboardItem.xaml
    /// </summary>
    public partial class SingleKeyboardItem : UserControl
    {
        public string MainContent { get; set; }
        public SolidColorBrush BackColor { get; set; }
        
        public SingleKeyboardItem()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
