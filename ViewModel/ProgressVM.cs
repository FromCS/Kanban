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

public class ProgressVM : INotifyPropertyChanged
{
    private IProject selectedProject = null!;

    public ObservableCollection<IProject> Projects { get; set; } = null!;

    public IProject SelectedProject
    {
        get => selectedProject;
        set
        {
            selectedProject = value;
            OnPropertyChanged("selectedProject");
        }
    }

    public ProgressVM()
    {
        try
        {
            Projects = MainDatabase.GetAllProjects();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}