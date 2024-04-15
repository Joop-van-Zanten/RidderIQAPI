using System.Collections.Generic;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ login Company
	/// </summary>
	public class RidderIQLoginCompany
	{
		/// <summary>
		/// SQL Database
		/// </summary>
		public string DatabaseName { get; set; }

		/// <summary>
		/// Company number index
		/// </summary>
		public int CompanyNumber { get; set; }

		/// <summary>
		/// Company name
		/// </summary>
		public string CompanyName { get; set; }

		/// <summary>
		/// Company aliases
		/// </summary>
		public List<string> CompanyAliases { get; set; }

		/// <summary>
		/// Database version
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// List of available users
		/// </summary>
		public List<string> Users { get; set; }
	}
}