using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission script request
	/// </summary>
	public class RidderIQPermissionScriptReqeust
	{
		/// <summary>
		/// RidderIQ permission script request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="name">Name</param>
		public RidderIQPermissionScriptReqeust(RidderIQDesignerScope scope, string name)
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
		public RidderIQDesignerScope Scope { get; set; }
	}
}