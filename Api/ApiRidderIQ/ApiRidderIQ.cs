using ADODB;
using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKDataAcces;
using Ridder.Common.ADO;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Web;

namespace RidderIQAPI.Api.ApiRidderIQ
{
	/// <summary>
	/// RidderIQ API handler
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

		private static readonly RidderIQSDK RidderSDK = new RidderIQSDK();

		private static readonly List<RidderIQCredentialToken> SdkClients = new List<RidderIQCredentialToken>();

		internal static RidderIQSDK GetClient(Collection<CookieHeaderValue> cookies, bool throwException = false)
		{
			try
			{
				RidderIQCredential cred = GetIqCredential(cookies);
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

					Register(new RidderIQCredentialToken(cred, result));
				}
				return result;
			}
			catch { }
			if (throwException)
				throw new UnauthorizedAccessException();
			return default;
		}

		internal static void Register(RidderIQCredentialToken token)
		{
			if (!SdkClients.Any(x => x.Equals(token)))
				SdkClients.Add(token);
		}

		internal static void UnRegister(Collection<CookieHeaderValue> cookies)
		{
			RidderIQCredential cred = GetIqCredential(cookies);
			if (cred is null)
				return;
			RidderIQCredentialToken item = SdkClients.FirstOrDefault(x => x.Person == cred);
			if (item != null)
			{
				item.Dispose();
				SdkClients.Remove(item);
			}
		}

		private static RidderIQSDK GetClient(RidderIQCredential cred) => SdkClients.FirstOrDefault(x => x.Person == cred)?.Sdk;

		internal static RidderIQCredential GetIqCredential(Collection<CookieHeaderValue> cookies)
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
						string decrypted = cValue?.OpenSSLDecrypt();
						return decrypted?.DeserializeJSON<RidderIQCredential>() ?? (decrypted?.DeserializeXML<RidderIQCredential>());
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

		public static List<RidderIQWorflowVisibility> RecordsGetWorkflows(
			Collection<CookieHeaderValue> cookies,
			string table,
			string recordID
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);

			// Load the table record
			SDKRecordset tableRecord = sdk.CreateRecordset(
				$"{table}",
				null,
				$"PK_{table} = '{recordID}'",
				null
			);
			tableRecord.MoveFirst();
			// Get the current workflowstate of the table record
			var tableRecordWorkflowState = tableRecord.GetField<Guid>("FK_WORKFLOWSTATE");

			// Find the table
			SDKRecordset tableInfo = sdk.CreateRecordset(
				"M_TABLEINFO",
				null,
				$"TABLENAME = '{table}' AND FK_WORKFLOWMODEL IS NOT NULL",
				null
			);
			// Check if any records are found
			if (tableInfo.RecordCount == 0)
				throw new KeyNotFoundException();
			// Move the pointer to the start
			tableInfo.MoveFirst();
			// Get the workflow model
			Guid workflowModelID = tableInfo.GetField<Guid>("FK_WORKFLOWMODEL");

			SDKRecordset workflowEvents = sdk.CreateRecordset(
				"M_WORKFLOWEVENT",
				null,
				$"FK_WORKFLOWMODEL = '{workflowModelID}' AND TYPE != 3",
				"SEQUENCENUMBER, NAME"
			);
			// Check if any records are found
			if (workflowEvents.RecordCount == 0)
				throw new KeyNotFoundException();

			List<RidderIQWorflowVisibility> result = new List<RidderIQWorflowVisibility>();
			foreach (SDKRecordset item in workflowEvents.AsEnumerable())
			{
				var hasPermission = CheckPermissionsWorkflows(
					cookies,
					new RidderIQPermissionWorkflowReqeust(
						(RidderIQDesignerScope)item.GetField<int>("SCOPE"),
						item.GetField<Guid>("PK_M_WORKFLOWEVENT")
					)
				).First().Result;
				if (!hasPermission)
					continue;

				var wfScope = (RidderIQDesignerScope)item.GetField<int>("SCOPE");
				var wfEvent = item.GetField<Guid>("PK_M_WORKFLOWEVENT");

				RidderIQWorflowVisibility add = new RidderIQWorflowVisibility
				{
					Event = wfEvent,
					Scope = wfScope,
					Name = item.GetField<string>("NAME"),
					Action = item.GetField<string>("ACTIONNAME"),
					Caption = item.GetField<string>("CAPTION"),
					SequenceNumber = item.GetField<int>("SEQUENCENUMBER")
				};

				// Step 1: Get a list of states that the records requires to be in
				var states = sdk.CreateRecordset(
					"M_WORKFLOWEVENTSTATES",
					"FK_STATE",
					$"FK_WORKFLOWEVENT = '{item.GetField<Guid>("PK_M_WORKFLOWEVENT")}'",
					null
				).AsEnumerable().Select(x => x.GetField<Guid>("FK_STATE")).ToList();

				if (states.Count > 0 && !states.Contains(tableRecordWorkflowState))
					continue;

				var systemVisibility = item.GetField<byte[]>("AVAILABILITY");
				if (systemVisibility != default)
					if (sdk.GetCalculatedColumnOutput(systemVisibility, table, recordID) is bool b)
						if (b == false)
							continue;

				var customVisibility = item.GetField<byte[]>("USERAVAILABILITY");
				if (customVisibility != default)
					if (sdk.GetCalculatedColumnOutput(customVisibility, table, recordID) is bool b)
						if (b == false)
							continue;

				var userVisibility = item.GetField<byte[]>("USERAVAILABILITY");
				if (userVisibility != default)
					if (sdk.GetCalculatedColumnOutput(userVisibility, table, recordID) is bool b)
						if (b == false)
							continue;

				result.Add(add);
			}
			return result.OrderBy(x => x.SequenceNumber == 0)
				.ThenBy(x => x.SequenceNumber)
				.ThenBy(x => x.Name)
				.ToList();
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
	}
}