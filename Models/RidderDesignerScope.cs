using Newtonsoft.Json;

namespace RidderIQAPI.Models
{
	[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public enum RidderDesignerScope
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