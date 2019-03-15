using System;
using System.IO;
using System.Web.UI.HtmlControls;
using HtmlAgilityPack;
using OfficeOpenXml;
namespace Detectors.Http
{
	internal static class HTMLTableHelper
	{
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Static Variables            ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Thread Static Variables     ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Variables                   ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Constructors                ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Static Methods              ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		#endregion
		#region ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■   Methods                     ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
		internal static HtmlNode GetHtmlTable(string HtmlString, string TableId)
		{
			HtmlString = HtmlString.Replace("&nbsp;", "");
			HtmlDocument HtmlDocument = new HtmlDocument();
			HtmlDocument.LoadHtml(HtmlString);
			HtmlNode HtmlNode = HtmlDocument.DocumentNode.ChildNodes["html"];
			HtmlNode TableHtmlNode =
				HtmlNode
				.SelectSingleNode("//div[@id='segment_context_box']")
				.SelectSingleNode("//table[@class='inlayTable']");
			return TableHtmlNode;
		}
		private static int FillHeaderRow(int Row, ExcelWorksheet ExcelWorksheet, string HeaderRowString)
		{
			HtmlDocument HtmlDocument = new HtmlDocument();
			HtmlDocument.LoadHtml(HeaderRowString);
			HtmlNodeCollection TableColumns = HtmlDocument.DocumentNode.SelectNodes("//th");
			if (TableColumns == null) { return Row; }
			int Column = 1;
			foreach (HtmlNode TableColumn in TableColumns)
			{
				HtmlAttribute HtmlAttributeColumnSpan = TableColumn.Attributes["colspan"];
				int ColumnSpan = (HtmlAttributeColumnSpan != null)? int.Parse(HtmlAttributeColumnSpan.Value ):0;
				string Text= TableColumn.InnerText;
				if (ColumnSpan > 1)
				{
					ExcelRange Range = ExcelWorksheet.Cells[Row, Column, Row, Column + ColumnSpan - 1];

					Range.Merge = true;
					Range.Value = Text;
					Column += ColumnSpan;
				}
				else
				{
					ExcelRange Range = ExcelWorksheet.Cells[Row, Column];
					Range.Value = Text;
					Column ++;
				}
			}

			return Row + 1;
		}
		private static int FillHeaderRows(ExcelWorksheet ExcelWorksheet, string TableString)
		{
			HtmlDocument HtmlDocument = new HtmlDocument();
			HtmlDocument.LoadHtml(TableString);
			HtmlNodeCollection HeaderRows = HtmlDocument.DocumentNode.SelectNodes("//tr");
			int Row = 1;
			foreach (HtmlNode HeaderRow in HeaderRows)
			{
				Row = FillHeaderRow(Row, ExcelWorksheet, HeaderRow.InnerHtml);
			}
			return Row;
		}
		private static bool IsNumeric(string Text) => double.TryParse(Text, out double Number);
		private static bool IsInteger(string Text) => int.TryParse(Text, out int Number);
		private static int FillTableRow(int Row, ExcelWorksheet ExcelWorksheet, string TableRowString)
		{
			HtmlDocument HtmlDocument = new HtmlDocument();
			HtmlDocument.LoadHtml(TableRowString);
			HtmlNodeCollection TableColumns = HtmlDocument.DocumentNode.SelectNodes("//td");

			if (TableColumns == null) { return Row; }
			int Column = 1;
			foreach (HtmlNode TableColumn in TableColumns)
			{
				string Text = TableColumn.InnerText;
				ExcelRange Range = ExcelWorksheet.Cells[Row, Column];
				if (IsNumeric(Text))
				{
					Range.Value = IsInteger(Text) ? int.Parse(Text) : double.Parse(Text);
				}
				else { Range.Value = Text; }
				Column += 1;
			}

			return Row + 1;
		}
		private static int FillTableRows(ExcelWorksheet ExcelWorksheet, int Row, string TableString)
		{
			HtmlDocument HtmlDocument = new HtmlDocument();
			HtmlDocument.LoadHtml(TableString);
			HtmlNodeCollection HeaderRows = HtmlDocument.DocumentNode.SelectNodes("//tr");
			foreach (HtmlNode HeaderRow in HeaderRows)
			{
				Row = FillTableRow(Row, ExcelWorksheet, HeaderRow.InnerHtml);
			}
			return Row;
		}

		internal static bool SaveResponseToExcel(HtmlNode HtmlNodeTable, string FileName)
		{


			if (File.Exists(FileName)) { File.Delete(FileName); }
			using (ExcelPackage ExcelPackage = new ExcelPackage(new FileInfo(FileName)))
			{
				try
				{
					ExcelWorkbook ExcelWorkbook = ExcelPackage.Workbook;
					ExcelWorksheet ExcelWorksheet = ExcelWorkbook.Worksheets.Add("Data");
					string HeaderRows = HtmlNodeTable.SelectSingleNode("//thead").InnerHtml;
					int Row = FillHeaderRows( ExcelWorksheet, HeaderRows);
					string BodyRows = HtmlNodeTable.SelectSingleNode("//tbody").InnerHtml;
					int TotalRows =	 FillTableRows(ExcelWorksheet, Row, BodyRows);
					
					ExcelPackage.Save();
					return true;
				}
				catch (Exception) { return false; }
			}
		}
		#endregion
	}
}
