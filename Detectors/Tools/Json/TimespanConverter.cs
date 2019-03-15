using System;
using Newtonsoft.Json;

namespace Detectors.Tools.Json
{
	internal class TimeSpanConverter : JsonConverter<TimeSpan>
	{
		public const string TIME_SPAN_FORMAT_STRING = @"hh\:mm\ tt";

		public override void WriteJson(JsonWriter JsonWriter, TimeSpan TimeSpan, JsonSerializer _)
		{
			string timespanFormatted = $"{TimeSpan.ToString(TIME_SPAN_FORMAT_STRING)}";
			JsonWriter.WriteValue(timespanFormatted);
		}

		public override TimeSpan ReadJson(JsonReader JsonReader, Type _, TimeSpan __, bool ___, JsonSerializer ____)
		{
			TimeSpan.TryParseExact((string)JsonReader.Value, TIME_SPAN_FORMAT_STRING, null, out TimeSpan ParsedTimeSpan);
			return ParsedTimeSpan;
		}
	}

}
