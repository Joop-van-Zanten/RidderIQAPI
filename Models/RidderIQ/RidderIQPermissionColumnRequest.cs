using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission column request
	/// </summary>
	public class RidderIQPermissionColumnRequest
	{
		/// <summary>
		/// RidderIQ permission table request constructor
		/// </summary>
		/// <param name="table"></param>
		/// <param name="column"></param>
		public RidderIQPermissionColumnRequest(string table, string column)
		{
			Table = table;
			Column = column;
		}

		/// <summary>
		/// Table name
		/// </summary>
		[JsonProperty("table")]
		[JsonRequired]
		public string Table { get; set; }

		/// <summary>
		/// Table name
		/// </summary>
		[JsonProperty("column")]
		[JsonRequired]
		public string Column { get; set; }
	}
}