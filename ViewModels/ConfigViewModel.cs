using System.Collections.Generic;
using Avalonia;
using Calculator.Core;
using Calculator.Models;
using Calculator.Utils;

namespace Calculator.ViewModels;

public class ConfigViewModel : SubPageViewModel
{
    public static List<ThemeMode> Themes => new()
    {
        ThemeMode.Light,
        ThemeMode.Dark,
        ThemeMode.SyncWithSystem
    };

    public static List<SupportedLanguage> Languages => new()
    {
        SupportedLanguage.English,
        SupportedLanguage.Chinese,
        SupportedLanguage.Japanese
    };

    public ThemeMode CurrentTheme
    {
        get => Config.Instance!.AppTheme;
        set
        {
            if (value == ThemeMode.SyncWithSystem)
                Application.Current!.PlatformSettings!.ColorValuesChanged +=
                    MainWindowViewModel.Instance!.SystemThemeChangedEvent;
            else
                Application.Current!.PlatformSettings!.ColorValuesChanged -=
                    MainWindowViewModel.Instance!.SystemThemeChangedEvent;

            Config.Instance!.AppTheme = value;
            MainWindowViewModel.Instance.CurrentTheme = ThemeConverter.ThemeMode2AppThemeVar(value);
            Config.Instance.Save();
        }
    }
    
    public SupportedLanguage CurrentLanguage
    {
        get => LanguageConverter.LanguageCode2SupportedLanguage(Config.Instance!.AppLanguage);
        set
        {
            Config.Instance!.AppLanguage = LanguageConverter.SupportedLanguage2LanguageCode(value);
            Config.Instance.Save();
        }
    }
}