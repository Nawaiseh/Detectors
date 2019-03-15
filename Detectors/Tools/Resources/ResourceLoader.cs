using System;
using System.IO;
using System.Reflection;
using System.Windows;

using Detectors.UI;

using MahApps.Metro.Controls.Dialogs;

namespace Detectors.Tools.Resources
{
	internal static class ResourceLoader
	{
		internal static string Load(string ResourceName)
		{
			try
			{
				Assembly Assembly = Application.Current.GetType().Assembly;
				using (Stream Stream = Assembly.GetManifestResourceStream(ResourceName))
				{
					using (StreamReader StreamReader = new StreamReader(Stream))
					{
						string Text = StreamReader.ReadToEnd();
						return Text;
					}
				}
			}
			catch (Exception)
			{
				MainWindow MainWindow                   = MainWindow.Window;
				MetroDialogSettings MetroDialogSettings = new MetroDialogSettings
				{
					AffirmativeButtonText = "Continue Without Using a Schema to Validate Scenario File",
					NegativeButtonText    = "Abort",
					AnimateShow           = true,
					AnimateHide           = true,
					DialogMessageFontSize = 14,
					ColorScheme           = MetroDialogColorScheme.Accented,
					DefaultButtonFocus    = MessageDialogResult.FirstAuxiliary
				};
				string Title                = "Failed To Load The Schema File From Resources!";
				string Body                 = "Failed To Load Schema File!\n Do You Want To Continue Without Using Schema File or Abort Execution?";
				MessageDialogResult Result  = MainWindow.ShowModalMessageExternal(Title, Body, MessageDialogStyle.AffirmativeAndNegative, MetroDialogSettings);
				return null;
			}
		}

	}
}
