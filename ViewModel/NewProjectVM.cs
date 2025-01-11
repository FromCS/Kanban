using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Canvas.Database;
using Canvas.Model;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel;

public class NewProjectVM : INotifyPropertyChanged
{
    private string projectName = null!;
    private string workCategory = null!;
    private string priority = null!;
    private ObservableCollection<Step> _legend = new ObservableCollection<Step>();
    private ObservableCollection<LegendTemplate> _templates = new ObservableCollection<LegendTemplate>();
    private LegendTemplate selectedTemplate;
    private RelayCommand addNewProjectToDb;
    private RelayCommand addStepToTree;
    private ObservableCollection<IProject> _projects;
    
    private void SetTemplateLegendToForm(LegendTemplate template)
    {
        Legend.Clear();
        foreach (var step in template.Legend)
        {
            step.ParentSteps = Legend;
            Legend.Add(step);
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
    public string ProjectName
    {
        get => projectName;
        set
        {
            projectName = value;
            OnPropertyChanged("projectName");
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
        get => priority.Split(":")[1].Trim();
        set
        {
            priority = value;
            OnPropertyChanged("priority");
        }
    }

    public ObservableCollection<Step> Legend
    {
        get => _legend;
        set
        {
            _legend = value;
            OnPropertyChanged("_legend");
        }
    }

    public ObservableCollection<LegendTemplate> Templates
    {
        get => _templates;
        set
        {
            _templates = value;
            OnPropertyChanged("_templates");
        }
    }

    public LegendTemplate SelectedTemplate
    {
        get => selectedTemplate;
        set
        {
            if (selectedTemplate == null) // init window
            {
                selectedTemplate = value;
                OnPropertyChanged("selectedTemplate");
                return;
            }
            LegendTemplate origValue = selectedTemplate;
            selectedTemplate = value;
            var answer = MessageBox.Show(
                "При изменении шаблона текущая легенда будет аннулирована. Вы уверены, что хотите использовать другой шаблон?",
                "Подтверждение", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.No)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    selectedTemplate = origValue;
                    OnPropertyChanged("selectedTemplate");
                }), DispatcherPriority.ContextIdle, null);
                return;
            }
            SetTemplateLegendToForm(value);
            OnPropertyChanged("selectedTemplate");
        }
    }
    public RelayCommand AddNewProjectToDb
    {
        get
        {
            return addNewProjectToDb ??= new RelayCommand(obj =>
            {
                var currentProject = new Project()
                    { Name = ProjectName, WorkCategory = WorkCategory, Priority = Priority };
                var rawLegend = Legend;
                Utils.SetupCorrectID(ref rawLegend);
                var legend = Utils.GetFlatSteps(rawLegend);
                MainDatabase.AddNewProject(currentProject);
                _projects.Add(currentProject);
                MainDatabase.AddProjectLegendTable(projectName, legend);
            });
        }
    }

    public NewProjectVM(ObservableCollection<IProject> projectsList)
    {
        _projects = projectsList;
        var databaseTemplates = new ObservableCollection<LegendTemplate>
        {
            new LegendTemplate() {LegendName = "--Без шаблона--", Legend = new ObservableCollection<Step>()}
        };
        TemplatesDatabase.GetLegendTemplates(ref databaseTemplates);
        Templates = databaseTemplates;
        SelectedTemplate = Templates[0];
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}