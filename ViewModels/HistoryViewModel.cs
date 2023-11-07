using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;

namespace Calculator.ViewModels;

public class HistoryViewModel : SubPageViewModel
{
    public static HistoryViewModel? Instance;
    
    public HistoryViewModel()
    {
        Instance = this;
    }
    
    public List<string> HistoryList => Core.DataHolder.HistoryList;
    
    public ICommand ClearHistoryCommand => ReactiveCommand.Create(() =>
    {
        HistoryList.Clear();
        this.RaisePropertyChanged(nameof(Core.DataHolder.HistoryList));
    });
    
}