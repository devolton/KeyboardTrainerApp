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
using System.Windows.Shapes;

namespace KeyboardTrainerApp
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public string Speed { get; set; }
        public string MissCount { get; set; }
        public string Time { get; set; }

        public ResultWindow(double speed, int missCount, int time)
        {
            InitializeComponent();
            DataContext = this;
            Speed = speed.ToString() + " symbols/min";
            MissCount = missCount.ToString();
            Time = time.ToString();
        }
       
    }
}
