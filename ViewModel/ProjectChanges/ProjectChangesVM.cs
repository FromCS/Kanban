using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Canvas.Database;
using Canvas.Model;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel.ProjectChanges;

public class ProjectChangesVM : INotifyPropertyChanged
{
    private string projectName = null!;
    private IProject _project;
    private string workCategory = null!;
    private string priority = null!;
    private ObservableCollection<Step> _legend = new ObservableCollection<Step>();
    private ObservableCollection<LegendTemplate> _templates = new ObservableCollection<LegendTemplate>();
    private LegendTemplate selectedTemplate;
    private RelayCommand makeChanges;
    private RelayCommand addStepToTree;
    private ObservableCollection<IProject> _projects;
    private ObservableCollection<string> prioritySource;
    private string windowName;

    private void SetTemplateLegendToForm(LegendTemplate template)
    {
        Legend.Clear();
        foreach (var step in template.Legend)
        {
            step.ParentSteps = Legend;
            Legend.Add(step);
        }
    }
    public string WindowName
    {
        get => windowName;
        set
        {
            windowName = value;
            OnPropertyChanged("windowName");
        }
    }

    public ObservableCollection<string> PrioritySource
    {
        get => new()
        {
            "Отложенный", "Низкий", "Средний", "Высокий"
        };
        set
        {
            prioritySource = value;
            OnPropertyChanged("prioritySource");
        }
    }
    public RelayCommand AddStepToTree
    {
        get => addStepToTree ??= new RelayCommand(obj =>
        {
            Legend.Add(new Step() {StepName = "NEW STEP", ParentSteps = Legend, ParentId = null, ID = 1, IsDone = false});
        });
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
        get => priority;
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
    public RelayCommand MakeChanges
    {
        get
        {
            return makeChanges ??= new RelayCommand(obj =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var newLegend = Utils.GetParsedLegend(Legend);
                    var newProject = new Project
                        { Name = ProjectName, Priority = Priority, ID = _project.ID, WorkCategory = WorkCategory };
                    _projects[_projects.IndexOf(_project)] = newProject; // updating projects' list on page
                    MainDatabase.MakeChangesForProject(newProject, _project, newLegend);
                    MessageBox.Show("Изменение выполнено успешно!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    (obj as Window).Close();
                }), DispatcherPriority.ContextIdle);
            });
        }
    }
    // init for changes
    public ProjectChangesVM(ObservableCollection<IProject> projectsList, IProject? project)
    {
        _projects = projectsList;
        _project = project;
        ProjectName = project!.Name;
        WorkCategory = project.WorkCategory;
        Legend = MainDatabase.GetProjectLegend(project.Name);
        Priority = PrioritySource[GetCurrentPriorityIndex(project)];
        WindowName = $"Изменение проекта: \"{project.Name}\"";
        
        var databaseTemplates = new ObservableCollection<LegendTemplate>
        {
            new () {LegendName = "--Без шаблона--", Legend = new ObservableCollection<Step>()}
        };
        TemplatesDatabase.GetLegendTemplates(ref databaseTemplates);
        Templates = databaseTemplates;
        SelectedTemplate = Templates[0];
    }

    private int GetCurrentPriorityIndex(IProject? project)
    {
        int index = 0;
        for (int i = 0; i < PrioritySource.Count; i++)
        {
            if (project!.Priority == PrioritySource[i])
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}