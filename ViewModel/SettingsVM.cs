using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Canvas.ViewModel;

public class SettingsVM : INotifyPropertyChanged
{
    private string databasePath = null!;

    public string DatabasePath
    {
        get => databasePath;
        set
        {
            databasePath = value;
            OnPropertyChanged("databasePath");
        }
    }

    public SettingsVM()
    {
        DatabasePath = "КАКАЯ-ТО ССЫЛКА НА БАЗУ ДАННЫХ";
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


}