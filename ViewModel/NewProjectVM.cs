using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Canvas.Database;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel;

public class NewProjectVM : INotifyPropertyChanged
{
    private string projectName = null!;
    private string workCategory = null!;
    private string priority = null!;
    private RelayCommand command;

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

    public RelayCommand Command
    {
        get
        {
            return command ??= new RelayCommand(obj =>
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