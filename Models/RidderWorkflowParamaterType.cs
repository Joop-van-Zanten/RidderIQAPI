using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RidderIQAPI
{
	/// <summary>
	/// Workflow parameter type
	/// </summary>
	[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public enum RidderWorkflowParamaterType
	{
		/// <summary>
		/// String value
		/// </summary>
		[EnumMember(Value = "string")]
		String = 0,

		/// <summary>
		/// Integer value
		/// </summary>
		[EnumMember(Value = "int")]
		Int = 1,

		/// <summary>
		/// Double value
		/// </summary>
		[EnumMember(Value = "double")]
		Double = 2,

		/// <summary>
		/// Long value
		/// </summary>
		[EnumMember(Value = "long")]
		Long = 3,

		/// <summary>
		/// DateTime value
		/// </summary>
		[EnumMember(Value = "datetime")]
		DateTime = 4
	}
}