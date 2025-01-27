using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Canvas.Database;
using Canvas.ViewModel;
using Canvas.ViewModel.ProjectChanges;
using Canvas.Windows;
using Canvas.Windows.ChangesViews;

namespace Canvas.Model.ProjectModel;

public class Project : IProject
{
    private string name;
    private string workCategory;
    private ObservableCollection<Step> legend;
    private string priority;
    private string progressValue;
    private RelayCommand openViewForChanges;

    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged("name");
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
    
    public ObservableCollection<Step> Legend
    {
        get
        {
            legend = MainDatabase.GetProjectLegend(name);
            return legend;
        } 
        set
        {
            legend = value;
            OnPropertyChanged("legend");
        }
    }

    public string ProgressValue
    {
        get
        {
            double value = Math.Round(MainDatabase.GetProgressValue(name) * 100);
            progressValue = $"{value}%"; 
            return progressValue;
        }
        set
        {
            progressValue = value;
            OnPropertyChanged("progressValue");
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
    public RelayCommand OpenViewForChanges
    {
        get => openViewForChanges ??= new RelayCommand(obj =>
        {
            var cont = obj as ListBox;
            var projects = cont!.ItemsSource as ObservableCollection<IProject>;
            var view = new ChangedProjectView()
            {
                DataContext = new ProjectChangesVM(projects, this)
            };
            view.ShowDialog();
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}