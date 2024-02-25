using ADODB;
using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKDataAcces;
using Ridder.Common.ADO;
using RidderIQAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Web;

namespace RidderIQAPI.Api
{
	/// <summary>
	/// Ridder API handler
	/// </summary>
	internal static partial class ApiRidderIQ
	{
		#region config

		/// <summary>
		/// Maximum page size
		/// </summary>
		public const int MaxPageSize = 200;

		#endregion config

		#region Sessions

		internal const string CookieIqToken = "RidderIQ_Token";

		private static RidderIQSDK RidderSDK = new RidderIQSDK();

		private static List<RidderCredentialToken> SdkClients = new List<RidderCredentialToken>();

		internal static RidderIQSDK GetClient(Collection<CookieHeaderValue> cookies, bool throwException = false)
		{
			try
			{
				RidderCredential cred = GetIqCredential(cookies);
				if (cred is null)
				{
					if (throwException)
						throw new UnauthorizedAccessException();
					return default;
				}
				RidderIQSDK result = GetClient(cred);
				if (result == default)
				{
					result = new RidderIQSDK();
					// Try to 're-use' existing Ridder SDK connection
					ISDKResult loginResult = result.ConnectToPersistedSession(cred.Username, cred.Password, cred.Company);

					// Check if the user is succesfully LoggedIn using Persisted
					if (loginResult != null && loginResult.HasError)
					{
						// Try to create new connection using the credentials
						loginResult = result.Login(cred.Username, cred.Password, cred.Company);
						// Check if connection succeeded
						if (loginResult != null && loginResult.HasError)
							// Login Failed
							throw new UnauthorizedAccessException();
						result.PersistSession();
					}
					if (loginResult.HasError)
						throw new UnauthorizedAccessException();

					Register(new RidderCredentialToken(cred, result));
				}
				return result;
			}
			catch (Exception ex)
			{
			}
			if (throwException)
				throw new UnauthorizedAccessException();
			return default;
		}

		internal static void Register(RidderCredentialToken token)
		{
			if (!SdkClients.Any(x => x.Equals(token)))
				SdkClients.Add(token);
		}

		internal static void UnRegister(Collection<CookieHeaderValue> cookies)
		{
			RidderCredential cred = GetIqCredential(cookies);
			if (cred is null)
				return;
			RidderCredentialToken item = SdkClients.FirstOrDefault(x => x.Person == cred);
			if (item != null)
			{
				item.Dispose();
				SdkClients.Remove(item);
			}
		}

		private static RidderIQSDK GetClient(RidderCredential cred) => SdkClients.FirstOrDefault(x => x.Person == cred)?.Sdk;

		private static RidderCredential GetIqCredential(Collection<CookieHeaderValue> cookies)
		{
			try
			{
				foreach (var cookieHeader in cookies)
				{
					if (cookieHeader.Cookies.Any(x => x.Name == CookieIqToken))
					{
						var cValue = cookieHeader.Cookies.First(x => x.Name == CookieIqToken)?.Value;

						if (!cValue.IsBase64String())
							cValue = HttpUtility.UrlDecode(cValue);

						return cValue?.OpenSSLDecrypt()?.Deserialize<RidderCredential>();
					}
				}

				return default;
			}
			catch (Exception)
			{
				return default;
			}
		}

		#endregion Sessions

		#region WorkFlows

		public static RidderSDKResult RecordsExecuteWorkflows(Collection<CookieHeaderValue> cookies, string table, string recordID, Guid workflowID, List<RidderWorkFlowEventParameter> jsonParameters = null)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);

			Dictionary<string, object> parameters = new Dictionary<string, object>();

			foreach (var jsonParameter in jsonParameters)
			{
				if (string.Equals(jsonParameter.Type, "int", StringComparison.InvariantCultureIgnoreCase))
					parameters.Add(jsonParameter.Key, Convert.ToInt32(jsonParameter.Value));
				else if (string.Equals(jsonParameter.Type, "double", StringComparison.InvariantCultureIgnoreCase))
					parameters.Add(jsonParameter.Key, Convert.ToDouble(jsonParameter.Value));
				else if (string.Equals(jsonParameter.Type, "long", StringComparison.InvariantCultureIgnoreCase))
					parameters.Add(jsonParameter.Key, Convert.ToInt64(jsonParameter.Value));
				else
					parameters.Add(jsonParameter.Key, jsonParameter.Value.ToString().Trim());
			}

			try
			{
				ISDKResult sdkResult = null;

				if (parameters == null || parameters.Count == 0)
					sdkResult = sdk.ExecuteWorkflowEvent(table, Convert.ToInt32(recordID), workflowID);
				else
					sdkResult = sdk.ExecuteWorkflowEvent(table, Convert.ToInt32(recordID), workflowID, parameters);
				return new RidderSDKResult(sdkResult);
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

		public static List<Dictionary<string, object>> RecordsGetWorkflows(
			Collection<CookieHeaderValue> cookies,
			string table,
			string recordID
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Find the table
			SDKRecordset records = sdk.CreateRecordset(
				"M_TABLEINFO",
				null,
				$"TABLENAME = '{table}' AND FK_WORKFLOWMODEL IS NOT NULL",
				null
			);
			// Check if any records are found
			if (records.RecordCount == 0)
				throw new KeyNotFoundException();
			// Move the pointer to the start
			records.MoveFirst();
			// Get the workflow model
			Guid workflowModel = (Guid)records.GetField("FK_WORKFLOWMODEL").Value;
			records = sdk.CreateRecordset(
				"M_WORKFLOWEVENT",
				null,
				$"FK_WORKFLOWMODEL = '{workflowModel}' AND (ACTIONNAME IS NULL OR ACTIONNAME = '') AND TYPE == 1",
				null
			);
			// Check if any records are found
			if (records.RecordCount == 0)
				throw new KeyNotFoundException();
			// Move the pointer to the start
			records.MoveFirst();
			List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

			object id = Guid.TryParse(recordID, out Guid guid) ? guid : (object)Convert.ToInt32(recordID);
			while (!records.EOF)
			{
				bool add = true;
				var userAvailabilityValue = records.GetField("USERAVAILABILITY").Value;

				if (userAvailabilityValue != DBNull.Value)
				{
					var userAvailability = (byte[])records.GetField("USERAVAILABILITY").Value;

					var calResult = sdk.GetCalculatedColumnOutput(userAvailability, table, id);
					if (calResult is bool b)
					{
						add = b;
					}
				}

				if (add)
				{
					result.Add(records.ToDictionary());
				}

				records.MoveNext();
			}

			return result;
		}

		public static List<Dictionary<string, object>> RecordsTableWorkflows(
			Collection<CookieHeaderValue> cookies,
			string table
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);

			// Find the table
			SDKRecordset records = sdk.CreateRecordset(
				"M_TABLEINFO",
				null,
				$"TABLENAME = '{table}' AND FK_WORKFLOWMODEL IS NOT NULL",
				null
			);
			// Check if any records are found
			if (records.RecordCount == 0)
				throw new KeyNotFoundException();
			// Move the pointer to the start
			records.MoveFirst();
			// Get the workflow model
			Guid workflowModel = (Guid)records.GetField("FK_WORKFLOWMODEL").Value;
			records = sdk.CreateRecordset(
				"M_WORKFLOWEVENT",
				null,
				$"FK_WORKFLOWMODEL = '{workflowModel}' AND (ACTIONNAME IS NULL OR ACTIONNAME = '') AND TYPE == 1",
				null
			);
			// Check if any records are found
			if (records.RecordCount == 0)
				throw new KeyNotFoundException();
			// Create result set
			List<Dictionary<string, object>> result = records.AsEnumerable().Select(x => x.ToDictionary()).ToList();
			// Return the result
			return result;
		}

		public static bool WorkflowEventAvailable(
			Collection<CookieHeaderValue> cookies,
			Guid workflowID,

			string recordID
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);

			// Find the workflow event
			SDKRecordset WorkflowEvent = sdk.CreateRecordset(
				"M_WORKFLOWEVENT",
				"FK_WORKFLOWMODEL,CUSTOMAVAILABILITY,AVAILABILITY,USERAVAILABILITY",
				$"PK_M_WORKFLOWEVENT = '{workflowID}'",
				null
			);
			WorkflowEvent.MoveFirst();

			// Check if any records are found
			if (WorkflowEvent.RecordCount == 0)
				throw new KeyNotFoundException();

			var workflowModelID = WorkflowEvent.GetField<Guid>("FK_WORKFLOWMODEL");

			// Find the Table which is linked to the workflow
			SDKRecordset TableInfo = sdk.CreateRecordset(
				"M_TABLEINFO",
				"PK_M_TABLEINFO",
				$"FK_WORKFLOWMODEL = '{workflowModelID}'",
				null
			);
			string tablename = TableInfo.GetField<string>("TABLENAME");
			string tableID = TableInfo.GetField<string>("TABLENAME");

			// Find the record
			SDKRecordset record = sdk.CreateRecordset(
				tablename,
				"FK_WORKFLOWSTATE",
				$"PK_{tablename} = '{recordID}'",
				null
			);
			record.MoveFirst();

			// Check if any records are found
			if (record.RecordCount == 0)
				throw new KeyNotFoundException();

			// Find the workflowevent available States
			var WorkflowEventStates = sdk.CreateRecordset(
				"M_WORKFLOWEVENTSTATES",
				"FK_STATE",
				$"FK_WORKFLOWEVENT = '{WorkflowEvent.GetField("PK_M_WORKFLOWEVENT").Value}'",
				null
			).AsEnumerable().Select(x => (Guid)x.GetField("FK_STATE").Value).ToList();
			if (WorkflowEventStates.Count > 0)
			{
				Guid recordWorkflowstate = (Guid)record.GetField("FK_WORKFLOWSTATE").Value;
				if (!WorkflowEventStates.Contains(recordWorkflowstate))
					return false;
			}
			// Loop through all Fields
			foreach (Field field in WorkflowEvent.Fields)
			{
				// Find availability fields
				if (!field.Name.EndsWith("AVAILABILITY", StringComparison.InvariantCultureIgnoreCase))
					continue;
				// Check for DBNull values
				var visibilityValue = WorkflowEvent.GetField(field.Name).Value;
				if (visibilityValue == DBNull.Value)
					continue;

				// Get the calculated column
				var availability = (byte[])WorkflowEvent.GetField(field.Name).Value;
				// Calculate the value for the table record
				object calResult = sdk.GetCalculatedColumnOutput(availability, tablename, recordID);
				if (calResult == null)
					return false;
				// If not visible
				if (calResult is bool b && b == false)
					return false;
			}
			return true;
		}

		#endregion WorkFlows

		#region RecordTag

		public static string RecordsGetRecordTag(Collection<CookieHeaderValue> cookies, string table, string recordID)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			return sdk.GetRecordTag(table, recordID);
		}

		#endregion RecordTag

		#region Documents

		public static RidderRecords RecordsGetDocuments(
			Collection<CookieHeaderValue> cookies,
			string table,
			string recordID,
			string columns,
			string filter,
			string sort
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);

			string pkfield = "FK_" + string.Join("_", table.Split('_').Skip(1));
			// Find the Record by its ID
			SDKRecordset records = sdk.CreateRecordset(
				$"{table}DOCUMENT",
				"FK_DOCUMENT",
				$"{pkfield} = '{recordID}'",
				null
			);
			// Check if any records are found
			if (records.RecordCount == 0)
				throw new KeyNotFoundException();
			// Move the pointer to the start
			records.MoveFirst();
			List<int> pks = records.AsEnumerable().Select(x => (int)x.GetField("FK_DOCUMENT").Value).ToList();
			string mainFilter = $"PK_R_DOCUMENT IN ({string.Join(", ", pks)})";
			// Define the Query parameters
			QueryParameters qp = new QueryParameters(
				"R_DOCUMENT",
				columns == null ? null : string.Join(",", columns.Split(',').Select(x => x.Trim()).OrderBy(x => x)),
				filter != null ? $"{mainFilter} AND {filter}" : mainFilter,
				sort == null ? null : string.Join(",", sort.Split(',').Select(x => x.Trim()).OrderBy(x => x))
			);
			// Execute the query
			records = sdk.CreateRecordset(qp);
			// Create result
			RidderRecords result = new RidderRecords(qp)
			{
				Data = records.AsEnumerable().Select(x => x.ToDictionary()).ToList(),
				Columns = qp.Columns,
				Filter = filter,
				Sort = qp.Sort
			};
			// Return the result
			return result;
		}

		#endregion Documents
	}
}