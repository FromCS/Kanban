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
    private RelayCommand getFlatLegend;
    private ObservableCollection<IProject> _projects;


    public RelayCommand GetFlatLegend
    {
        get
        {
            return getFlatLegend ??= new RelayCommand(obj =>
            {
                try
                {
                    var coll = Utils.GetFlatSteps(Legend);
                    coll.ToList().ForEach(step => MessageBox.Show(step.StepName));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            });
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
        get
        {
            var databaseTemplates = new ObservableCollection<LegendTemplate>() { new LegendTemplate() {LegendName = "--Без шаблона--", Legend = new ObservableCollection<Step>()} };
            TemplatesDatabase.GetLegendTemplates(ref databaseTemplates);
            return databaseTemplates;
        }
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
            LegendTemplate origValue = selectedTemplate;
            selectedTemplate = value;
            if (MessageBox.Show(
                    "При изменении шаблона текущая легенда будет аннулирована. Вы уверены, что хотите использовать другой шаблон?", "Подтверждение", MessageBoxButton.YesNo) ==
                MessageBoxResult.No)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    selectedTemplate = origValue;
                    OnPropertyChanged("selectedTemplate");
                }), DispatcherPriority.ContextIdle, null);
                return;
            }
            Legend.Clear();
            foreach (var step in value.Legend)
            {
                step.ParentSteps = Legend;
                Legend.Add(step);
            }
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
    }
    
    // init for changes
    public NewProjectVM(ObservableCollection<IProject> projectsList, IProject project)
    {
        _projects = projectsList;
        ProjectName = project.Name;
        WorkCategory = project.WorkCategory;
        Legend = MainDatabase.GetProjectLegend(project.Name);
        Priority = project.Priority;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}