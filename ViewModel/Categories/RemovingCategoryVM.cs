using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Canvas.Database;
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
        get => new (categories.Skip(1));
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
            if (SelectedCategory == null) return;
            var answer = MessageBox.Show($"Вы уверены, что хотите удалить категорию \"{SelectedCategory}\"?", 
                "Подтверждение удаления", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.No) return;
            var window = obj as Window;
            _projects.ToList().ForEach(project =>
            {
                if (project.WorkCategory == SelectedCategory)
                {
                    project.WorkCategory = "Без категории";
                }
            });
            MainDatabase.RemoveCategory(SelectedCategory);
            categories.Remove(SelectedCategory);
            window!.Close();
        });
    }

    public RemovingCategoryVM(ObservableCollection<string> categoriesList, ObservableCollection<IProject> projects)
    {
        Categories = categoriesList;
        _projects = projects; // for updating project's work category -  when category is removed project will have category "Без категории"
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}