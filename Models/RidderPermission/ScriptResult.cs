using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission formpart result
	/// </summary>
	public class ScriptResult : ScriptReqeust
	{
		/// <summary>
		/// Ridder permission formpart result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public ScriptResult(ScriptReqeust request, bool result) : base(request.Scope, request.Name) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}