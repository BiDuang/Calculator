using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Calculator.Models;
using I18N.Avalonia.Interface;
using Newtonsoft.Json;
using Splat;

namespace Calculator.Core;

public class Config
{
    [JsonIgnore] public static readonly string ProgramDir = AppDomain.CurrentDomain.BaseDirectory;
    [JsonIgnore] public static readonly string ConfigDir = Path.Combine(ProgramDir, "Config.json");

    [JsonIgnore] public static Config? Instance;
    [JsonProperty] public ThemeMode AppTheme { get; set; } = ThemeMode.SyncWithSystem;

    [JsonProperty]
    public string AppLanguage
    {
        get => Locator.Current.GetService<ILocalizer>()!.Language.Name;
        set => Locator.Current.GetService<ILocalizer>()!.Language = new CultureInfo(value);
    }

    public Config()
    {
        if (File.Exists(ConfigDir)) JsonConvert.PopulateObject(File.ReadAllText(ConfigDir), this);
        else AppLanguage = Thread.CurrentThread.CurrentCulture.Name;
    }

    public void Save() => File.WriteAllText(ConfigDir, JsonConvert.SerializeObject(this));
}