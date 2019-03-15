using System;
using System.IO;
using Detectors.Http;
using OfficeOpenXml;

namespace Detectors.Data
{
	internal class Detector : IDisposable
	{
		private static string Url    { get; } = $"";
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Variables                   ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		internal string Id           { get; private set; } = string.Empty;
		internal string Fwy          { get; private set; } = string.Empty;
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

		internal string FormData     { get => $"{{ \"arguments\":\"{Id}\" }}"; }
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
				District   = ExcelRange            [Row, 02].Value?.ToString()?? "",
				County     = ExcelRange            [Row, 03].Value?.ToString()?? "",
				City       = ExcelRange            [Row, 04].Value?.ToString()?? "",
				CAPM       = ExcelRange            [Row, 05].Value?.ToString()?? "",
				AbsPM      = float.Parse(ExcelRange[Row, 06].Value?.ToString()?? "0"),
				Length     = ExcelRange            [Row, 07].Value?.ToString()?? "",
			    Id         = ExcelRange            [Row, 08].Value?.ToString()?? "",
				Name       = ExcelRange            [Row, 09].Value?.ToString()?? "",
				Lanes      = ExcelRange            [Row, 10].Value?.ToString()?? "",
				Type       = ExcelRange            [Row, 11].Value?.ToString()?? "",
				SensorType = ExcelRange            [Row, 12].Value?.ToString()?? "",
				HOV        = ExcelRange            [Row, 13].Value?.ToString()?? "",
				MSID       = ExcelRange            [Row, 14].Value?.ToString()?? "",
				IRM        = ExcelRange            [Row, 15].Value?.ToString() ?? ""
			};
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

		internal void DownloadAndSave(string Url, string RoadwayFolder)
		{
			bool Fail = false;
			do
			{
				try
				{

					Response = DetectorCounters.DownloadPage(Url, this);
					SaveResponsToImageFile(RoadwayFolder);
					Fail = false;
				}
				catch (Exception) { Fail = true; }
			} while (Fail);
		}
		#endregion
	}
}
