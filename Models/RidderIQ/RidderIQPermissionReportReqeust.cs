using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission report request
	/// </summary>
	public class RidderIQPermissionReportReqeust
	{
		/// <summary>
		/// RidderIQ permission report request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="reportID">Report ID</param>
		public RidderIQPermissionReportReqeust(RidderIQDesignerScope scope, Guid reportID)
		{
			Scope = scope;
			ReportID = reportID;
		}

		/// <summary>
		/// Report
		/// </summary>
		[JsonProperty("reportID")]
		[JsonRequired]
		public Guid ReportID { get; set; }

		/// <summary>
		/// Designer scope
		/// </summary>
		[JsonProperty("scope")]
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonRequired]
		public RidderIQDesignerScope Scope { get; set; }
	}
}