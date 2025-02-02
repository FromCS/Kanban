using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Canvas.Database;
using Canvas.Model;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel;

public class NewLegendTemplateVM : INotifyPropertyChanged
{
    private ObservableCollection<Step> legend = new ObservableCollection<Step>();
    private string templateName;
    private RelayCommand addStepToTree;
    private RelayCommand addTemplateToDb;
    private ObservableCollection<LegendTemplate> _templates;

    public ObservableCollection<Step> Legend
    {
        get => legend;
        set
        {
            legend = value;
            OnPropertyChanged("legend");
        }
    }

    public string TemplateName
    {
        get => templateName;
        set
        {
            templateName = value;
            OnPropertyChanged("templateName");
        }
    }
    
    public RelayCommand AddStepToTree
    {
        get
        {
            return addStepToTree ??= new RelayCommand(obj =>
            {
                Legend.Add(new Step() {StepName = "NEW STEP", ParentSteps = Legend, ParentId = null, ID = 1, IsDone = false});
            });
        }
    }

    public RelayCommand AddTemplateToDb
    {
        get => addTemplateToDb ??= new RelayCommand(obj =>
        {
            var rawLegend = Legend;
            _templates.Add(new LegendTemplate {LegendName = TemplateName, Legend = rawLegend});
            var cleanLegend = Utils.GetParsedLegend(Legend);
            TemplatesDatabase.AddLegendTemplateTable(TemplateName, cleanLegend);
        });
    }
    public NewLegendTemplateVM(ObservableCollection<LegendTemplate> templates)
    {
        _templates = templates;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}