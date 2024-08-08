using Ridder.Common.ADO;
using System.Collections.Generic;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ Records
	/// </summary>
	public class RidderIQRecords
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="q">Query parameters</param>
		public RidderIQRecords(QueryParameters q)
		{
			Columns = q.Columns;
			Filter = q.Filter;
			Sort = q.Sort;
		}

		/// <summary>
		/// Query Columns
		/// </summary>
		public string Columns { get; set; }

		/// <summary>
		/// Records from the database
		/// </summary>
		public List<Dictionary<string, object>> Data { get; set; } = new List<Dictionary<string, object>>();

		/// <summary>
		/// Query Filter
		/// </summary>
		public string Filter { get; set; }

		/// <summary>
		/// Query Sort
		/// </summary>
		public string Sort { get; set; }

		/// <summary>
		/// Record count
		/// </summary>
		public int RecordCount { get; set; }

		/// <summary>
		/// Has more: a next page is available given the recordcount
		/// </summary>
		public bool HasMore { get; set; }

		/// <summary>
		/// Current Page
		/// </summary>
		public int Page { get; set; }
	}
}