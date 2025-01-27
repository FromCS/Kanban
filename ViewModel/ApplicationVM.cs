using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Canvas.Views;
using Canvas.Views.ProgressView;
using Canvas.Views.ProjectsView;
using Canvas.Views.TemplatesView;

namespace Canvas.ViewModel;

public class ApplicationVM : INotifyPropertyChanged
{
    private readonly MainWindow? MainWindow = null!;
    private RelayCommand chooseWindow;
    private ProgressView progressView = null!;
    private CommonProjectsListView projectsView = null!;
    private TemplatesView templatesView = null!;
    private SettingsView settingsView = null!;
    private ProgressSelectedProjectView selectedProgressView = null!;

    private void OpenProgressView()
    {
        selectedProgressView = new ProgressSelectedProjectView();
        MainWindow!.CurrentPage.Content = progressView;
        MainWindow.SelectedProjectView.Content = selectedProgressView;
    }

    private void OpenProjectsView()
    {
        AnimationUtils.CloseLegendAdditionalWindow(MainWindow);
        progressView.LegendViewButton.Content = ">>>";
        MainWindow.CurrentPage.Content = projectsView;
    }

    private void OpenTemplatesView()
    {
        AnimationUtils.CloseLegendAdditionalWindow(MainWindow);
        progressView.LegendViewButton.Content = ">>>";
        MainWindow.CurrentPage.Content = templatesView;
    }

    private void OpenSettingsView()
    {
        AnimationUtils.CloseLegendAdditionalWindow(MainWindow);
        progressView.LegendViewButton.Content = ">>>";
        MainWindow.CurrentPage.Content = settingsView;
    }
    public RelayCommand ChooseWindow
    {
        get => chooseWindow ??= new RelayCommand(obj =>
        {
            switch (obj as string)
            {
                case "progress":
                    OpenProgressView();
                    break;
                case "projects":
                    OpenProjectsView();
                    break;
                case "templates":
                    OpenTemplatesView();
                    break;
                case "settings":
                    OpenSettingsView();
                    break;
                default:
                    break;
            }
        });
    }

    public ApplicationVM()
    {
        MainWindow = Application.Current.MainWindow as MainWindow;
        progressView = new ProgressView { DataContext = new ProgressVM(MainWindow) };
        projectsView = new CommonProjectsListView { DataContext = new ProjectsVM((progressView.DataContext as ProgressVM)!.Projects) };
        templatesView = new TemplatesView { DataContext = new TemplatesVM() };
        settingsView = new SettingsView { DataContext = new SettingsVM() };
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}