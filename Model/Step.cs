using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Canvas.Model;

public class Step : INotifyPropertyChanged
{
    private string stepName;
    private ObservableCollection<NestedStep> miniSteps;

    public string StepName
    {
        get => stepName;
        set
        {
            stepName = value;
            OnPropertyChanged("stepName");
        }
    }

    public ObservableCollection<NestedStep> MiniSteps
    {
        get => miniSteps;
        set
        {
            miniSteps = value;
            OnPropertyChanged("miniSteps");
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}