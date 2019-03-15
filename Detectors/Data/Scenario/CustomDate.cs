using System;
using System.Globalization;
using Detectors.Tools.Json;
using Newtonsoft.Json;

namespace Detectors.Data
{
	internal class CustomDate
	{
		[JsonProperty("Date", NullValueHandling = NullValueHandling.Ignore)]
		//[JsonConverter(typeof(DateTimeConverter))]
		internal string DateAsString { get; set; }


		[JsonProperty("Time", NullValueHandling = NullValueHandling.Ignore)]
		//[JsonConverter(typeof(TimeSpanConverter))]
		internal string TimeAsString { get; set; }

		[JsonIgnore] internal DateTime Date { get; private set; }
		[JsonIgnore] internal TimeSpan Time { get; private set; }
		internal void Initialize()
		{
			Date = DateTime.ParseExact(DateAsString, @"yyyy\-mm\-dd"  , CultureInfo.InvariantCulture);
			Time = DateTime.ParseExact(TimeAsString, @"hh\:mm\:ss\ tt", CultureInfo.InvariantCulture).TimeOfDay;
			//DateTime.TryParse(DateAsString, out DateTime _Date); Date = _Date;
			//TimeSpan.TryParse(TimeAsString, out TimeSpan _Time); Time = _Time;
		}

	}
}
