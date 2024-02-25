using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission column result
	/// </summary>
	public class ColumnResult : ColumnRequest
	{
		/// <summary>
		/// Ridder permission column result constructor
		/// </summary>
		/// <param name="request">Column request</param>
		/// <param name="read">Read permissions</param>
		/// <param name="write">Write permissions</param>
		public ColumnResult(ColumnRequest request, bool read, bool write) : base(request.Table, request.Column)
		{
			Read = read;
			Write = write;
		}

		/// <summary>
		/// Read permission
		/// </summary>
		[JsonProperty("read")]
		public bool Read { get; set; }

		/// <summary>
		/// Write permission
		/// </summary>
		[JsonProperty("write")]
		public bool Write { get; set; }
	}
}