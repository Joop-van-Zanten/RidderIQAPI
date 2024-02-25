using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKDataAcces;
using Ridder.Common.ADO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceModel;

namespace RidderIQAPI.Api
{
	internal static partial class ApiRidderIQ
	{
		public static RidderSDKResult RecordsCreate(
			Collection<CookieHeaderValue> cookies,
			string table,
			string columns,
			Dictionary<string, object> fields,
			bool UseDataChanges = true
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Create new empty record
			var record = sdk.CreateRecordset(
				table,
				string.Join(",", fields.Keys),
				$"PK_{table} = NULL",
				null
			);
			// Move the pointer to the start
			record.MoveFirst();
			// Mark as new record
			record.AddNew();
			// Use datachanges
			record.UseDataChanges = UseDataChanges;
			// Loop through fields
			foreach (var field in fields)
				record.Fields[field.Key].Value = field.Value;

			try
			{
				return new RidderSDKResult(record.Update());
			}
			catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
			{
				return new RidderSDKResult(ex2);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static RidderSDKResult RecordsDelete(
			Collection<CookieHeaderValue> cookies,
			string table,
			string recordID
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Find the Record by its ID
			SDKRecordset records = sdk.CreateRecordset(
				table,
				$"PK_{table}",
				$"PK_{table} = '{recordID}'",
				null
			);
			// Check if any records are found
			if (records.RecordCount == 0 || records.RecordCount > 1)
				throw new KeyNotFoundException();

			try
			{
				records.MoveFirst();
				records.Delete();
				return new RidderSDKResult(records.Update());
			}
			catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
			{
				return new RidderSDKResult(ex2);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static RidderRecords RecordsGetList(
			Collection<CookieHeaderValue> cookies,
			string table,
			string columns,
			string filter,
			string sort,
			int page = 1,
			int pageSize = MaxPageSize
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Define the Query parameters
			QueryParameters qp = new QueryParameters(
				table,
				columns == null ? null : string.Join(",", columns.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x)),
				filter,
				sort == null ? null : string.Join(",", sort.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x)),
				page >= 1 ? page : 1,
				pageSize <= MaxPageSize ? pageSize : MaxPageSize
			);
			// Create recordset from the QueryParameters
			var records = sdk.CreateRecordset(qp);
			// Move the pointer to the start
			records.MoveFirst();
			// Create result
			RidderRecords result = new RidderRecords(qp)
			{
				Data = records.AsEnumerable().Select(x => x.ToDictionary()).ToList(),
				Columns = columns,
				Filter = filter,
				Sort = qp.Sort,
				RecordCount = records.RecordCount
			};
			// Return the result
			return result;
		}

		public static Dictionary<string, object> RecordsGetSingle(
			Collection<CookieHeaderValue> cookies,
			string table,
			string recordID,
			string columns
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Find the Record by its ID
			SDKRecordset records = sdk.CreateRecordset(
				table,
				columns,
				$"PK_{table} = '{recordID}'",
				null
			);
			// Check if any records are found
			if (records.RecordCount == 0)
				throw new KeyNotFoundException();
			// Move the pointer to the start
			records.MoveFirst();
			// Creat the result
			Dictionary<string, object> result = records.ToDictionary();
			// Return the result
			return result;
		}

		public static RidderSDKResult RecordsUpdate(
			Collection<CookieHeaderValue> cookies,
			string table,
			string recordID,
			Dictionary<string, object> data
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Get record by ID
			var record = sdk.CreateRecordset(
				table,
				null,
				$"PK_{table} = '{recordID}'",
				null
			);
			// Validate a record has been found
			if (record.RecordCount == 0)
				throw new KeyNotFoundException();

			// Move the pointer to the start
			record.MoveFirst();
			// Use datachanges
			record.UseDataChanges = true;
			// Loop through update fields
			foreach (KeyValuePair<string, object> field in data)
			{
				// Ignore PK if present
				if (field.Key.StartsWith("PK_"))
					continue;
				// Update the field
				record.Fields[field.Key].Value = field.Value;
			}

			try
			{
				// Update the record: Save
				return new RidderSDKResult(record.Update());
			}
			catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
			{
				return new RidderSDKResult(ex2);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}