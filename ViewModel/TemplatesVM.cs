using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Canvas.Database;
using Canvas.Model;
using Canvas.Model.ProjectModel;
using Canvas.Windows;

namespace Canvas.ViewModel;

public class TemplatesVM : INotifyPropertyChanged
{
    private ObservableCollection<LegendTemplate> templates = null!;
    private RelayCommand openNewTemplateWindow;

    public ObservableCollection<LegendTemplate> Templates
    {
        get => templates;
        set
        {
            templates = value;
            OnPropertyChanged("templates");
        }
    }

    public RelayCommand OpenNewTemplateWindow
    {
        get
        {
            return openNewTemplateWindow ??= new RelayCommand(obj =>
            {
                var window = new NewStepsTemplateWindow
                {
                    DataContext = new NewStepTemplateVM()
                };
                window.ShowDialog();
            });
        }
    }

    public TemplatesVM()
    {
        var templatesFromDb = new ObservableCollection<LegendTemplate>();
        TemplatesDatabase.GetLegendTemplates(ref templatesFromDb);
        Templates = templatesFromDb;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}