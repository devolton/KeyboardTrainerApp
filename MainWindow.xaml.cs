using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace KeyboardTrainerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _fullText;
        private string[] _wordsArray;
        private string _currentWord;
        private Key _currentCharCode;
        private int _currentCharIndex = 0;
        private InlineCollection _currentInlineCollection;
        private Run _currentCharRun;
        public MainWindow()
        {
            InitializeComponent();

        }
        public void ChangeFocusChar()
        {
            _currentInlineCollection = focusWordTextBlock.Inlines;

            _currentCharRun = _currentInlineCollection.ElementAt(_currentCharIndex) as Run;
            _currentCharRun.Foreground = Brushes.Gray;
            if (_currentCharIndex == _currentInlineCollection.Count - 1)
            {
                _currentCharIndex = 0;
                UpdateWord();
                return;
            }
            _currentCharIndex++;

            _currentCharRun = _currentInlineCollection.ElementAt(_currentCharIndex) as Run;
            _currentCharCode = (_currentCharIndex == _currentInlineCollection.Count - 1) ? (Key)18 : (Key)_currentCharRun.Text.ToUpper()[0] - 21;
            _currentCharRun.Foreground = Brushes.LightGreen;


        }
        private string[] Cut() => _fullText.Split(new char[] { ' ', ',', '.', ';', ':', '-', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        private void UpdateWord()
        {
            MessageBox.Show("End of word");
        }


        private void OnMainFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == _currentCharCode)
            {
                ChangeFocusChar();

            }
            else
            {

                _currentCharRun.Foreground = Brushes.Red;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fullText = "Hello world my name Antonio";
            _wordsArray = Cut();
            _currentWord = _wordsArray[0];
            _currentCharCode = (Key)_currentWord[0] - 21;
            DrawCharsRun();
            var sb = new StringBuilder();
            for (int i = 2; i < _wordsArray.Length; i++)
            {
                sb.Append(_wordsArray[i] + " ");
            }
            afterRun.Text = sb.ToString();


        }
        private void DrawCharsRun()
        {
            for (int i = 0; i < _currentWord.Length; i++)
            {
                var newRunElement = new Run(_currentWord[i].ToString());
                focusWordTextBlock.Inlines.Add(newRunElement);
                newRunElement.Foreground = (i == 0) ? Brushes.LightGreen : Brushes.Gray;
            }
            //добавить проверку на последнее слово
            focusWordTextBlock.Inlines.Add(new Run(" "));

        }
    }
}