using System.Collections.Generic;
using System.ComponentModel;
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

    public RelayCommand ChooseWindow
    {
        get
        {
            return chooseWindow ??
                   (chooseWindow = new RelayCommand(obj =>
                   {
                       switch (obj as string)
                       {
                           case "progress":
                               MainWindow.CurrentPage.Content = new ProgressView();
                               MainWindow.CurrentPage.DataContext = new ProgressVM();
                               break;
                           case "projects":
                               MainWindow.CurrentPage.Content = new CommonProjectsListView();
                               MainWindow.CurrentPage.DataContext = new ProjectsVM();
                               break;
                           case "templates":
                               MainWindow.CurrentPage.Content = new TemplatesView();
                               MainWindow.CurrentPage.DataContext = new TemplatesVM();
                               break;
                           case "settings":
                               MainWindow.CurrentPage.Content = new SettingsView();
                               MainWindow.CurrentPage.DataContext = new SettingsVM();
                               break;
                           default:
                               break;
                       }
                   }));
        }
    }

    public ApplicationVM()
    {
        MainWindow = Application.Current.MainWindow as MainWindow;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}