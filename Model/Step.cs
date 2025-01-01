using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Canvas.Model;

public class Step : INotifyPropertyChanged
{
    private string stepName;
    private ObservableCollection<Step> miniSteps = new ObservableCollection<Step>();
    private RelayCommand addNestedStep;
    private RelayCommand removeSelf;
    private ObservableCollection<Step> parentSteps;

    public string StepName
    {
        get => stepName;
        set
        {
            stepName = value;
            OnPropertyChanged("stepName");
        }
    }

    public ObservableCollection<Step> Steps
    {
        get => miniSteps;
        set
        {
            miniSteps = value;
            OnPropertyChanged("miniSteps");
        }
    }
    public ObservableCollection<Step> ParentSteps
    {
        get => parentSteps;
        set
        {
            parentSteps = value;
            OnPropertyChanged("parentSteps");
        }
    }

    public RelayCommand AddNestedStep
    {
        get
        {
            return addNestedStep ??= new RelayCommand(obj =>
            {
                try
                {
                    Steps.Add(new Step(){stepName = "NEW ELEMENT", ParentSteps = this.Steps});
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            });
        }
    }
    
    public RelayCommand RemoveSelf
    {
        get
        {
            return removeSelf ??= new RelayCommand(obj =>
            {
                parentSteps.Remove(this);
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}