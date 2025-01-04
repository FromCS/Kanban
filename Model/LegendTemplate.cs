using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Canvas.Model;

public class TemplateStepsCategory : INotifyPropertyChanged
{
    private string legendName;
    private ObservableCollection<Step> legend;

    public string LegendName
    {
        get => legendName;
        set
        {
            legendName = value;
            OnPropertyChanged("legendName");
        }
    }

    public ObservableCollection<Step> Legend
    {
        get => legend;
        set
        {
            legend = value;
            OnPropertyChanged("legend");
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}