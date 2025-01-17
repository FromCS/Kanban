﻿using System;
using System.Windows.Input;

namespace Canvas;

public class RelayCommand : ICommand
{
    private Action<Object> execute;
    private Func<object, bool>? canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }


    public bool CanExecute(object parameter)
    {
        return this.canExecute == null || canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        this.execute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}