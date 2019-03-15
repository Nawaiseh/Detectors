using System;
using Newtonsoft.Json;

namespace Detectors.Tools.Json
{
	internal class DateTimeConverter : JsonConverter<TimeSpan>
	{
		public const string DATETIME_FORMAT_STRING = @"yyyy\-mm\-dd";

		public override void WriteJson(JsonWriter JsonWriter, TimeSpan TimeSpan, JsonSerializer _)
		{
			string timespanFormatted = $"{TimeSpan.ToString(DATETIME_FORMAT_STRING)}";
			JsonWriter.WriteValue(timespanFormatted);
		}

		public override TimeSpan ReadJson(JsonReader JsonReader, Type _, TimeSpan __, bool ___, JsonSerializer ____)
		{
			TimeSpan.TryParseExact((string)JsonReader.Value, DATETIME_FORMAT_STRING, null, out TimeSpan ParsedTimeSpan);
			return ParsedTimeSpan;
		}
	}

}
