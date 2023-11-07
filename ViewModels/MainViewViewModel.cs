using System;
using System.Globalization;
using System.Windows.Input;
using Calculator.Animation;
using Calculator.Core;
using Calculator.Models;
using Calculator.Utils;
using Calculator.Views;
using ReactiveUI;
using Config = Calculator.Views.Config;

namespace Calculator.ViewModels;

public class MainViewViewModel : ViewModelBase
{
    private Operator? _operator;
    public static MainViewViewModel? Instance { get; private set; }

    public MainViewViewModel() => Instance = this;

    public string TopText
    {
        get => DataHolder.TopText;
        set => this.RaiseAndSetIfChanged(ref DataHolder.TopText, value);
    }

    public string BottomText
    {
        get => DataHolder.BottomText;
        set => this.RaiseAndSetIfChanged(ref DataHolder.BottomText, value);
    }

    public ICommand GotoConfigCommand => ReactiveCommand.Create(() =>
    {
        MainWindowViewModel.Instance!.Navigate<Config, ConfigViewModel>(new DrillTransition());
    });

    public ICommand GotoHistoryCommand => ReactiveCommand.Create(() =>
    {
        MainWindowViewModel.Instance!.Navigate<History, HistoryViewModel>(new DrillTransition());
    });

    public ICommand ChangeWindowKeepFrontCommand => ReactiveCommand.Create(() =>
    {
        if (MainWindow.Instance!.Topmost)
        {
            MainWindow.Instance.Topmost = false;
        }
        else
        {
            MainWindow.Instance.Topmost = true;
        }
    });

    public ICommand BackspaceCommand => ReactiveCommand.Create(() =>
    {
        if (BottomText == string.Empty) return;
        BottomText = BottomText[..^1];
    });

    public ICommand ClearCommand => ReactiveCommand.Create(() =>
    {
        TopText = string.Empty;
        BottomText = string.Empty;
        _operator = null;
    });

    public ICommand CopyResultCommand => ReactiveCommand.Create(async () =>
    {
        if (BottomText == string.Empty) return;
        await MainWindow.Instance!.Clipboard!.SetTextAsync(BottomText);
    });

    public ICommand CeCommand => ReactiveCommand.Create(() => BottomText = string.Empty);

    public ICommand ReciprocalCommand => ReactiveCommand.Create(() =>
    {
        if (BottomText == string.Empty) return;
        TopText = $"1 / {BottomText} =";
        BottomText = (1 / double.Parse(BottomText)).ToString(CultureInfo.InvariantCulture);
        DataHolder.HistoryList.Add($"{TopText} {BottomText}");
    });

    public ICommand SquareCommand => ReactiveCommand.Create(() =>
    {
        if (BottomText == string.Empty) return;
        TopText = $"{BottomText}² =";
        BottomText = (double.Parse(BottomText) * double.Parse(BottomText)).ToString(CultureInfo.InvariantCulture);
        DataHolder.HistoryList.Add($"{TopText} {BottomText}");
    });

    public ICommand SquareRootCommand => ReactiveCommand.Create(() =>
    {
        if (BottomText == string.Empty) return;
        if (double.Parse(BottomText) < 0)
        {
            TopText = "负数无法求平方根";
            BottomText = string.Empty;
            return;
        }
        TopText = $"√{BottomText} =";
        BottomText = Math.Sqrt(double.Parse(BottomText)).ToString(CultureInfo.InvariantCulture);
        DataHolder.HistoryList.Add($"{TopText} {BottomText}");
    });

    public ICommand OperatorCommand => ReactiveCommand.Create((int mode) =>
    {
        if (BottomText.EndsWith('.')) BottomText += "0";
        TopText = (BottomText == string.Empty ? "0" : BottomText) + " " +
                  Calculate.ConvertOperatorToString((Operator)mode);
        _operator = (Operator)mode;
        BottomText = string.Empty;
    });

    public ICommand EqualCommand => ReactiveCommand.Create(() =>
    {
        if (_operator == null)
        {
            TopText = BottomText == string.Empty ? "0 =" : $"{BottomText} =";
            return;
        }

        if (BottomText == string.Empty) return;

        var top = double.Parse(TopText[..^2]);
        var bottom = double.Parse(BottomText);
        TopText = $"{top} {Calculate.ConvertOperatorToString(_operator.Value)} {bottom} =";

        if (_operator == Operator.Divide && bottom == 0)
        {
            TopText = "除数不能为0";
            BottomText = string.Empty;
            return;
        }

        BottomText = _operator switch
        {
            Operator.Add => (top + bottom).ToString(CultureInfo.InvariantCulture),
            Operator.Minus => (top - bottom).ToString(CultureInfo.InvariantCulture),
            Operator.Multiply => (top * bottom).ToString(CultureInfo.InvariantCulture),
            Operator.Divide => (top / bottom).ToString(CultureInfo.InvariantCulture),
            _ => BottomText
        };

        DataHolder.HistoryList.Add(
            $"{top} {Calculate.ConvertOperatorToString(_operator.Value)} {bottom} = {BottomText}");
        _operator = null;
    });

    public ICommand PositiveNegativeCommand => ReactiveCommand.Create(() =>
    {
        if (BottomText.StartsWith("0.") && (BottomText.EndsWith("0") || BottomText.EndsWith("."))) return;
        if (BottomText is "" or "0") BottomText = "0";
        else BottomText = BottomText.StartsWith('-') ? BottomText[1..] : $"-{BottomText}";
    });

    public ICommand NumberInputCommand => ReactiveCommand.Create((string number) =>
    {
        if (BottomText.Length >= 15) return;
        if (number == ".")
        {
            if (BottomText.Contains('.')) return;
            if (BottomText == string.Empty) BottomText = "0";
        }
        else if (BottomText == "0")
        {
            if (number != "0") BottomText = number;
            return;
        }

        BottomText += number;
    });
}