using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKParameters;
using Ridder.Common;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceModel;
using WebGrease.Css.Extensions;

namespace RidderIQAPI.Api
{
	/// <summary>
	/// Ridder IQ API handler
	/// </summary>
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Ridder SalesEvents handler
		/// </summary>
		internal static class SalesEvents
		{
			internal static RidderIQSDKResult CreateShippingOrderFromOrderFromSelectedDetails(
				Collection<CookieHeaderValue> cookies,
				RidderIQCreateShippingOrder apiShippingOrder
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

				// Inject fields according to the System
				sdk.CreateRecordset(
					"R_SALESORDERALLDETAIL",
					null,
					$"PK_R_SALESORDERALLDETAIL IN ('{string.Join("','", apiShippingOrder.Rows.Select(x => x.ID))}')",
					null
				).AsEnumerable().ForEach(record =>
				{
					Guid saleOrderId = record.GetField<Guid>("PK_R_SALESORDERALLDETAIL");
					double RemainingQuantity = record.GetField<double>("REMAININGQUANTITYTODELIVER");

					if (RemainingQuantity <= 0)
						throw new Exception($"SalesOrderDetail '{saleOrderId}' delivery is complete");

					RidderIQCreateShippingOrderRow line = apiShippingOrder.Rows.FirstOrDefault(x => x.ID == saleOrderId);

					line.RemainingQuantity = RemainingQuantity;
					line.DeliveryComplete = line.Quantity >= line.RemainingQuantity;
				});

				CreateShippingOrderParameter SdkShippingOrder = new CreateShippingOrderParameter();
				apiShippingOrder.Rows.ForEach(x => SdkShippingOrder.AddRow(x.ConvertToSDK()));

				ISDKResult sdkResult = sdk.EventsAndActions.Sales.Events.CreateShippingOrderFromOrderFromSelectedDetails(
					apiShippingOrder.OrderID,
					SdkShippingOrder
				);

				try
				{
					return new RidderIQSDKResult(sdkResult);
				}
				catch (FaultException<TranslationMessageInfo> ex)
				{
					return new RidderIQSDKResult(ex, null);
				}
				catch
				{
					throw;
				}
			}
		}
	}
}