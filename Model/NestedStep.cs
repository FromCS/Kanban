using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Canvas.Model;

public class NestedStep : INotifyPropertyChanged
{
    private string caption;
    private bool isDone;
    public string Caption
    {
        get => caption;
        set
        {
            caption = value;
            OnPropertyChanged("caption");
        }
    }

    public bool IsDone
    {
        get => isDone;
        set
        {
            isDone = value;
            OnPropertyChanged("isDone");
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}