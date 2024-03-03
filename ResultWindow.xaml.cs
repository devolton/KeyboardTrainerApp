using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KeyboardTrainerApp
{
    public partial class ResultWindow : Window
    {
        private readonly string _descriptionFilePath;
        private string[] _speedLevelDescritionArray;
        public string SpeedLogoPath { get; set; }
        private PrintSpeedLevel _printSpeedLevel;
        public string Speed { get; set; }
        public string MissCount { get; set; }
        public string Time { get; set; }


        public ResultWindow(double speed, int missCount, int time)
        {
            _descriptionFilePath = "../../../Resource/speedLevelDescription.json";
            _speedLevelDescritionArray = GetSpeedLevelDescriptionFile();
            FindOutPrintLevel(speed);
            InitializeComponent();
            InitSpeedDependentFields();
            DataContext = this;
            Speed = speed.ToString("F2") + " symbols/min";
            MissCount = missCount.ToString();
            Time = time.ToString();
        }
        private void FindOutPrintLevel(double speed)
        {
            if (speed >= 0 && speed <= 120) _printSpeedLevel = PrintSpeedLevel.Low;
            else if (speed > 120 && speed <= 160) _printSpeedLevel = PrintSpeedLevel.Middle;
            else if (speed > 160 && speed <= 260) _printSpeedLevel = PrintSpeedLevel.MiddlePlus;
            else _printSpeedLevel = PrintSpeedLevel.High;
        }
        private string[] GetSpeedLevelDescriptionFile()
        {
            using var sr = new StreamReader(_descriptionFilePath);
            var jsonContent = sr.ReadToEnd();
            return JsonSerializer.Deserialize<string[]>(jsonContent) ?? new string[1];

        }
        private void InitSpeedDependentFields()
        {
            switch (_printSpeedLevel)
            {
                case PrintSpeedLevel.Low:
                    {
                        SpeedLogoPath = "Resource/lowestSpeed.png";
                        ResultCard.SetResourceReference(BackgroundProperty, "LowSpeedLevelCardBackground");
                        SpeedLevelDescritionTextBlock.Text = _speedLevelDescritionArray[0];

                        break;
                    }
                case PrintSpeedLevel.Middle:
                    {
                        SpeedLogoPath = "Resource/middle.png";
                        ResultCard.SetResourceReference(BackgroundProperty, "MiddleSpeedLevelCardBackground");
                        SpeedLevelDescritionTextBlock.Text= _speedLevelDescritionArray[1];
                        break;
                    }
                case PrintSpeedLevel.MiddlePlus:
                    {
                        SpeedLogoPath = "Resource/highSpeed.png";
                        ResultCard.SetResourceReference(BackgroundProperty, "MiddlePlusSpeedLevelCardBackground");
                        SpeedLevelDescritionTextBlock.Text = _speedLevelDescritionArray[2];
                        break;
                    }
                default:
                    {
                        SpeedLogoPath = "Resource/highest.png";
                        ResultCard.SetResourceReference(BackgroundProperty, "HighLevelSpeedCardBackground");
                        SpeedLevelDescritionTextBlock.Text = _speedLevelDescritionArray[3];
                        break;
                    }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var button = sender as Button;
            button?.SetResourceReference(StyleProperty, "CustomOnEnterWhiteButton");

        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var button = sender as Button;
            button?.SetResourceReference(StyleProperty, "CustomWhiteButton");

        }
    }
}
