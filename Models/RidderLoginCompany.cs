using System.Collections.Generic;

namespace RidderIQAPI.Models
{
	public class RidderLoginCompany
	{
		public string DatabaseName { get; set; }

		public int CompanyNumber { get; set; }

		public string CompanyName { get; set; }

		public List<string> CompanyAliases { get; set; }

		public string Version { get; set; }

		public List<string> Users { get; set; }
	}
}