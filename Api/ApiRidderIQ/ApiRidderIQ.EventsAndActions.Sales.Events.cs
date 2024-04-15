﻿using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKParameters;
using Ridder.Common;
using RidderIQAPI.Models.RidderIQ;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.ServiceModel;

namespace RidderIQAPI.Api.ApiRidderIQ
{
	internal static partial class ApiRidderIQ
	{
		internal static partial class EventsAndActions
		{
			internal static partial class SalesEvents
			{
				internal static RidderIQSDKResult CreateShippingOrderFromOrderFromSelectedDetails(
					Collection<CookieHeaderValue> cookies,
					RidderIQCreateShippingOrder apiShippingOrder
				)
				{
					// Get the SDK Client
					RidderIQSDK sdk = GetClient(cookies, true);

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
						return new RidderIQSDKResult(ex);
					}
					catch
					{
						throw;
					}
				}
			}
		}
	}
}