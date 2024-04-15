using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission action result
	/// </summary>
	public class RidderIQPermissionActionResult : RidderIQPermissionActionReqeust
	{
		/// <summary>
		/// RidderIQ permission action result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public RidderIQPermissionActionResult(RidderIQPermissionActionReqeust request, bool result) : base(request.Scope, request.ActionName) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}