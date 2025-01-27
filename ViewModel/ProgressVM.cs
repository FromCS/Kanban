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

public class ProgressVM : INotifyPropertyChanged
{
    private IProject selectedProject = null!;
    private MainWindow _mainWindow;
    private RelayCommand openAdditWindow;
    private bool _isAdditWindowOpen;

    public ObservableCollection<IProject> Projects { get; set; } = null!;

    public IProject SelectedProject
    {
        get => selectedProject;
        set
        {
            selectedProject = value;
            OnPropertyChanged("selectedProject");
            _mainWindow.SelectedProjectView.DataContext = selectedProject;
        }
    }

    public RelayCommand OpenAdditWindow
    {
        get => openAdditWindow ??= new RelayCommand(obj =>
        {
            _isAdditWindowOpen = _mainWindow.Grid.ColumnDefinitions.Last().Width.Value > 0;
            Button? button = obj as Button;
            if (!_isAdditWindowOpen)
            {
                button!.Content = "<<<";
                _mainWindow.DataContext = SelectedProject;
                _mainWindow.Width = 1350;
                _mainWindow.Grid.ColumnDefinitions.Last().Width = new GridLength(911);
                _mainWindow.Grid.ColumnDefinitions[0].Width = new GridLength(128);
                _mainWindow.Grid.ColumnDefinitions[1].Width = new GridLength(311);
                _isAdditWindowOpen = true;
                AnimationUtils.OffsetMainWindowToLeftWithAnimation(_mainWindow);
            }
            else
            {
                button!.Content = ">>>";
                _mainWindow.Width = 850;
                _isAdditWindowOpen = false;
                _mainWindow.Grid.ColumnDefinitions.Last().Width = new GridLength(0);
                _mainWindow.Grid.ColumnDefinitions[0].Width = new GridLength(228);
                _mainWindow.Grid.ColumnDefinitions[1].Width = new GridLength(622);
                AnimationUtils.OffsetMainWindowToRightWithAnimation(_mainWindow);
            }
        });
    }

    public ProgressVM(MainWindow mainWindow)
    {
        Projects = MainDatabase.GetAllProjects();
        _mainWindow = mainWindow;
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}