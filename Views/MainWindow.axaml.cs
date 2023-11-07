using Avalonia.Controls;

namespace Calculator.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Core.Config.Instance = new Core.Config();
        InitializeComponent();
        Instance = this;
    }

    public static MainWindow? Instance { get; private set; }
}