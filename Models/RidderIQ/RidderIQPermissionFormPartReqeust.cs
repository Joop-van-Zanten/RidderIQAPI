using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission formpart request
	/// </summary>
	public class RidderIQPermissionFormPartReqeust
	{
		/// <summary>
		/// RidderIQ permission formpart request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="formpartName">Formpart name</param>
		public RidderIQPermissionFormPartReqeust(RidderIQDesignerScope scope, string formpartName)
		{
			Scope = scope;
			FormpartName = formpartName;
		}

		/// <summary>
		/// Form part name
		/// </summary>
		[JsonProperty("formpartName")]
		[JsonRequired]
		public string FormpartName { get; set; }

		/// <summary>
		/// Designer scope
		/// </summary>
		[JsonProperty("scope")]
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonRequired]
		public RidderIQDesignerScope Scope { get; set; }
	}
}