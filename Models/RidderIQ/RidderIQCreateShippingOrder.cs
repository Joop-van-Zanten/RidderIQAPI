using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
		[Required]
		public int OrderID { get; set; }

		/// <summary>
		/// Shippingorder rows
		/// </summary>
		[Required]
		public List<RidderIQCreateShippingOrderRow> Rows { get; set; }
	}
}