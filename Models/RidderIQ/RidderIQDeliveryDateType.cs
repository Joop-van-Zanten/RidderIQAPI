using System.Runtime.Serialization;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ Delivery date type
	/// </summary>
	public enum RidderIQDeliveryDateType
	{
		/// <summary>
		/// Use date from offer detail
		/// </summary>
		[EnumMember(Value = "UseDateFromOfferDetail")]
		UseDateFromOfferDetail = 1,

		/// <summary>
		/// Calculate date with delivery period
		/// </summary>
		[EnumMember(Value = "CalculateDateWithDeliveryPeriod")]
		CalculateDateWithDeliveryPeriod = 2,

		/// <summary>
		/// UseGgiven date
		/// </summary>
		[EnumMember(Value = "UseGivenDate")]
		UseGivenDate = 3
	}
}