using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission script result
	/// </summary>
	public class RidderIQPermissionScriptResult : RidderIQPermissionScriptReqeust
	{
		/// <summary>
		/// RidderIQ permission script result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public RidderIQPermissionScriptResult(RidderIQPermissionScriptReqeust request, bool result) : base(request.Scope, request.Name) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}