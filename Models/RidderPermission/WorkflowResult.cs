using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission workflow result
	/// </summary>
	public class WorkflowResult : WorkflowReqeust
	{
		/// <summary>
		/// Ridder permission workflow result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public WorkflowResult(WorkflowReqeust request, bool result) : base(request.Scope, request.WorkflowEvent) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}