using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission report result
	/// </summary>
	public class RidderIQPermissionReportResult : RidderIQPermissionReportReqeust
	{
		/// <summary>
		/// RidderIQ permission report result constructor
		/// </summary>
		/// <param name="request">Request parameters</param>
		/// <param name="result">Result</param>
		public RidderIQPermissionReportResult(RidderIQPermissionReportReqeust request, bool result) : base(request.Scope, request.ReportID) => Result = result;

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}