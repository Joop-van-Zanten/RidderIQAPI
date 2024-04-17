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
		public int ExistingOrderId { get; set; }

		[Required]
		public OfferDetail[] Details { get; set; }

		[Required]
		public RidderIQDeliveryDateType DeliveryDateType { get; set; }

		public DateTime DeliveryDate { get; set; }

		public int RentalContractId { get; set; }

		public bool MakeMainOrder { get; set; }
	}
}