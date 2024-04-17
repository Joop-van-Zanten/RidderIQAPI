using Ridder.Client.SDK;
using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace RidderIQAPI.Api
{
	/// <summary>
	/// Ridder IQ API handler
	/// </summary>
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Ridder StockActions handler
		/// </summary>
		internal static class StockActions
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
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Find the Lot and Item
				var lot = sdk.CreateRecordset("R_LOT", "PK_R_LOT, FK_ITEM", $"PK_R_LOT = '{lotID}'", null);
				if (lot == null || lot.RecordCount == 0)
					throw new Exception($"Lot with ID '{lotID}' could not be found");
				lot.MoveFirst();
				// Return the hysical Stock For Lot Item
				return Math.Round(sdk.EventsAndActions.Stock.Actions.GetPhysicalStockForLotItem(
					lot.GetField<int>("FK_ITEM"),
					warehouseId,
					lot.GetField<int>("PK_R_LOT")
				), Core.IQ_DoublePrecision);
			}

			/// <summary>
			/// EventsAndActions.Stock.Actions.GetPhysicalStockForLotItem
			/// </summary>
			/// <param name="cookies">Cookies for getting the SDK Session</param>
			/// <param name="itemID">Item ID</param>
			/// <param name="warehouseId">Warehouse ID</param>
			/// <returns></returns>
			/// <exception cref="Exception"></exception>
			public static double GetPhysicalStockForItem(Collection<CookieHeaderValue> cookies, int itemID, int warehouseId)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Return the hysical Stock For Item
				return Math.Round(sdk.EventsAndActions.Stock.Actions.GetPhysicalStockForItem(
					itemID,
					warehouseId
				), Core.IQ_DoublePrecision);
			}
		}
	}
}