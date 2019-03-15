using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Detectors.Data;
using MahApps.Metro.Controls.Dialogs;

namespace Detectors.UI.Controls
{
	public partial class ControlPanel
	{
        public static readonly DependencyProperty ICON_WIDTH_PROPERTY = DependencyProperty.Register("IconWidth", typeof(int), typeof(ControlPanel), new UIPropertyMetadata(1, new PropertyChangedCallback(OnValueChanged)));
        public int IconWidth { get => (int)GetValue(ICON_WIDTH_PROPERTY); set => SetValue(ICON_WIDTH_PROPERTY, value); }
        private Timer Timer { get; } = new Timer { Interval = 1000 };
		public ControlPanel()
		{
			InitializeComponent();
			Timer.Tick += _Timer_Tick;
		}
		internal void SetTime(string Time)
		{

		}
		internal void StartTimer() => Timer.Start();
        internal void DisableTimer() { Timer.Interval = int.MaxValue; StartButton.IsEnabled = false; StopButton.IsEnabled = false; }
        private void _Timer_Tick(object sender, EventArgs e)
		{
		}

		private void Exit_Click(object sender, RoutedEventArgs e) => MainWindow.Window?.Close();
		public static Process PriorProcess()
		{
			string ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
			Process[] RunningProcesses = Process.GetProcessesByName(ApplicationName);
			if ((RunningProcesses?.Length ?? 0) < 2 ) { return null; }
            
			return RunningProcesses[0];
		}
        private static void OnValueChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs eDependencyPropertyChangedEventArgs) { }
        internal void CheckForPriorInstances(Process PreviousProcess)
		{
			try
			{
				if (PreviousProcess != null)
				{
					MessageDialogResult MessageDialogResult = MessageDialogResult.SecondAuxiliary;
					System.Windows.Application.Current.Dispatcher.Invoke(() =>
					{
						MetroDialogSettings MetroDialogSettings = new MetroDialogSettings
						{
							AffirmativeButtonText = "Close Instance",
							NegativeButtonText = "Abort",
							AnimateShow = true,
							AnimateHide = true,
							DialogMessageFontSize = 14,
							ColorScheme = MetroDialogColorScheme.Accented,
							DefaultButtonFocus = MessageDialogResult.FirstAuxiliary
						};
						string Title = "A Previous Instance of This Application Is Running!";
						string Body = "A Previous Instance Is Running! Close Instance and Continue or Abort Execution?";
						MessageDialogStyle MessageDialogStyle = MessageDialogStyle.AffirmativeAndNegative;
						MessageDialogResult = MainWindow.Window.ShowModalMessageExternal(Title, Body, MessageDialogStyle, MetroDialogSettings);

						if (MessageDialogResult == MessageDialogResult.Affirmative)
						{
							PreviousProcess.Kill();
							System.Threading.Thread.Sleep(50);
						}
						else
						{
							MainWindow.Window.Close();
						}
					});

				}

			}
			catch (Exception) { }
		}
		private void Start()
		{
			if (MainWindow.Scenario == null) {
				MainWindow.Scenario =  Scenario.Load(@"C:\CS\Others\New folder\TD\Detectors\CalTrans\CalTrans.json");
				Data.Detectors.CreateDetectors(MainWindow.Scenario);
			}
			
			//string Url = "http://pems.dot.ca.gov/";
			//bool Ok = DetectorCounters.Login(Url, "ahmedehassan@gmail.com", "water7%B~!u");
			//if (Ok)
			//{
			//	Ok = DetectorCounters.Download("http://pems.dot.ca.gov/?report_form=1&dnode=Freeway&content=spatial&tab=contours&export=&fwy=10&dir=E&s_time_id=1551398400&s_time_id_f=03%2F01%2F2019&from_hh=6&to_hh=18&start_pm=.17&end_pm=0.18&lanes=&station_type=ml&q=flow&colormap=30%2C31%2C32&sc=auto&ymin=&ymax=&view_d=2&html.x=53&html.y=11");
			//}

			StartButton.IsEnabled = false;
			StartTimer();
		}
		private void Start_Click(object sender, RoutedEventArgs e) => Start();
        private void Stop_Click(object sender, RoutedEventArgs e) { Timer.Stop(); StartButton.IsEnabled = true; }

    }
}
