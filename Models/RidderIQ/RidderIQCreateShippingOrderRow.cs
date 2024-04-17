using Ridder.Client.SDK.SDKParameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// SDKCreateShippingOrderRow API model
	/// </summary>
	public class RidderIQCreateShippingOrderRow
	{
		/// <summary>
		/// Serials numbers
		/// </summary>
		public List<RidderIQSerialNumberInfo> SerialNumbers { get; set; }

		/// <summary>
		/// PK (All detail)
		/// </summary>
		[Required]
		public Guid ID { get; set; }

		/// <summary>
		/// Quantity
		/// </summary>
		[Required]
		public double Quantity { get; set; }

		/// <summary>
		/// Within delivery time (If there is an workactivity rule)
		/// </summary>
		public TimeSpan TimeSpan { get; set; }

		/// <summary>
		/// Quantity in backorder
		/// </summary>
		public double BackOrderQuantity { get; set; }

		/// <summary>
		/// Within backorde rdelivery time (If there is an workactivity rule)
		/// </summary>
		public TimeSpan BackOrderTimeSpan { get; set; }

		/// <summary>
		/// Is the delivery completed
		/// </summary>
		internal bool DeliveryComplete { get; set; }

		/// <summary>
		/// Delivery date of the backorder
		/// </summary>
		public DateTime BackOrderDate { get; set; }

		/// <summary>
		/// Remaining quantity to deliver
		/// </summary>
		//[Required]
		internal double RemainingQuantity { get; set; }

		/// <summary>
		/// Convert the API model back to SDK model
		/// </summary>
		/// <returns></returns>
		internal SDKCreateShippingOrderRow ConvertToSDK()
		{
			SDKCreateShippingOrderRow result = new SDKCreateShippingOrderRow
			{
				Id = ID,
				Quantity = Quantity,
				TimeSpan = TimeSpan,
				BackOrderQuantity = BackOrderQuantity,
				DeliveryComplete = DeliveryComplete,
				BackOrderDate = BackOrderDate,
				RemainingQuantity = RemainingQuantity,
				BackOrderTimeSpan = BackOrderTimeSpan
			};

			SerialNumbers?.ForEach(x => result.AddSerialNumber(x.ConvertToSDK()));

			return result;
		}
	}
}