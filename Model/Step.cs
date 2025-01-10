using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Canvas.Database;
using Canvas.Model.ProjectModel;

namespace Canvas.Model;

public class Step : INotifyPropertyChanged
{
    private string stepName;
    private ObservableCollection<Step> miniSteps = new ObservableCollection<Step>();
    private ObservableCollection<Step> parentSteps;
    private int? parentID;
    private int id;
    private bool isDone;
    private RelayCommand setIsDone;
    private RelayCommand addNestedStep;
    private RelayCommand removeSelf;
    private RelayCommand changeStepName;

    public RelayCommand ChangeStepName
    {
        get
        {
            return changeStepName ??= new RelayCommand(obj =>
            {
                var textbox = obj as TextBox;
                textbox.IsEnabled = false;
            });
        }
    }
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
    public int? ParentId
    {
        get => parentID;
        set
        {
            parentID = value;
            OnPropertyChanged("parentID");
        }
    }
    public int ID
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged("id");
        }
    }
    public bool IsDone
    {
        get => isDone;
        set
        {
            isDone = value;
            OnPropertyChanged("isDone");
        }
    }
    public RelayCommand SetIsDone
    {
        get => setIsDone ??= new RelayCommand(obj =>
        {
            IProject? project = (IProject)(obj as Grid)!.DataContext;
            MainDatabase.InsertIsDoneForLegendStepByID(project.Name, this.ID, IsDone);
        });
    }
    public RelayCommand AddNestedStep
    {
        get
        {
            return addNestedStep ??= new RelayCommand(obj =>
            {
                try
                {
                    Steps.Add(new Step(){stepName = "NEW ELEMENT", ParentSteps = this.Steps, ParentId = ID, id = ID + 1, isDone = false});
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