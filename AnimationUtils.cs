using System.Linq;
using System.Windows;

namespace Canvas;

public static class AnimationUtils
{
    public static void OffsetMainWindowToLeftWithAnimation(MainWindow mainWindow)
    {
        for (int i = 0; i < 125; i += 1)
        {
            mainWindow.Left -= 1;
        }
    }

    public static void OffsetMainWindowToRightWithAnimation(MainWindow mainWindow)
    {
        for (int i = 0; i < 125; i += 1)
        {
            mainWindow.Left += 1;
        }
    }

    public static void CloseLegendAdditionalWindow(MainWindow mainWindow)
    {
        if (mainWindow.Grid.ColumnDefinitions.Last().Width.Value > 0)
        {
            OffsetMainWindowToRightWithAnimation(mainWindow);
        }
        mainWindow.Grid.ColumnDefinitions.Last().Width = new GridLength(0);
        mainWindow.Width = 850;
        
    }
    
    public static void OpenLegendAdditionalWindow(MainWindow mainWindow)
    {
        
    }
    
}