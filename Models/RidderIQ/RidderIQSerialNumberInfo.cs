﻿using Ridder.Client.SDK.SDKParameters;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// SerialNumberInfo API Model
	/// </summary>
	public class RidderIQSerialNumberInfo
	{
		/// <summary>
		/// Serial number ID
		/// </summary>
		public int SerialNumberID { get; set; }

		/// <summary>
		/// Number
		/// </summary>
		public string Number { get; set; }

		internal SerialNumberInfo ConvertToSDK() => new SerialNumberInfo
		{
			SerialNumberId = SerialNumberID,
			Number = Number
		};
	}
}