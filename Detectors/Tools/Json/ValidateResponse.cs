extern alias JsonSchemaDLL;

using System;
using System.Collections.Generic;

using ValidationError = JsonSchemaDLL::Newtonsoft.Json.Schema.ValidationError;

namespace Detectors.Tools.Json
{
	internal class ValidateResponse : IDisposable
	{
		internal bool IsValid                  { get; set; }
		internal IList<ValidationError> Errors { get; set; }

		public void Dispose() => Errors = null;
	}
}
