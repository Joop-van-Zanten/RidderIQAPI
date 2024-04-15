using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKParameters;
using Ridder.Common;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.ServiceModel;

namespace RidderIQAPI.Api.ApiRidderIQ
{
	internal static partial class ApiRidderIQ
	{
		internal static partial class EventsAndActions
		{
			/// <summary>
			/// EventsAndActions.Stock.Actions.GetPhysicalStockForLotItem
			/// </summary>
			/// <param name="cookies">Cookies for getting the SDK Session</param>
			/// <param name="lotID">Lot ID</param>
			/// <param name="warehouseId">Warehouse ID</param>
			/// <returns></returns>
			/// <exception cref="Exception"></exception>
			public static double GetPhysicalStockForLotItem(Collection<CookieHeaderValue> cookies, int lotID, int warehouseId)
			{
				// Get the SDK Client
				RidderIQSDK sdk = GetClient(cookies, true);
				// Find the Lot and Item
				var lot = sdk.CreateRecordset("R_LOT", "PK_R_LOT, FK_ITEM", $"PK_R_LOT = '{lotID}'", null);
				if (lot == null || lot.RecordCount == 0)
					throw new Exception($"Lot with ID '{lotID}' could not be found");
				lot.MoveFirst();
				// Return the hysical Stock For Lot Item, rounded at 8
				return Math.Round(sdk.EventsAndActions.Stock.Actions.GetPhysicalStockForLotItem(
					lot.GetField<int>("FK_ITEM"),
					warehouseId,
					lot.GetField<int>("PK_R_LOT")
				), 8);
			}
		}
	}
}