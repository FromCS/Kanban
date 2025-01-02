using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Canvas.Database;
using Canvas.Model;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel;

public class NewProjectVM : INotifyPropertyChanged
{
    private string projectName = null!;
    private string workCategory = null!;
    private string priority = null!;
    private ObservableCollection<Step> _steps = new ObservableCollection<Step>();
    private RelayCommand addNewProjectToDb;
    private RelayCommand addStepToTree;
    private RelayCommand getFlatLegend;


    public RelayCommand GetFlatLegend
    {
        get
        {
            return getFlatLegend ??= new RelayCommand(obj =>
            {
                try
                {
                    var coll = GetFlatSteps(Steps);
                    coll.ToList().ForEach(step => MessageBox.Show(step.StepName));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            });
        }
    }
    private ObservableCollection<Step> GetFlatSteps(ObservableCollection<Step> steps)
    {
        ObservableCollection<Step> flatSteps = new ObservableCollection<Step>();
        if (steps.Count < 1) return flatSteps;
        foreach (var currentStep in steps)
        {
            flatSteps.Add(currentStep);
            var nestedSteps = GetFlatSteps(currentStep.Steps);
            nestedSteps.ToList().ForEach(nestedStep => flatSteps.Add(nestedStep));
        }    
        return flatSteps;
    }

    public RelayCommand AddStepToTree
    {
        get
        {
            return addStepToTree ??= new RelayCommand(obj =>
            {
                Steps.Add(new Step() {StepName = "NEW STEP", ParentSteps = Steps, ParentId = null, ID = 1});
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

    public ObservableCollection<Step> Steps
    {
        get => _steps;
        set
        {
            _steps = value;
            OnPropertyChanged("_projects");
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
                var rawLegend = Steps;
                Utils.SetupCorrectID(ref rawLegend);
                var legend = GetFlatSteps(rawLegend);
                MainDatabase.AddNewProject(currentProject);
                MainDatabase.AddProjectLegendTable(projectName, legend);
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}