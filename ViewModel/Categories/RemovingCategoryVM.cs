using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel.Categories;

public class RemovingCategoryVM : INotifyPropertyChanged
{
    private ObservableCollection<string> categories;
    private string selectedCategory;
    private RelayCommand removeSelectedCategory;
    private ObservableCollection<IProject> _projects;

    public ObservableCollection<string> Categories
    {
        get => categories;
        set
        {
            categories = value;
            OnPropertyChanged("categories");
        }
    }
    public string SelectedCategory
    {
        get => selectedCategory;
        set
        {
            selectedCategory = value;
            OnPropertyChanged("selectedCategory");
        }
    }

    public RelayCommand RemoveSelectedCategory
    {
        get => removeSelectedCategory ??= new RelayCommand(obj =>
        {
            var window = obj as Window;
            window!.Close();
        });
    }

    public RemovingCategoryVM(ObservableCollection<string> categories, ObservableCollection<IProject> projects)
    {
        Categories = categories;
        _projects = projects; // for updating project's work category -  when category is removed project will have category "Без категории"
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}