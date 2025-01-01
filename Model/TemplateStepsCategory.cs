using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Canvas.Model;

public class TemplateStepsCategory : INotifyPropertyChanged
{
    private string categoryName;
    private ObservableCollection<Step> categSteps;

    public string CategoryName
    {
        get => categoryName;
        set
        {
            categoryName = value;
            OnPropertyChanged("categoryName");
        }
    }

    public ObservableCollection<Step> CategSteps
    {
        get => categSteps;
        set
        {
            categSteps = value;
            OnPropertyChanged("categSteps");
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}