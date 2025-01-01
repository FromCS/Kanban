using System.ComponentModel;

namespace Canvas.Model.ProjectModel;

public interface IProject : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string WorkCategory { get; set; }
    
    public string Priority { get; set; }

}