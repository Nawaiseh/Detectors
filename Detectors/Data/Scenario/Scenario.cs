using System.IO;

using Detectors.Tools.Json;
using Detectors.Tools.Resources;
using Detectors.UI;

using MahApps.Metro.Controls.Dialogs;

using Newtonsoft.Json;

namespace Detectors.Data
{
	internal class Scenario : JReader
	{
		internal static string DefaultErrorsFileName                                                                                { get; set; } = "Errors.Log";
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   JSON Loaded Variables       ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		[JsonProperty("Name"             , NullValueHandling = NullValueHandling.Ignore)] internal string         Name              { get; set; }
		[JsonProperty("Username"         , NullValueHandling = NullValueHandling.Ignore)] internal string         Username          { get; set; }
		[JsonProperty("Password"         , NullValueHandling = NullValueHandling.Ignore)] internal string         Password          { get; set; }
		[JsonProperty("Url"              , NullValueHandling = NullValueHandling.Ignore)] internal string         Url               { get; set; }
		[JsonProperty("Start"            , NullValueHandling = NullValueHandling.Ignore)] internal CustomDate     Start             { get; set; }
		[JsonProperty("End"              , NullValueHandling = NullValueHandling.Ignore)] internal CustomDate     End               { get; set; }
		[JsonProperty("FolderSettings"   , NullValueHandling = NullValueHandling.Ignore)] internal FolderSettings FolderSettings    { get; set; }
		[JsonProperty("DetectorsFileName", NullValueHandling = NullValueHandling.Ignore)] internal string         DetectorsFileName { get; set; } = "Input";
		[JsonProperty("ErrorsFileName"   , NullValueHandling = NullValueHandling.Ignore)] internal string         ErrorsFileName    { get; set; } = "Errors.Log";
		#endregion

		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		private static string Schema() => ResourceLoader.Load("Detectors.Data.Scenario.ScenarioSchema.json");
		internal static Scenario Load(string FileName)
		{
			// Calling a Method in C# Allows to use any order of Parameters With the use of ParameterName followed by : then Parameter value
			// The Load<Scenario> () method can be called using the defauld order of parameters as they are described in the Method Definition
			//         Called as  ==> Load<Scenario>(FileName, Schema(), false, true, true);
			Scenario Scenario = Load<Scenario>(FileName, Schema(), IsSchemaFileName: false, IgonreSpace: false, IgnoreCase: false);
			if (Scenario == null)
			{
				MainWindow MainWindow = MainWindow.Window;
				MetroDialogSettings MetroDialogSettings = new MetroDialogSettings
				{
					AffirmativeButtonText = "Abort",
					AnimateShow           = true,
					AnimateHide           = true,
					DialogMessageFontSize = 14,
					ColorScheme           = MetroDialogColorScheme.Accented,
					DefaultButtonFocus    = MessageDialogResult.FirstAuxiliary
				};
				string Title = "Failed To Load Scenario File!";
				string Body = $"Failed To Load [{new FileInfo(FileName).Name}]!";
				Body = $"\n{Body} The File Does not Meet The Requirements Set for a Scenario File!";
				Body = $"\n{Body} Please Check  [Desktop\\{DefaultErrorsFileName}]";
				Body = $"\n{Body} Execution Will Stop!";
				MessageDialogResult Result = MainWindow.ShowModalMessageExternal(Title, Body, MessageDialogStyle.Affirmative, MetroDialogSettings);
				return null;
			}
			Scenario.Initialize(FileName);
			return Scenario;

		}
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Methods                     ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		private void Initialize(string FileName)
		{
			Start.Initialize();
			End.Initialize();
		}
		#endregion
	}
}
