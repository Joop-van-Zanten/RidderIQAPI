using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission action request
	/// </summary>
	public class ActionReqeust
	{
		/// <summary>
		/// Ridder permission action request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="actionName">Action name</param>
		public ActionReqeust(RidderDesignerScope scope, string actionName)
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
		public RidderDesignerScope Scope { get; set; }
	}
}