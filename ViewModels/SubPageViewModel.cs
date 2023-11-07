using System.Windows.Input;
using Calculator.Animation;
using Calculator.Views;
using ReactiveUI;

namespace Calculator.ViewModels;

public class SubPageViewModel : ViewModelBase
{
    public ICommand GotoMainViewCommand => ReactiveCommand.Create(() =>
    {
        MainWindowViewModel.Instance!.Navigate<MainView, MainViewViewModel>(new MainViewViewModel(),
            transition: new DrillTransition { Backward = true });
    });
}