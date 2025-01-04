using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Canvas.Model;

namespace Canvas;

public static class Utils
{
    public static void SetupCorrectID(ref ObservableCollection<Step> legend)
    {
        try
        {
            int idNumber = 1;

            void Iter(ObservableCollection<Step> steps, int parentID)
            {
                foreach (var step in steps)
                {
                    MessageBox.Show(step.StepName + "  " + idNumber);
                    step.ID = idNumber;
                    idNumber += 1;
                    if (step.ParentId != null)
                    {
                        step.ParentId = parentID;
                    }

                    if (step.Steps.Count > 0)
                    {
                        Iter(step.Steps, step.ID);    
                    }
                    
                }
            }
            Iter(legend, 0);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }
    
    public static ObservableCollection<Step> GetFlatSteps(ObservableCollection<Step> steps)
    {
        ObservableCollection<Step> flatSteps = new ObservableCollection<Step>();
        if (steps.Count < 1) return flatSteps;
        foreach (var currentStep in steps)
        {
            flatSteps.Add(currentStep);
            var nestedSteps = GetFlatSteps(currentStep.Steps);
            nestedSteps.ToList().ForEach(nestedStep => flatSteps.Add(nestedStep));
        }    
        return flatSteps;
    }
}