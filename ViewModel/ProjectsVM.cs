using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Canvas.Database;
using Canvas.Model.ProjectModel;
using Canvas.Windows;

namespace Canvas.ViewModel;

public class ProjectsVM : INotifyPropertyChanged
{
    private ObservableCollection<IProject> projects = null!;
    private RelayCommand addNewProject;
    private ObservableCollection<IProject> _progress;

    public ObservableCollection<IProject> Projects
    {
        get => projects;
        set
        {
            projects = value;
            OnPropertyChanged("projects");
        }
    }

    public RelayCommand AddNewProject
    {
        get
        {
            return addNewProject ??= new RelayCommand(obj =>
            {
                var window = new NewProjectWindow
                {
                    DataContext = new NewProjectVM(Projects)
                };
                window.Show();
            });
        }
    }
    
    public ProjectsVM()
    {
        Projects = MainDatabase.GetAllProjects();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}