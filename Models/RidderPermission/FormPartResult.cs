using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission formpart result
	/// </summary>
	public class FormPartResult : FormPartReqeust
	{
		/// <summary>
		/// Ridder permission formpart result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public FormPartResult(FormPartReqeust request, bool result) : base(request.Scope, request.FormpartName) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}