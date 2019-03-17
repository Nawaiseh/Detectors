using System;
using System.Collections.Generic;
using System.IO;
using Detectors.Http;
using Detectors.UI.Controls;
using OfficeOpenXml;

namespace Detectors.Data
{


	internal class Detector : IDisposable
	{
        internal static SortedList<string, string> StationTypes { get; } = new SortedList<string, string> {
            { "On Ramp"                 , "onr" },
            { "Off Ramp"                , "offr"},
            { "Conventional Highway"    , "ch"  },
            { "Fwy-Fwy"                 , "ff"  },
            { "HOV"                     , "hv" },
            { "Mainline"                , "ml"  },
            { "Coll-Dist"               , "cd"  }
        };
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Variables                   ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		internal string Id           { get; private set; } = string.Empty;
        internal string Fwy          { get; private set; } = string.Empty;
        internal int    FwyId        { get; private set; } = 0;

        internal string District     { get; private set; } = string.Empty;
		internal string County 	     { get; private set; } = string.Empty;
		internal string City         { get; private set; } = string.Empty;
		internal string CAPM   	     { get; private set; } = string.Empty;
		internal float AbsPM  	     { get; private set; } = -1;
		internal string Length 	     { get; private set; } = string.Empty;
		internal string Name 	     { get; private set; } = string.Empty;
		internal string Lanes        { get; private set; } = string.Empty;
		internal string Type 	     { get; private set; } = string.Empty;
		internal string SensorType   { get; private set; } = string.Empty;
		internal string HOV          { get; private set; } = string.Empty;
		internal string MSID         { get; private set; } = string.Empty;
		internal string IRM          { get; private set; } = string.Empty;

		internal string Response     { get; set; }
		internal string FileName     { get; set; }
		internal string SourceString { get; private set; }
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		internal static Detector CreateDetector(ExcelRange ExcelRange, int Row)
		{
			//Excel is 1 Based Rows and Columns
			if (ExcelRange == null) { return null; }

			//  1      2          3      4       5       6      7     7    8        9      10         11       12     13      14
			// Fwy  District    County  City    CAPM   AbsPM  Length  ID  Name    Lanes   Type    SensorType   HOV   MSID     IRM
			Detector Detector = new Detector
			{
				Fwy        = ExcelRange            [Row, 01].Value?.ToString()?? "",
                FwyId      = int.Parse(ExcelRange  [Row, 04].Value?.ToString() ?? "0"),
                District   = ExcelRange            [Row, 05].Value?.ToString()?? "",
				County     = ExcelRange            [Row, 06].Value?.ToString()?? "",
				City       = ExcelRange            [Row, 07].Value?.ToString()?? "",
				CAPM       = ExcelRange            [Row, 08].Value?.ToString()?? "",
				AbsPM      = float.Parse(ExcelRange[Row, 09].Value?.ToString()?? "0"),
				Length     = ExcelRange            [Row, 10].Value?.ToString()?? "",
			    Id         = ExcelRange            [Row, 11].Value?.ToString()?? "",
				Name       = ExcelRange            [Row, 12].Value?.ToString()?? "",
				Lanes      = ExcelRange            [Row, 13].Value?.ToString()?? "",
				Type       = ExcelRange            [Row, 14].Value?.ToString()?? "",
				SensorType = ExcelRange            [Row, 15].Value?.ToString()?? "",
				HOV        = ExcelRange            [Row, 16].Value?.ToString()?? "",
				MSID       = ExcelRange            [Row, 17].Value?.ToString()?? "",
				IRM        = ExcelRange            [Row, 18].Value?.ToString() ?? ""
			};

            string ID = Detector.Fwy.Substring(0, Detector.Fwy.IndexOf("-") - 1);

			return Detector;
		}
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Methods                     ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		internal void SaveResponsToImageFile( string Folder)
		{
			int StartIndex        = Response.IndexOf(@"data:image\/jpeg;base64,") + @"data:image\/jpeg;base64,".Length;
			int Length            = Response.Length - StartIndex - 1;
			string EncryptedImage = Response.Substring(StartIndex, Length).Replace("\\", "");
			byte[] Bytes          = Convert .FromBase64String(EncryptedImage);

			using (FileStream imageFile = new FileStream(Path.Combine(Folder, FileName), FileMode.Create))
			{
				imageFile.Write(Bytes, 0, Bytes.Length);
				imageFile.Flush();
			}
		}
		public void Dispose() {  }
        internal string Dir { get => $"{Fwy.ToCharArray()[Fwy.Length - 1]}"; }
        internal string UrlReportForm { get => "report_form=1"; }
        internal string UrlNode { get => $"dnode=Freeway"; }
        internal string UrlContent { get => "content=spatial"; }
        internal string UrlTabs { get => "tab=contours"; }
        internal string UrlExport { get => $"export="; }
        internal string UrlFreeway { get => $"fwy={FwyId}"; }  // "fwy=10"
        internal string UrlDirection { get => $"dir={Dir}"; }
        internal string UrlPosition { get => $"start_pm={Math.Round(AbsPM - 0.01, 3)}&end_pm={Math.Round(AbsPM + 0.01, 3)}"; }
        internal string UrlLanes { get => $"lanes={Lanes}"; }
        internal string UrlStationType { get => $"station_type={StationTypes[Type]}"; }
        internal string UrlFormData { get => $"/?{UrlReportForm}&{UrlNode}&{UrlContent}&{UrlTabs}&{UrlExport}&{UrlFreeway}&{UrlDirection}"; }
        private static DateTime Year1970 { get; } = new DateTime(1970, 1, 1);
        private static TimeSpan OneDay { get; } = new TimeSpan(1, 0, 0, 0);
        private string DateToString(Date Start, Date End, DateTime CurrentDate)
        {
            string UrlDate = $"s_time_id_f={DateTime.Now.ToString(@"MM\\d\\yyyy")}";
            int TimeId = (int)(CurrentDate - Year1970).TotalSeconds;
            string UrlTimeId = $"s_time_id={TimeId}";
            string UrlFrom = $"from_hh={Start.SelectedTime.Hours}";
            string UrlTo = $"to_hh={End.SelectedTime.Hours}";
            return $"{UrlTimeId}&{UrlDate}&{UrlFrom}&{UrlTo}";
        }
       

        internal bool DownloadData(Scenario Scenario, Date Start, Date End, string Option, string SaveFolder)
		{
            string Url = Scenario.Url;
            for (DateTime CurrentDate = Start.SelectedDate; CurrentDate <= End.SelectedDate; CurrentDate += OneDay) {

                int TimeId = (int)(CurrentDate - Year1970).TotalSeconds;
                string DateId = CurrentDate.ToString(@"MM\\d\\yyyy");
                string FileName = $"{CurrentDate.ToString(@"yyyy_MM_d")}.xlsx";
                FileName = Path.Combine(SaveFolder, FileName);
                string UrlParameters = $"{UrlFormData}&{DateToString(Start, End, CurrentDate)}&{UrlPosition}&{UrlLanes}&{UrlStationType}&q={Option}&colormap=30%2C31%2C32&sc=auto&ymin=&ymax=&view_d=2&html.x=53&html.y=11";
                string FullUrl = $"{Scenario.Url}{UrlParameters}";
                bool Fail = false;
                do
                {
                    try
                    {
                        Fail = !DetectorCounters.Download(FullUrl, this, Option, TimeId, DateId, Start.SelectedTime.Hours, End.SelectedTime.Hours, FileName);
                        
                    }
                    catch (Exception) { Fail = true; }
                } while (Fail);
            }
           
            return true;
		}
		#endregion
	}
}
