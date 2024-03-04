using KeyboardTrainerApp.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
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
    public partial class DoubleKeyboardItem : UserControl,IKeyboardItem
    {
        //унаследовать от абстрактного класса CustomKeyboardItem
        public string TopLeftContent { get; set; }
        public string BottomRightContent { get; set; }
        public SolidColorBrush BackColor {  get; set; }
        public double OpacityValue { get; set; }
        
        public DoubleKeyboardItem()
        {
            InitializeComponent();
            DataContext = this;
            OpacityValue = 0.8;
        }

        public void SetFocusStyle()
        {
            KeyboardBorder.SetResourceReference(StyleProperty, "CustomKeyboardElementOnFocusBorder");
            var labelsContainer = KeyboardCanvas.Children.OfType<Label>();
            foreach(var oneLabel in labelsContainer)
            {
                oneLabel.SetResourceReference(StyleProperty, "CustomKeyboardElementCharOnFocusLabel");
            }
            OpacityValue = 1;
        }

        public void SetDefaultStyle()
        {

            KeyboardBorder.SetResourceReference(StyleProperty, "CustomKeyboardElementBorder");
            var labelsContainer = KeyboardCanvas.Children.OfType<Label>();
            foreach (var oneLabel in labelsContainer)
            {
                oneLabel.SetResourceReference(StyleProperty, "CustomKeyboardElementCharLabel");
            }
            OpacityValue = 0.8;
        }

        public void SetMisclickStyle()
        {
            KeyboardBorder.SetResourceReference(StyleProperty, "CustomKeyboardElementMisclickBorder");
            
        }
    }
}
