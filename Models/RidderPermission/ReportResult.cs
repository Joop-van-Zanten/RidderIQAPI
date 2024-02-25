using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission report result
	/// </summary>
	public class ReportResult : ReportReqeust
	{
		/// <summary>
		/// Ridder permission report result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public ReportResult(ReportReqeust request, bool result) : base(request.Scope, request.ReportID) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}