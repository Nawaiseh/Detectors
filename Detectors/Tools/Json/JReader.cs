using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Detectors.Data;
using Detectors.Tools.Strings;

using Newtonsoft.Json;

namespace Detectors.Tools.Json
{
	internal class JReader
	{
		private static ValidateResponse ValidateJSON(string JSONString, string Schema, bool IsSchemaFileName, bool IgnoreCase, bool IgonreSpace)
		{
			Schema                            = (IsSchemaFileName) ? File.ReadAllText(Schema).Replace("additionalproperties", "additionalProperties") : Schema;
			Schema                            = IgonreSpace ? Schema.Replace(" ", "") : Schema;
			Schema                            = IgnoreCase ? Schema.ToLower() : Schema;
			ValidateRequest ValidateRequest   = new ValidateRequest { Json = JSONString, Schema = Schema };
			ValidateResponse ValidateResponse = SchemaController.Validate(ValidateRequest);
			return ValidateResponse;
		}
		internal static void Save<T>(T Item, string FileName) where T : class
		{
			string JSON = JsonConvert.SerializeObject(Item, Formatting.Indented);
			File.WriteAllText(FileName, JSON);
		}
		private static string LoadFile(string FileName) => File.ReadAllText(FileName);
		protected static T Load<T>(string FileName, string Schema = null, bool IsSchemaFileName = true, bool IgnoreCase = true, bool IgonreSpace = true) where T : class
		{
			string JSONString = LoadFile(FileName);
			JSONString = IgonreSpace ? JSONString.Replace(" ", "") : JSONString;
			JSONString = IgnoreCase ? JSONString.ToLower() : JSONString;
			JSONString = JSONString.Trim();
			using (ValidateResponse ValidateResponse = ValidateJSON(JSONString, Schema, IsSchemaFileName, IgnoreCase, IgonreSpace))
			{
				if (ValidateResponse.IsValid)
				{
					JsonConvert.DefaultSettings = () => new JsonSerializerSettings
					{
						DateParseHandling = DateParseHandling.DateTime,
						StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
						DefaultValueHandling = DefaultValueHandling.Ignore,
						NullValueHandling = NullValueHandling.Ignore

					};
					T Result = JsonConvert.DeserializeObject<T>(JSONString);
					return Result;
				}
				// TODO Display Error Message and Prevent Simulation From Starting
				if (ValidateResponse.Errors.Count <= 0) { return null; /*return (Instance = null);*/ }
				List<string> Errors = new List<string> { $"\"{FileName}\" Failed To Validate Against Requirements.", "Below is the List of Validation Results\n[" };
				Errors.AddRange(ValidateResponse.Errors.Select(StringExtensions.ToString).ToList());
				Errors.Add("]");
				string ErrorsFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Scenario.DefaultErrorsFileName ?? "Errors.Log");
				File.AppendAllLines(ErrorsFileName, Errors);
				return null;
			}
		}
	}
}
