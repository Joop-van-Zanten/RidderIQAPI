using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission formpart result
	/// </summary>
	public class RidderIQPermissionFormPartResult : RidderIQPermissionFormPartReqeust
	{
		/// <summary>
		/// RidderIQ permission formpart result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public RidderIQPermissionFormPartResult(RidderIQPermissionFormPartReqeust request, bool result) : base(request.Scope, request.FormpartName) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}