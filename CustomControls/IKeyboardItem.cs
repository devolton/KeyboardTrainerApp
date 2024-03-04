using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KeyboardTrainerApp.CustomControls;
public interface IKeyboardItem
{
    SolidColorBrush BackColor { get; set; }
    double OpacityValue { get; set; }
    void SetFocusStyle();
    void SetDefaultStyle();
    void SetMisclickStyle();
}

