using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Canvas.Database;

namespace Canvas.ViewModel.Categories;

public class NewCategoryVM : INotifyPropertyChanged
{
    private string categoryName;
    private RelayCommand addNewCategoryToDb;
    private ObservableCollection<string> _categories;

    public string CategoryName
    {
        get => categoryName;
        set
        {
            categoryName = value;
            OnPropertyChanged("categoryName");
        }
    }

    public RelayCommand AddNewCategoryToDb
    {
        get => addNewCategoryToDb ??= new RelayCommand(obj =>
        {
            var window = obj as Window;
            MainDatabase.AddNewCategory(CategoryName);
            _categories.Add(CategoryName);
            window!.Close();
        });
    }

    public NewCategoryVM(ObservableCollection<string> categories)
    {
        _categories = categories;
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}