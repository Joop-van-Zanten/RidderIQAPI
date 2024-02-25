using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission column request
	/// </summary>
	public class ColumnRequest
	{
		/// <summary>
		/// Ridder permission table request constructor
		/// </summary>
		/// <param name="table"></param>
		/// <param name="column"></param>
		public ColumnRequest(string table, string column)
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