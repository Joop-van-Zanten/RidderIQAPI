using Newtonsoft.Json;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission table result
	/// </summary>
	public class RidderIQPermissionTableResult
	{
		/// <summary>
		/// RidderIQ permission table result constructor
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <param name="insert">Insert permissions</param>
		/// <param name="read">Read permissions</param>
		/// <param name="write">Write permissions</param>
		/// <param name="delete">Delete permissions</param>
		public RidderIQPermissionTableResult(string tableName, bool insert, bool read, bool write, bool delete)
		{
			TableName = tableName;
			Insert = insert;
			Read = read;
			Write = write;
			Delete = delete;
		}

		/// <summary>
		/// Insert permission
		/// </summary>
		[JsonProperty("insert")]
		public bool Insert { get; set; }

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

		/// <summary>
		/// Delete permission
		/// </summary>
		[JsonProperty("delete")]
		public bool Delete { get; set; }

		/// <summary>
		/// Table name
		/// </summary>
		[JsonProperty("tableName")]
		public string TableName { get; set; }
	}
}