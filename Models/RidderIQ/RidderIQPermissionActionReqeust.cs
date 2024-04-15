using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission action request
	/// </summary>
	public class RidderIQPermissionActionReqeust
	{
		/// <summary>
		/// RidderIQ permission action request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="actionName">Action name</param>
		public RidderIQPermissionActionReqeust(RidderIQDesignerScope scope, string actionName)
		{
			Scope = scope;
			ActionName = actionName;
		}

		/// <summary>
		/// Action name
		/// </summary>
		[JsonProperty("actionName")]
		[JsonRequired]
		public string ActionName { get; set; }

		/// <summary>
		/// Designer scope
		/// </summary>
		[JsonProperty("scope")]
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonRequired]
		public RidderIQDesignerScope Scope { get; set; }
	}
}