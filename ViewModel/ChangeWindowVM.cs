using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Canvas.ViewModel;

public class ChangeWindowVM : INotifyPropertyChanged
{
    public enum ViewType
    {
        Progress,
        Projects,
        Templates
    }

    public ChangeWindowVM()
    {
        
    }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


}