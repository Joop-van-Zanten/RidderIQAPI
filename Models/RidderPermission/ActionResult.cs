using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission action result
	/// </summary>
	public class ActionResult : ActionReqeust
	{
		/// <summary>
		/// Ridder permission action result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public ActionResult(ActionReqeust request, bool result) : base(request.Scope, request.ActionName) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}