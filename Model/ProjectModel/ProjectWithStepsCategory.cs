using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Canvas.Model.ProjectModel;

public class ProjectWithStepsCategory : IProject
{
    private string name;
    private ObservableCollection<Step> steps;
    private string workCategory;
    private LegendTemplate _templateStepsCategory;
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

    public LegendTemplate TemplateStepsCategory
    {
        get => _templateStepsCategory;
        set => _templateStepsCategory = value;
    }
    public ObservableCollection<Step> Steps
    {
        get => _templateStepsCategory.Legend;
        set
        {
            steps = TemplateStepsCategory.Legend;
            OnPropertyChanged("CategSteps");
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