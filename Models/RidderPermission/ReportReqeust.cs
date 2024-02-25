using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission formpart request
	/// </summary>
	public class ReportReqeust
	{
		/// <summary>
		/// Ridder permission action request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="reportID">Report ID</param>
		public ReportReqeust(RidderDesignerScope scope, Guid reportID)
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
		public RidderDesignerScope Scope { get; set; }
	}
}