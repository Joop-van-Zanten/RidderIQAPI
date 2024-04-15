using System.Collections.Generic;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQCreateShippingOrder API model
	/// </summary>
	public class RidderIQCreateShippingOrder
	{
		/// <summary>
		/// Order ID
		/// </summary>
		public int OrderID { get; set; }

		/// <summary>
		/// Shippingorder rows
		/// </summary>
		public List<RidderIQCreateShippingOrderRow> Rows { get; set; }
	}
}