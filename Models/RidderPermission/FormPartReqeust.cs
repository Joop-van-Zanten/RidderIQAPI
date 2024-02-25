using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission formpart request
	/// </summary>
	public class FormPartReqeust
	{
		/// <summary>
		/// Ridder permission action request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="formpartName">Formpart name</param>
		public FormPartReqeust(RidderDesignerScope scope, string formpartName)
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
		public RidderDesignerScope Scope { get; set; }
	}
}