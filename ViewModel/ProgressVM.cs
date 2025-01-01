using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Canvas.Model;
using Canvas.Model.ProjectModel;

namespace Canvas.ViewModel;

public class ProgressVM : INotifyPropertyChanged
{
    private IProject selectedProject = null!;

    public ObservableCollection<IProject> Projects { get; set; } = null!;

    public IProject SelectedProject
    {
        get => selectedProject;
        set
        {
            selectedProject = value;
            OnPropertyChanged("selectedProject");
        }
    }

    public ProgressVM()
    {
        try
        {
            Projects = new ObservableCollection<IProject>
        {
            new Project {Name = "Разработка нового продукта", Priority = "Высокий", Steps = new ObservableCollection<Step>
            {
                new (){StepName = "Закупка сырья", 
                    MiniSteps = new ObservableCollection<NestedStep>
                    {
                        new (){Caption = "Полимер"},
                        new (){Caption = "Наполнитель"},
                        new (){Caption = "Добавка 1"},
                        new (){Caption = "Добавка 2"},
                        new (){Caption = "Добавка 3"},
                    }}, 
                new (){StepName = "Разработка рецептуры", MiniSteps = new ObservableCollection<NestedStep>
                {
                    new (){Caption = "Отработка 1"},
                    new (){Caption = "Отработка 2"},
                    new (){Caption = "Отработка 3"}
                }}, 
                new (){StepName = "Производство материала"}, 
                new (){StepName = "Отчетные документы"}
            }},
            
            
            new ProjectWithStepsCategory {Name = "Отгрузка какого-то продукта", Priority = "Очень высокий", TemplateStepsCategory = new TemplateStepsCategory
            {
                CategoryName = "Проверка категории",
                CategSteps = new ObservableCollection<Step>
                {
                    new () {StepName = "Закупка материалов"},
                    new () {StepName = "Наработка продуктов"},
                    new () {StepName = "Выходной контроль"},
                    new () {StepName = "Оформление паспорта"},
                    new () {StepName = "Отгрузка"}
                }
            }},
            
            
            new Project {Name = "Договор", Priority = "Низкий", Steps = new ObservableCollection<Step>
            {
                new () {StepName = "Оформление договора"},
                new () {StepName = "Составление ТЗ"},
                new () {StepName = "Разработка материала"},
                new () {StepName = "Оформление ТУ"},
                new () {StepName = "Оформление тех. процесса"},
                new () {StepName = "Завершение работы"}
            }},

        };
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}