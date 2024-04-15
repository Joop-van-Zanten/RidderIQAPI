using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ Designer Scope
	/// </summary>
	[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public enum RidderIQDesignerScope
	{
		/// <summary>
		/// Custom
		/// </summary>
		Custom = 3,

		/// <summary>
		/// User
		/// </summary>
		User = 2,

		/// <summary>
		/// System
		/// </summary>
		System = 1
	}
}