using Microsoft.VisualBasic;
using System.Diagnostics;
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

namespace KeyboardTrainerApp;

public partial class MainWindow : Window
{
    private int _charsCounter = 0;
    private Stopwatch _stopwatch;
    private string _fullText;
    private string[] _wordsArray;
    private string _currentWord;
    private Key _currentCharCode;
    private int _currentCharIndex = 0;
    private int _currentWordIndex = 0;
    private InlineCollection _currentInlineCollection;
    private Run _currentCharRun;
    private StringBuilder _sb;
    private bool _isKeyDownHandlerActive = true;
    public MainWindow()
    {
        InitializeComponent();

    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _stopwatch = new Stopwatch();
        _sb = new StringBuilder();
        _fullText = "Hello world my name Antonio";
        _wordsArray = Cut();
        _currentWord = _wordsArray[_currentWordIndex];
        _currentCharCode = (Key)_currentWord[_currentCharIndex] - 21;
        DrawCharsRun();

        for (int i = 1; i < _wordsArray.Length; i++)
        {
            _sb.Append(_wordsArray[i] + " ");
        }
        afterRun.Text = _sb.ToString();
        _sb.Clear();
        _stopwatch.Start();


    }
    public void ChangeFocusChar()
    {
        _currentInlineCollection = focusWordTextBlock.Inlines;
        _currentCharRun = _currentInlineCollection.ElementAt(_currentCharIndex) as Run;
        _currentCharRun.Foreground = Brushes.Gray;
        if (_currentCharIndex == _currentInlineCollection.Count - 1)
        {
            if (_currentWordIndex == _wordsArray.Length - 1)
            {
                _stopwatch.Stop();
                var seconds=_stopwatch.Elapsed.Seconds;
                MessageBox.Show(seconds.ToString());
                var charsAtMinute = ((double)_charsCounter / (double)seconds) * 60;
                MessageBox.Show("Game over! Speed: "+charsAtMinute.ToString("F2"));
                _stopwatch.Reset();
                return;
            }
            _currentCharIndex = 0;
            UpdateWord();
            return;
        }
        _currentCharIndex++;
        _charsCounter++;
        _currentCharRun = _currentInlineCollection.ElementAt(_currentCharIndex) as Run;
        _currentCharCode = (_currentCharIndex == _currentInlineCollection.Count - 1) ? (Key)18 : (Key)_currentCharRun.Text.ToUpper()[0] - 21;
        _currentCharRun.Foreground = Brushes.LightGreen;


    }



    private void OnMainFormKeyDown(object sender, KeyEventArgs e)
    {

        if (IsValidUpperKeyDown(e))
            ChangeFocusChar();

        //не совсем коректно работает 
        else if (IsValidLowerKeyDown(e))
            ChangeFocusChar();

        else
            if (Keyboard.Modifiers != ModifierKeys.Shift && _isKeyDownHandlerActive)
            _currentCharRun.Foreground = Brushes.Red;

    }

 
    private void DrawCharsRun()
    {
        for (int i = 0; i < _currentWord.Length; i++)
        {
            var newRunElement = new Run(_currentWord[i].ToString());
            focusWordTextBlock.Inlines.Add(newRunElement);
            newRunElement.Foreground = (i == 0) ? Brushes.LightGreen : Brushes.Gray;
            if (i == 0) _currentCharRun = newRunElement;
        }

        
            focusWordTextBlock.Inlines.Add(new Run(" "));

    }
    private void UpdateWord()
    {

        focusWordTextBlock.Inlines.Clear();
        AddWordToPrevRunBlock(_currentWord + " ");
        _currentWord = _wordsArray[++_currentWordIndex];
        _currentCharCode = (_currentWord[_currentCharIndex] < 'a') ? (Key)_currentWord[_currentCharIndex] - 21 : (Key)_currentWord[_currentCharIndex] - 53;
        DrawCharsRun();
        CutWordFromAfterRunBlock(_currentWord);

    }

    private void AddWordToPrevRunBlock(string word)
    {
        if (_currentWordIndex == 0)
            prevRun.Text += (" " + word);
        else
            prevRun.Text += " "+word;
    }
    private void CutWordFromAfterRunBlock(string word)
    {
        _sb.Append(afterRun.Text);
        var newAfterStr = _sb.Remove(0, word.Length + 1).ToString();
        afterRun.Text = newAfterStr;
        _sb.Clear();


    }

    private bool IsUpperChar(char ch) => ch >= 'A' && ch <= 'Z';
    private bool IsNotUpperChar(char ch) => !(ch >= 'A' && ch <= 'Z');

    private bool IsValidUpperKeyDown(KeyEventArgs e) => _isKeyDownHandlerActive && IsUpperChar(_currentCharRun.Text[0]) && e.Key == _currentCharCode && (Keyboard.Modifiers == ModifierKeys.Shift || Keyboard.IsKeyToggled(Key.CapsLock));
    private bool IsValidLowerKeyDown(KeyEventArgs e) => _isKeyDownHandlerActive && IsNotUpperChar(_currentCharRun.Text[0]) && e.Key == _currentCharCode && (!Keyboard.IsKeyDown(Key.LeftShift) || !Keyboard.IsKeyToggled(Key.CapsLock));

    private string[] Cut() => _fullText.Split(new char[] { ' ', ',', '.', ';', ':', '-', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);



}