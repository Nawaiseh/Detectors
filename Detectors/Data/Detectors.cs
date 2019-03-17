using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Detectors.Http;
using Detectors.UI;
using Detectors.UI.Controls;
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
        private static void DownloadDetectorData(Detector Detector, Scenario Scenario, List<string> Options, Date Start, Date End)
        {
            string RootFolder = Path.Combine(Scenario.FolderSettings.Scenario, Scenario.FolderSettings.Output);
            foreach (string Option in Options) {
                string SaveFolder = Path.Combine(RootFolder, Option, Detector.Id);
                if (!Directory.Exists(SaveFolder)) { Directory.CreateDirectory(SaveFolder); }
                bool Ok = false;
                int TriesCount = 0;
                do
                {
                    TriesCount++;
                    App.Current.Dispatcher.Invoke(() => { Ok = Detector.DownloadData(Scenario, Start, End, Option, SaveFolder); });
                } while (!Ok && TriesCount <= 10);
            }
        }
		internal static void DownloadCountersData(Scenario Scenario, List<string> Options, Date Start, Date End)
		{
            foreach (Detector Detector in DetectorsList.Values) { DownloadDetectorData(Detector, Scenario, Options, Start, End); }
        }
        internal static void StartDownloading(Scenario Scenario, Date Start, Date End, DownloadOptions DownloadOptions)
		{
            List<string> Folders = new List<string>();
            if (DownloadOptions.All) { Folders.Add("speed"); Folders.Add("flow"); Folders.Add("occ"); }
            else if (DownloadOptions.Speed) { Folders.Add("speed"); }
            else if (DownloadOptions.Flow) { Folders.Add("flow"); }
            else if (DownloadOptions.Occupancy) { Folders.Add("occ"); }
            
			Thread Thread = new Thread(() => DownloadCountersData(Scenario, Folders, Start, End));
			Thread.Start();
		}

		#endregion
	}
}
