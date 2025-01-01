using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Canvas.Model;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel;

public class TemplatesVM : INotifyPropertyChanged
{
    private ObservableCollection<TemplateStepsCategory> templates = null!;

    public ObservableCollection<TemplateStepsCategory> Templates
    {
        get => templates;
        set
        {
            templates = value;
            OnPropertyChanged("templates");
        }
    }

    public TemplatesVM()
    {
        Templates = new ObservableCollection<TemplateStepsCategory>
        {
            new TemplateStepsCategory() {CategoryName = "Постановка на производство малотоннажного продукта"},
            new () {CategoryName = "Разработка рецептуры с нуля"},
            new () {CategoryName = "Оформление коммерческого договора"}
        };
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}