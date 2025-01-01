using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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

    public RelayCommand AddStepToTree
    {
        get
        {
            return addStepToTree ??= new RelayCommand(obj =>
            {
                Steps.Add(new Step() {StepName = "NEW STEP", ParentSteps = Steps});
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
                try
                {
                    var currentProject = new Project()
                        { Name = ProjectName, WorkCategory = WorkCategory, Priority = Priority };
                    MainDatabase.AddNewProject(currentProject);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());

                }
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}