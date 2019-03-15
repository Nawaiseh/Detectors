﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;

using Detectors.Data;
using HtmlAgilityPack;

namespace Detectors.Http
{
	internal static class DetectorCounters
	{
		private static string Response { get; set; } = string.Empty;
		private static MyWebClient WebClient { get; } = new MyWebClient();
		private static CookieContainer CreateLoginCookieContainer(string Url)
		{
			string Domain = new Uri(Url).Host; //"its.txdot.gov";

			CookieContainer CookieContainer = new CookieContainer(20);
			CookieContainer.Add(new Cookie("PHPSESSID", "2484112e1103e620aad1d3af29769b58") { Domain = Domain });
			CookieContainer.Add(new Cookie("__utma", "267661199.373903201.1551798485.1551798485.1551798485.1") { Domain = Domain });
			CookieContainer.Add(new Cookie("__utmc", "267661199") { Domain = Domain });
			CookieContainer.Add(new Cookie("__utmz", "267661199.1551798485.1.1.utmcsr = (direct) | utmccn = (direct) | utmcmd = (none)") { Domain = Domain });
			CookieContainer.Add(new Cookie("__utmt", "1") { Domain = Domain });
			CookieContainer.Add(new Cookie("__utmb", "267661199.11.10.1551798485") { Domain = Domain });

			return CookieContainer;
		}
		private static byte[] UpdateHttpWebRequestFormPayLoad(HttpWebRequest HttpWebRequest, string FormData)
		{
			byte[] EncodedFromData = Encoding.ASCII.GetBytes(FormData);
			EncodedFromData = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, EncodedFromData);
			Stream RequestStream = HttpWebRequest.GetRequestStream();
			RequestStream.Write(EncodedFromData, 0, EncodedFromData.Length);
			return EncodedFromData;
		}
		private static string FormatText(string InputText, char Separator = ':')
		{
			string[] Lines = InputText.Split('\n');
			string FormattedText = string.Empty;
			foreach (string Line in Lines)
			{
				if (string.IsNullOrEmpty(Line.Trim().Replace("\r", ""))) { continue; }
				string[] Tokens = Line.Replace("\r", "").Split(Separator);
				string Token1 = Tokens[0];
				string RemainderText = string.Empty;
				foreach (string Part in Tokens) { if (Part == Token1) { continue; } RemainderText = $"{RemainderText}:{Part}"; }
				if (RemainderText.Length > 0)
				{
					RemainderText = RemainderText.Substring(1, RemainderText.Length - 1).Trim();
					FormattedText = $"{FormattedText}\"{Token1}\":\"{RemainderText}\",\n";
				}

			}
			return FormattedText;
		}
		private static Stream GetHttpWebRequestResponse(HttpWebRequest HttpWebRequest)
		{
			try
			{
				HttpWebResponse HResponse = (HttpWebResponse)HttpWebRequest.GetResponse();
				return HResponse.GetResponseStream();
			}
			catch (WebException)
			{
				File.WriteAllLines("Errors.json", new string[] { "{\n\"Headers\": {\n\t", FormatText(HttpWebRequest.Headers.ToString()), "\n}\n," });
				throw;
			}
		}


		// Main Method
		public static string XMLHttpRequest(string Url, string FormData, string ContentType)
		{
			HttpWebRequest HttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			HttpWebRequest.Method = "POST";
			byte[] EncodedFromData = null;
			try
			{
				//FillHttpWebRequestHeaders(HttpWebRequest, Url, FormData, ContentType);

				EncodedFromData = UpdateHttpWebRequestFormPayLoad(HttpWebRequest, FormData);

				using (Stream ResponseStream = GetHttpWebRequestResponse(HttpWebRequest))

				using (StreamReader StreamReader = new StreamReader(ResponseStream, Encoding.UTF8)) { return StreamReader.ReadToEnd(); }

			}
			catch (WebException WebException)
			{
				string FormPayLoadDecrypted = Encoding.ASCII.GetString(EncodedFromData).Trim();
				FormPayLoadDecrypted = FormPayLoadDecrypted.Substring(1, FormPayLoadDecrypted.Length - 1).Trim();
				FormPayLoadDecrypted = FormPayLoadDecrypted.Substring(0, FormPayLoadDecrypted.Length - 1).Trim();
				File.AppendAllLines("Errors.json", new string[] { "\"FormData\": {\n\t", $"{FormPayLoadDecrypted.Replace("\",\"", "\": \"")}}},\n" });

				using (WebResponse WebResponse = WebException.Response)
				{
					using (Stream respStream = WebResponse.GetResponseStream())
					{
						StreamReader StreamReader = new StreamReader(respStream);
						string ErrorResponse = StreamReader.ReadToEnd();
						string Msg = ErrorResponse.Substring(1, ErrorResponse.Length - 1);
						Msg = Msg.Substring(0, Msg.Length - 1);
						File.AppendAllLines("Errors.json", new string[] { Msg, "\n}" });

						return ErrorResponse;
					}
				}
			}
		}

		private static void ToImage(string EncryptedImage, string FileName)
		{
			byte[] Bytes = Convert.FromBase64String(EncryptedImage);
			using (FileStream imageFile = new FileStream(FileName, FileMode.Create))
			{
				imageFile.Write(Bytes, 0, Bytes.Length);
				imageFile.Flush();
			}
		}

		internal static bool Login(string Url, string Username, string Password)
		{
			try
			{
				NameValueCollection Parameters = new NameValueCollection
				{
					["redirect"] = "",
					["username"] = Username,
					["password"] = Password,
					["login"] = "Login"
				};
				byte[] Response_Bytes = WebClient.UploadValues(Url, "POST", Parameters);
				string Response = Encoding.UTF8.GetString(Response_Bytes);
				return true;
			}
			catch (Exception) {
				return false;
				}
		}
		private static void SetRequestHeaders(string Url)
		{
			WebClient.Headers[HttpRequestHeader.Accept] = "*/*";
			WebClient.Headers["Accept-Encoding"] = "gzip, deflate";
			WebClient.Headers["Accept-Language"] = "en-US,en;q=0.9,ar;q=0.8";
			WebClient.Headers[HttpRequestHeader.KeepAlive] = "true"; 
			//WebClient.Headers[HttpRequestHeader.ContentLength] = "0";
			//WebClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=UTF-8";
			WebClient.Headers["DNT"]= "1";
			WebClient.Headers["Origin"]= "http://pems.dot.ca.gov";
			WebClient.Headers[HttpRequestHeader.Host] = "pems.dot.ca.gov";

			WebClient.Headers[HttpRequestHeader.Referer] = Url;
			WebClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36";
			WebClient.Headers["X-Requested-With"]= "XMLHttpRequest";
			WebClient.Headers[HttpRequestHeader.Expect] = null;

		}

		internal static bool Download(string Url)
		{
			try
			{
				SetRequestHeaders(Url);
				NameValueCollection Parameters = new NameValueCollection
				{
					["report_form"] = "1",
					["dnode"] = "Freeway",
					["content"] = "spatial",
					["tab"] = "contours",
					["export"] = "",
					["fwy"] = "10",
					["dir"] = "E",
					["s_time_id"] = "1551398400",
					["s_time_id_f"] = "03/01/2019",  
					["from_hh"] = "6",
					["to_hh"] = "18",
					["start_pm"] = ".17",
					["end_pm"] = "0.18",
					["lanes"] = "",
					["station_type"] = "ml",
					["q"] = "flow",
					["colormap"] = "30,31,32", 
					["sc"] = "auto",
					["ymin"] = "",
					["ymax"] = "",
					["view_d"] = "2"
				};
				byte[] Response_Bytes = WebClient.GetPostResponse(Url, Parameters);
				//string Response = WebClient.DownloadString(Url, "POST", Parameters)
				string Response = Encoding.UTF8.GetString(Response_Bytes);
				HtmlNode HtmlTable = HTMLTableHelper.GetHtmlTable(Response, "inlayTable");
				HTMLTableHelper.SaveResponseToExcel(HtmlTable, @"C:\CS\Others\New folder\TD\Detectors\Detectors\Test.xlsx");
				return true;
			}
			catch { return false; }
		}
		internal static void DownloadPage(string Url, string SavePath, List<KeyValuePair<string, string>> Queries) => Url = "http://its.txdot.gov/ITS_WEB/FrontEnd/svc/DataRequestWebService.svc/GetCctvContent";//WebClient.BaseAddress

		internal static string DownloadPage(string Url, Detector Camera) => XMLHttpRequest(Url, Camera.FormData, "application/json; charset=UTF-8");

	}
}
