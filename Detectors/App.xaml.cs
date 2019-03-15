using Detectors.UI;
using Detectors.UI.Controls;
using System.Windows;

namespace Counters
{
    public partial class App : Application
    {
        internal App()
		{

        }
		protected override void OnStartup(StartupEventArgs StartupEventArgs)
		{
            Detectors.UI.MainWindow.PreviousProcess = ControlPanel.PriorProcess();
			base.OnStartup(StartupEventArgs);
		}
	}
}
