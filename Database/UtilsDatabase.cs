using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Canvas.Model;

namespace Canvas.Database;

public static class UtilsDatabase
{
    public static Step FindStepByID(ObservableCollection<Step> legend, int? id) // ошибка здесь
    {
        Step result = new Step();
        foreach (var step in legend)
        {
            if (step.ID == id) return step;
            if (step.Steps.Count > 0) return FindStepByID(step.Steps, id);
        }
        return result;
    }
    public static ObservableCollection<Step> CreateLegendTreeStructure(ObservableCollection<Step> legend) // 03.01.2025 12:44 DO!!!
    {
        ObservableCollection<Step> treeLegend = new ObservableCollection<Step>();
        Dictionary<int?, ObservableCollection<Step>> stepsByparentID = new Dictionary<int?, ObservableCollection<Step>>();
        legend.ToList().ForEach(step =>
        {
            if (stepsByparentID.ContainsKey(step.ParentId))
            {
                stepsByparentID[step.ParentId].Add(step);
            }
            else
            {
                stepsByparentID[step.ParentId] = new ObservableCollection<Step> { step };
            }
        });
        foreach (var pair in stepsByparentID)
        {
            if (pair.Key == 0)
            {
                treeLegend = pair.Value;
            }
            else
            {
                Step nestedStep = FindStepByID(treeLegend, pair.Key);
                nestedStep.Steps = pair.Value;
                nestedStep.Steps.ToList().ForEach(step =>
                {
                    step.ParentSteps = nestedStep.Steps;
                });
            }
        }
        return treeLegend;
    } 
}