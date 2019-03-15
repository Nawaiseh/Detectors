extern alias JsonSchemaDLL;

using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using JSchema          = JsonSchemaDLL::Newtonsoft.Json.Schema.JSchema;
using SchemaExtensions = JsonSchemaDLL::Newtonsoft.Json.Schema.SchemaExtensions;
using ValidationError  = JsonSchemaDLL::Newtonsoft.Json.Schema.ValidationError;

namespace Detectors.Tools.Json
{
	internal static class SchemaController
	{
		internal static ValidateResponse Validate(ValidateRequest ValidateRequest)
		{
			ValidateResponse ValidateResponse = new ValidateResponse { IsValid = true, Errors = new List<ValidationError>() };
			if (string.IsNullOrEmpty(ValidateRequest.Schema)) { return ValidateResponse; }
			JSchema JSchema                   = JSchema.Parse(ValidateRequest.Schema);
			JToken JToken                     = JToken.Parse(ValidateRequest.Json);
			bool IsValid                      = SchemaExtensions.IsValid(JToken, JSchema, out IList<ValidationError> Errors);
			ValidateResponse.IsValid          = IsValid;
			ValidateResponse.Errors           = Errors;

			return new ValidateResponse { IsValid = IsValid, Errors = Errors };
		}
	}
}
