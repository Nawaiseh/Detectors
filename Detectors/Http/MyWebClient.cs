using System;
using System.Collections.Specialized;
using System.Net;

namespace Detectors.Http
{
	internal class MyWebClient : WebClient
	{
		internal int? Timeout { get; set; }
		internal CookieContainer CookieContainer { get; private set; }
		public MyWebClient() => CookieContainer = new CookieContainer();
		protected override WebRequest GetWebRequest(Uri Uri)
		{
			HttpWebRequest HttpWebRequest = (HttpWebRequest)base.GetWebRequest(Uri);
			HttpWebRequest.CookieContainer = CookieContainer;
			if (Timeout.HasValue) { HttpWebRequest.Timeout = Timeout.Value; }
			return HttpWebRequest;
		}
		internal void SetTimeout(int Timeout) => this.Timeout = Timeout;
		internal byte[] GetPostResponse(string Url, NameValueCollection Parameters)
		{
			try
			{
				byte[] Response_Bytes  = UploadValues(Url, "POST", Parameters );
				return Response_Bytes;
			}
			catch (Exception) { throw; }
		}
	}
}
