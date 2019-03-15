using Newtonsoft.Json;

namespace Detectors.Data
{
	internal class FolderSettings
	{
		[JsonProperty("Scenario", NullValueHandling = NullValueHandling.Ignore)] internal string Scenario { get; set; }
		[JsonProperty("Input"   , NullValueHandling = NullValueHandling.Ignore)] internal string Input    { get; set; } = "Input";
		[JsonProperty("Output"  , NullValueHandling = NullValueHandling.Ignore)] internal string Output   { get; set; } = "Output";
	}
}
