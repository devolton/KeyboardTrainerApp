﻿using KeyboardTrainerApp.CustomControls;
using Microsoft.VisualBasic;
using Microsoft.Xaml.Behaviors.Media;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace KeyboardTrainerApp;

public partial class MainWindow : Window
{
    private DispatcherTimer _timer;
    private int _charsCounter = 0;
    private Stopwatch _stopwatch;
    private string _fullText;
    private string[] _wordsArray;
    private string _currentWord;
    private Key _currentCharCode;
    private char _incorrectPressedChar;
    private int _currentCharIndex = 0;
    private int _currentWordIndex = 0;
    private int _missclickCounter = 0;
    private InlineCollection _currentInlineCollection;
    private Run _currentCharRun;
    private StringBuilder _sb;
    private bool _isKeyDownHandlerActive = true;
    private List<UserControl> _keyboardCollection;

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
            if (_currentWordIndex == _wordsArray.Length - 1)
            {
                _stopwatch.Stop();
                StartButton.FontSize = 18;
                StartButton.Content = "Play again";
                StartButton.IsEnabled = true;
                _timer.Stop();
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
        prevRun.Text += " " + word;
    }
    private void CutWordFromAfterRunBlock(string word)
    {
        _sb.Append(afterRun.Text);
        var newAfterStr = _sb.Remove(0, word.Length + 1).ToString();
        afterRun.Text = newAfterStr;
        _sb.Clear();

    }
    private void UpdatePrintSpeed()
    {
        var seconds = _stopwatch.Elapsed.Seconds;
        var speed = ((double)_charsCounter / (double)seconds) * 60;
        PrintSpeedLabel.Content = speed.ToString("F2") + " sb/min";
    }

    private bool IsUpperChar(char ch) => ch >= 'A' && ch <= 'Z';
    private bool IsNotUpperChar(char ch) => !(ch >= 'A' && ch <= 'Z');

    private bool IsValidUpperKeyDown(KeyEventArgs e) => _isKeyDownHandlerActive && IsUpperChar(_currentCharRun.Text[0]) && e.Key == _currentCharCode && (Keyboard.Modifiers == ModifierKeys.Shift || Keyboard.IsKeyToggled(Key.CapsLock));
    private bool IsValidLowerKeyDown(KeyEventArgs e) => _isKeyDownHandlerActive && IsNotUpperChar(_currentCharRun.Text[0]) && e.Key == _currentCharCode && (!Keyboard.IsKeyDown(Key.LeftShift) || !Keyboard.IsKeyToggled(Key.CapsLock));

    private string[] Cut() => _fullText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    private async Task UpdateTimeLabelAsync()
    {
        string newData = _stopwatch.Elapsed.Seconds.ToString();
        await Dispatcher.BeginInvoke(new Action(() =>
        {
            UpdatePrintSpeed();
            TimeLabel.Content = newData;
        }), DispatcherPriority.Normal);
    }

    private void UpdateGameData()
    {
        prevRun.Text = String.Empty;
        _fullText = "Hello think it wanna be some interesting and I know you must doing this";
        _wordsArray = Cut();
        _missclickCounter = 0;
        _charsCounter = 0;
        _currentCharIndex = 0;
        _currentWordIndex = 0;
        MissclickCountLabel.Content = _missclickCounter.ToString();
        TimeLabel.Content = _stopwatch.Elapsed.Seconds.ToString();
        _currentWord = _wordsArray[_currentWordIndex];
        _currentCharCode = (Key)_currentWord[_currentCharIndex] - 21;
        focusWordTextBlock.Inlines.Clear();
        DrawCharsRun();
        FillAfterRunBlock();

    }
    private void FillAfterRunBlock()
    {
        for (int i = 1; i < _wordsArray.Length; i++)
        {
            _sb.Append(_wordsArray[i] + " ");
        }
        afterRun.Text = _sb.ToString();
        _sb.Clear();
    }

    private void AddBoxShadowForFocusKey(char currentChar)
    {
        string curChar = Convert.ToString(currentChar).ToUpper();
        if (_keyboardCollection.Any(oneKey => (string)oneKey?.Tag == curChar))
        {
            var focusElement = _keyboardCollection.First(oneKeyboardElement => (string)oneKeyboardElement?.Tag == curChar);
            if (focusElement is IKeyboardItem)
            {
                var item = focusElement as IKeyboardItem;
                item.OpacityValue = 1;
            }
            focusElement.Effect = new DropShadowEffect()
            {
                ShadowDepth = 5,
                Color = Colors.Black,
                Opacity = 0.7,
                Direction = 200

            };

        }
    }
    private void AddBoxShadowForShiftKey()
    {
        try
        {
            var focusElementCollection = _keyboardCollection.Where(oneKey => (string)oneKey.Tag == "Shift");
            foreach (var oneFocusElement in focusElementCollection)
            {
                oneFocusElement.Effect = new DropShadowEffect()
                {
                    ShadowDepth = 5,
                    Color = Colors.Black,
                    Opacity = 0.7,
                    Direction = 200
                };
            }
        }
        catch
        {
            return;
        }
    }
    private void RemoveBoxShadowForLostFocusKey(char lostFocusChar)
    {
        string lostFocusCharStr = Convert.ToString(lostFocusChar).ToUpper();
        if (_keyboardCollection.Any(oneKey => (string)oneKey?.Tag == lostFocusCharStr))
        {
            _keyboardCollection.First(oneKey => (string)oneKey?.Tag == lostFocusCharStr).Effect = null;
        }

    }
    private void AddIncorrectlyPressedEffect()
    {
        string incorrectCharStr = Convert.ToString(_incorrectPressedChar).ToUpper();
        if (_keyboardCollection.Any(oneKey => (string)oneKey?.Tag == incorrectCharStr))
        {
            var element = _keyboardCollection.First(oneKey => (string)oneKey?.Tag == incorrectCharStr);
            element.SetResourceReference(BackgroundProperty, "IncorrectPushedBackground");



        }
    }
    private void RemoveIncorrectlyPressedEffect()
    {
        string incorrectCharStr = Convert.ToString(_incorrectPressedChar).ToUpper();
        if (_keyboardCollection.Any(oneKey => (string)oneKey?.Tag == incorrectCharStr))
        {
            var element = _keyboardCollection.First(oneKey => (string)oneKey?.Tag == incorrectCharStr);
            element.Background = null;


        }
    }

    //Event Handler Block
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _stopwatch = new Stopwatch();
        _sb = new StringBuilder();
        _timer = new();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _keyboardCollection = KeyboardGrid.Children.OfType<UserControl>().ToList();

    }
    private void OnMainFormKeyDown(object sender, KeyEventArgs e)
    {
        if (!StartButton.IsEnabled)
        {
            if (IsValidUpperKeyDown(e))
            {
                //добавить проверку на наличии неправильно нажатой клавиши
                RemoveIncorrectlyPressedEffect();
                if (_currentCharIndex != _currentWord.Length)
                    RemoveBoxShadowForLostFocusKey(_currentWord[_currentCharIndex]);
                ChangeFocusChar();
                if (_currentCharIndex != _currentWord.Length)
                {
                    AddBoxShadowForFocusKey(_currentWord[_currentCharIndex]);
                    //AddBoxShadowForShiftKey();
                }
            }

            //не совсем коректно работает 
            else if (IsValidLowerKeyDown(e))
            {
                if (_currentCharIndex != _currentWord.Length)
                {
                    RemoveIncorrectlyPressedEffect();
                    RemoveBoxShadowForLostFocusKey(_currentWord[_currentCharIndex]);

                }

                ChangeFocusChar();
                if (_currentCharIndex != _currentWord.Length)
                {

                    AddBoxShadowForFocusKey(_currentWord[_currentCharIndex]);
                }
            }

            else
                if (Keyboard.Modifiers != ModifierKeys.Shift && _isKeyDownHandlerActive)
            {
                _currentCharRun.Foreground = Brushes.Red;
                ++_missclickCounter;
                MissclickCountLabel.Content = _missclickCounter.ToString();
                RemoveIncorrectlyPressedEffect();
                _incorrectPressedChar = (char)(e.Key + 21);
                AddIncorrectlyPressedEffect();
            }
        }

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        UpdateGameData();
        var button = sender as Button;
        button.IsEnabled = false;
        _timer.Start();
        _stopwatch.Start();
        WordsTextBox.IsEnabled = true;


    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();

    }

    private void ChangeThemeButton_Click(object sender, RoutedEventArgs e)
    {
        MainCard.SetResourceReference(BackgroundProperty, "MainThemeBackground");
    }

    private async void Timer_Tick(object sender, EventArgs e)
    {
        await UpdateTimeLabelAsync();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        new ResultWindow(123.50,12, 20).ShowDialog();
    }
}