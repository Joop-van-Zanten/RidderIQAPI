using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission workflow result
	/// </summary>
	public class RidderIQPermissionWorkflowResult : RidderIQPermissionWorkflowReqeust
	{
		/// <summary>
		/// RidderIQ permission workflow result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public RidderIQPermissionWorkflowResult(RidderIQPermissionWorkflowReqeust request, bool result) : base(request.Scope, request.WorkflowEvent) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}