using Detectors.Data;
using Detectors.Http;
using System;
using System.Diagnostics;
using System.Windows;

namespace Detectors.UI
{
	public partial class MainWindow
	{
        public static readonly DependencyProperty PROJECT_NAME_PROPERTY = DependencyProperty.Register("ProjectName", typeof(string), typeof(MainWindow), new UIPropertyMetadata("", new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty PROJECT_PATH_PROPERTY = DependencyProperty.Register("ProjectPath", typeof(string), typeof(MainWindow), new UIPropertyMetadata("", new PropertyChangedCallback(OnValueChanged)));
        public string ProjectName { get => (string)GetValue(PROJECT_NAME_PROPERTY); set => SetValue(PROJECT_NAME_PROPERTY, value); }
        public string ProjectPath { get => (string)GetValue(PROJECT_PATH_PROPERTY); set => SetValue(PROJECT_PATH_PROPERTY, value); }
        internal static MainWindow Window { get; private set; }
		internal static Scenario Scenario { get;         set; }
        internal static Process PreviousProcess { get;   set; }
        public MainWindow()
		{
            PreviousProcess = Controls.ControlPanel.PriorProcess();
            InitializeComponent();
			Window = this;
		}
        private static void OnValueChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs) { }
        private void Open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ControlPanel.CheckForPriorInstances(PreviousProcess);

        }
    }
}
