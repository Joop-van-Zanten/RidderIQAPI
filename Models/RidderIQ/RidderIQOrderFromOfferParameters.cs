using Ridder.Client.SDK.SDKParameters;
using System;
using System.ComponentModel.DataAnnotations;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQOrderFromOfferParameters API model
	/// </summary>
	public class RidderIQOrderFromOfferParameters
	{
		/// <summary>
		/// Existing Order ID
		/// </summary>
		public int ExistingOrderId { get; set; }

		/// <summary>
		/// Details items
		/// </summary>
		[Required]
		public OfferDetail[] Details { get; set; }

		/// <summary>
		/// Type of delivery Date
		/// </summary>
		[Required]
		public RidderIQDeliveryDateType DeliveryDateType { get; set; }

		/// <summary>
		/// DeliveryDate
		/// </summary>
		public DateTime DeliveryDate { get; set; }

		/// <summary>
		/// Rental Contract ID
		/// </summary>
		public int RentalContractId { get; set; }

		/// <summary>
		/// Make main order
		/// </summary>
		public bool MakeMainOrder { get; set; }
	}
}