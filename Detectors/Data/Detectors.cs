using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Detectors.UI;

using MahApps.Metro.Controls.Dialogs;

using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Detectors.Data
{
	internal class Detectors
	{
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Static Variables            ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		private static string Url { get; } = "http://its.txdot.gov/ITS_WEB/FrontEnd/svc/DataRequestWebService.svc/GetCctvContent";
		private static SortedList<string, Detector> DetectorsList { get; } = new SortedList<string, Detector>();
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		private static bool FileExists(string FileName)
		{
			if (!File.Exists(FileName))
			{
				MessageDialogResult MessageDialogResult = MessageDialogResult.SecondAuxiliary;
				System.Windows.Application.Current.Dispatcher.Invoke(() =>
				{
					MetroDialogSettings MetroDialogSettings = new MetroDialogSettings
					{
						AffirmativeButtonText = "Close Focus Instance",
						NegativeButtonText = "Abort",
						AnimateShow = true,
						AnimateHide = true,
						DialogMessageFontSize = 14,
						ColorScheme = MetroDialogColorScheme.Accented,
						DefaultButtonFocus = MessageDialogResult.FirstAuxiliary
					};
					string Title = "A Previous Instance of Focus Is Running!";
					string Body = "A Previous Instance of Focus Is Running! Close Instance and Continue or Abort Execution?";
					MessageDialogStyle MessageDialogStyle = MessageDialogStyle.AffirmativeAndNegative;
					MessageDialogResult = MainWindow.Window.ShowModalMessageExternal(Title, Body, MessageDialogStyle, MetroDialogSettings);

					if (MessageDialogResult == MessageDialogResult.Affirmative)
					{
						//	PreviousProcess.Kill();
						System.Threading.Thread.Sleep(50);
					}
					else
					{
						MainWindow.Window.Close();
					}
				});

				return false;
			}

			return true;
		}
		private static bool ProcessDetectorsTable(ExcelWorksheet ExcelWorksheet, string TableName = "Detectors")
		{
			if (ExcelWorksheet == null) { return false; }
			ExcelTableCollection Tables = ExcelWorksheet.Tables;
			if (Tables == null) { return false; }
			ExcelTable DetectorsTable = Tables[TableName] ?? Tables[0];
			if (DetectorsTable == null) { return false; }
			ExcelRange ExcelRange = ExcelWorksheet.Cells[DetectorsTable.Address.Address];
			return ProcessExcelRange(ExcelRange);
		}
		private static bool ProcessRows(ExcelWorksheet ExcelWorksheet)
		{
			if (ExcelWorksheet == null) { return false; }
			int RowsCount = 1;
			while (true) { if (ExcelWorksheet.Cells[RowsCount, 1].Value != null) { break; } RowsCount++; }
			if (RowsCount == 1) { return false; }
			int ColumnsCount = 1;
			while (true) { if (ExcelWorksheet.Cells[1, ColumnsCount].Value != null) { break; } ColumnsCount++; }
			if (ColumnsCount < 15) { return false; }
			ExcelRange ExcelRange = ExcelWorksheet.Cells[1,1, RowsCount, ColumnsCount];
			return ProcessExcelRange(ExcelRange);
		}
		private static bool ProcessExcelRange(ExcelRange ExcelRange)
		{
			if (ExcelRange == null) { return false; }
			int Rows = ExcelRange.Rows;
			int Columns = ExcelRange.Columns;
			DetectorsList.Clear();
			for (int Row = 2; Row < Rows; Row++)
			{
				Detector Detector = Detector.CreateDetector(ExcelRange, Row);
				if (Detector == null) { continue; }
				DetectorsList.Add(Detector.Id, Detector);

			}
			return (DetectorsList.Count > 0);

		}
		internal static bool CreateDetectors(Scenario Scenario)
		{
			bool Status = false;
			string FileName = Path.Combine(Scenario.FolderSettings.Scenario, Scenario.FolderSettings.Input, Scenario.DetectorsFileName);
			if (!FileExists(FileName)) { return Status; }
			using (ExcelPackage ExcelPackage = new ExcelPackage(new FileInfo(FileName)))
			{
				using (ExcelWorkbook ExcelWorkbook = ExcelPackage.Workbook)
				{
					using (ExcelWorksheets ExcelWorksheets = ExcelWorkbook.Worksheets)
					{
						using (ExcelWorksheet ExcelWorksheet = ExcelWorksheets[1])
						{
							Status = ProcessDetectorsTable(ExcelWorksheet);
						}

					}
				}
			}
			return Status;
		}

		internal static void DownloadCountersImagery(string Folder, string Date, string Time)
		{
			//Folder = Path.Combine(Folder, $"{Date}__{Time}");
			//foreach (string Region in DetectorsList.Keys)
			//{
			//	string RegionFolder = Path.Combine(Folder, Region);
			//	SortedList<string, SortedList<string, Detector>> RegionDetectors = DetectorsList[Region];
			//	foreach (string Roadway in RegionDetectors.Keys)
			//	{
			//		string RoadwayFolder = Path.Combine(RegionFolder, Roadway);
			//		SortedList<string, Detector> RoadwayDetectors = RegionDetectors[Roadway];
			//		if (!Directory.Exists(RoadwayFolder)) { Directory.CreateDirectory(RoadwayFolder); }
			//		foreach (Detector Camera in RoadwayDetectors.Values) { Camera.DownloadAndSave(Url, RoadwayFolder); }
			//	}
			//}
		}

		internal static void StartDownloading(DateTime TimeStamp)
		{
			string Folder = Path.Combine(Environment.CurrentDirectory, "Output");
			string Date = $"TimeStamp___{TimeStamp.ToString(@"yyyy_MM_dd")}";
			string Time = TimeStamp.ToString(@"hh_mm_ss_tt");
			Thread Thread = new Thread(() => DownloadCountersImagery(Folder, Date, Time));
			Thread.Start();
		}

		#endregion
	}
}
