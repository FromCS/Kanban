using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Canvas.Database;

namespace Canvas.Model.ProjectModel;

public class Project : IProject
{
    private string name;
    private string workCategory;
    private ObservableCollection<Step> steps;
    private string priority;

    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged("name");
        }
    }

    public string WorkCategory
    {
        get => workCategory;
        set
        {
            workCategory = value;
            OnPropertyChanged("workCategory");
        }
    }
    
    public ObservableCollection<Step> Steps
    {
        get
        {
            steps = MainDatabase.GetProjectLegend(name);
            return steps;
        } 
        set
        {
            steps = value;
            OnPropertyChanged("steps");
        }
    }


    public string Priority
    {
        get => priority;
        set
        {
            priority = value;
            OnPropertyChanged("priority");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}