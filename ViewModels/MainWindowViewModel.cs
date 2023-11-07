using System.Collections.Generic;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Styling;
using Calculator.Core;
using Calculator.Models;
using Calculator.Utils;
using ReactiveUI;

namespace Calculator.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private static readonly Dictionary<ThemeVariant, Color> ThemeColors = new()
    {
        { ThemeVariant.Light, Colors.SeaShell },
        { ThemeVariant.Dark, Color.FromArgb(51, 46, 46, 20) }
    };

    private UserControl _currentContent = new Views.MainView();
    private ThemeVariant _currentTheme = ThemeConverter.ThemeMode2AppThemeVar(Config.Instance!.AppTheme);
    private Color _themeColor = ThemeColors[ThemeConverter.ThemeMode2AppThemeVar(Config.Instance.AppTheme)];
    
    private IPageTransition _transition = new CrossFade();
    public static MainWindowViewModel? Instance { get; set; }
    
    public MainWindowViewModel()
    {
        if (Config.Instance!.AppTheme == ThemeMode.SyncWithSystem)
            Application.Current!.PlatformSettings!.ColorValuesChanged += SystemThemeChangedEvent;
        Instance = this;
    }
    
    public Color BaseColor
    {
        get => _themeColor;
        set => this.RaiseAndSetIfChanged(ref _themeColor, value);
    }
    
    public ThemeVariant CurrentTheme
    {
        get => _currentTheme;
        set
        {
            BaseColor = ThemeColors[value];
            this.RaiseAndSetIfChanged(ref _currentTheme, value);
        }
    }
    
    public UserControl CurrentContent
    {
        get => _currentContent;
        set => this.RaiseAndSetIfChanged(ref _currentContent, value);
    }
    
    public IPageTransition Transition
    {
        get => _transition;
        set => this.RaiseAndSetIfChanged(ref _transition, value);
    }
    
    public void Navigate<TV, TM>(IPageTransition? transition = null)
        where TV : UserControl, new()
        where TM : new()
    {
        Navigate<TV, TM>(new TM(), transition);
    }

    public void Navigate<TV, TM>(TM viewModel, IPageTransition? transition = null)
        where TV : UserControl, new()
    {
        Transition = transition ?? new CrossFade();
        CurrentContent = new TV
        {
            DataContext = viewModel
        };
    }
    
    public void SystemThemeChangedEvent(object? o, PlatformColorValues values)
    {
        CurrentTheme = ThemeConverter.PlatformThemeVar2AppThemeVar(values.ThemeVariant);
        BaseColor = ThemeColors[ThemeConverter.PlatformThemeVar2AppThemeVar(values.ThemeVariant)];
    }
}
