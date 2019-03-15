extern alias JsonSchemaDLL;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using JsonSchemaDLL::Newtonsoft.Json.Schema;

namespace Detectors.Tools.Strings
{
	internal static class StringExtensions
	{
		internal static string AsString(this TimeSpan Time)
		{
			string Result = Time.Days                                        > 0 ? $"{Time.Days        } Days"         : string.Empty;
			Result        = string.IsNullOrEmpty(Result) ? Time.Hours        > 0 ? $"{Time.Hours       } Hours"        : string.Empty : Time.Hours   > 0 ? $"{Result}, {Time.Hours  } Hours"   : string.Empty;
			Result        = string.IsNullOrEmpty(Result) ? Time.Minutes      > 0 ? $"{Time.Minutes     } Minutes"      : string.Empty : Time.Minutes > 0 ? $"{Result}, {Time.Minutes} Minutes" : string.Empty;
			Result        = string.IsNullOrEmpty(Result) ? Time.Seconds      > 0 ? $"{Time.Seconds     } Seconds"      : string.Empty : Time.Seconds > 0 ? $"{Result}, {Time.Seconds} Seconds" : string.Empty;
			Result        = string.IsNullOrEmpty(Result) ? Time.Milliseconds > 0 ? $"{Time.Milliseconds} Milliseconds" : string.Empty : Result;
			return Result;
		}

		private static string Capitalize(this string Text)
		{
			string Result = Regex.Replace(Text, @"\b(\w)", _ => _.Value.ToUpper());
			return Regex.Replace(Result, @"(\s(of|in|by|and)|\'[st])\b", _ => _.Value.ToLower(), RegexOptions.IgnoreCase);
		}

		internal static string ClearSpaces   (this string Text, bool ConvertUnderScoreToSpace = false) => (ConvertUnderScoreToSpace) ? Text.Replace("_", " ").Replace(" ", "") : Text.Replace(" ", "");

		internal static string Capitalize    (this string Text, bool ClearSpaces = false) => (ClearSpaces) ? Text.Capitalize().ClearSpaces() : Text.Capitalize();
	
		internal static string Capitalize(this string Text, bool ClearSpaces = false, bool ConvertUnderScoreToSpace = false) => ((ConvertUnderScoreToSpace) ? Text.Replace("_", " ") : Text).Capitalize();
	
		internal static string ToString  (this ValidationError ValidationError)
		{
			string ChildMessage = (ValidationError.ChildErrors.Count > 0) ? ValidationError.ChildErrors[0].Message.Replace("\n", "").Replace("\r", "").Replace("  ", " ").Trim() : string.Empty;
			string ChildSchema  = (ValidationError.ChildErrors.Count > 0) ? ValidationError.ChildErrors[0].Schema.ToString().Replace("\n", "").Replace("\r", "").Replace("  ", " ").Trim() : string.Empty;
			ChildSchema         = (ChildSchema != string.Empty) ? ChildSchema.Substring(2, ChildSchema.Length - 3).Trim().Replace("  ", " ") : ChildSchema;
			string MainMessage  = (ValidationError.ChildErrors.Count > 0) ? "" : ValidationError.Message.Replace("\n", "").Replace("\r", "").Replace("  ", " ").Trim();
			return $"\t@Line {ValidationError.LineNumber,4} [{ValidationError.Path}]: {MainMessage}{ChildMessage}{ChildSchema}";
		}
	
		internal static int Contain(this List<string> List, string ItemToFind)
		{
			int ListLength = List.Count;
			for (int ItemIndex = 0; ItemIndex < ListLength; ItemIndex++) { if (List[ItemIndex].Equals(ItemToFind)) { return ItemIndex; } }
			return -1;
		}

	}
}

