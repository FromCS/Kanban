﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Canvas.Database;
using Canvas.Model;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel;

public class NewProjectVM : INotifyPropertyChanged
{
    private string projectName = null!;
    private string workCategory = null!;
    private string priority = null!;
    private ObservableCollection<Step> _legend = new ObservableCollection<Step>();
    private RelayCommand addNewProjectToDb;
    private RelayCommand addStepToTree;
    private RelayCommand getFlatLegend;
    private RelayCommand templateSelectionChanged;
    private ObservableCollection<IProject> _projects;


    public RelayCommand GetFlatLegend
    {
        get
        {
            return getFlatLegend ??= new RelayCommand(obj =>
            {
                try
                {
                    var coll = Utils.GetFlatSteps(Legend);
                    coll.ToList().ForEach(step => MessageBox.Show(step.StepName));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            });
        }
    }
    

    public RelayCommand AddStepToTree
    {
        get
        {
            return addStepToTree ??= new RelayCommand(obj =>
            {
                Legend.Add(new Step() {StepName = "NEW STEP", ParentSteps = Legend, ParentId = null, ID = 1});
            });
        }
    }
    public string ProjectName
    {
        get => projectName;
        set
        {
            projectName = value;
            OnPropertyChanged("projectName");
        }
    }

    public string WorkCategory
    {
        get => workCategory;
        set
        {
            workCategory = value;
            OnPropertyChanged("workCategory");
        }
    }

    public string Priority
    {
        get => priority.Split(":")[1].Trim();
        set
        {
            priority = value;
            OnPropertyChanged("priority");
        }
    }

    public ObservableCollection<Step> Legend
    {
        get => _legend;
        set
        {
            _legend = value;
            OnPropertyChanged("_legend");
        }
    }
    public RelayCommand AddNewProjectToDb
    {
        get
        {
            return addNewProjectToDb ??= new RelayCommand(obj =>
            {
                var currentProject = new Project()
                    { Name = ProjectName, WorkCategory = WorkCategory, Priority = Priority };
                var rawLegend = Legend;
                Utils.SetupCorrectID(ref rawLegend);
                var legend = Utils.GetFlatSteps(rawLegend);
                MainDatabase.AddNewProject(currentProject);
                MainDatabase.AddProjectLegendTable(projectName, legend);
            });
        }
    }

    public NewProjectVM(ObservableCollection<IProject> projectsList)
    {
        _projects = projectsList;
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}