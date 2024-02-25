using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission formpart request
	/// </summary>
	public class ScriptReqeust
	{
		/// <summary>
		/// Ridder permission action request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="name">Name</param>
		public ScriptReqeust(RidderDesignerScope scope, string name)
		{
			Scope = scope;
			Name = name;
		}

		/// <summary>
		/// Name
		/// </summary>
		[JsonProperty("name")]
		[JsonRequired]
		public string Name { get; set; }

		/// <summary>
		/// Designer scope
		/// </summary>
		[JsonProperty("scope")]
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonRequired]
		public RidderDesignerScope Scope { get; set; }
	}
}