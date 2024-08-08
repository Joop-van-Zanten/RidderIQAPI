using ADODB;
using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKDataAcces;
using Ridder.Common.ADO;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceModel;

namespace RidderIQAPI.Api
{
	/// <summary>
	/// Ridder IQ API handler
	/// </summary>
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Ridder IQ API Records handler
		/// </summary>
		internal static class Records
		{
			public static RidderIQSDKResult Create(
				Collection<CookieHeaderValue> cookies,
				string table,
				Dictionary<string, object> fields,
				bool UseDataChanges = true
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Create new empty record
				var record = sdk.CreateRecordset(
					table,
					null,
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
					return new RidderIQSDKResult(record.Update());
				}
				catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
				{
					return new RidderIQSDKResult(ex2);
				}
				catch (Exception)
				{
					throw;
				}
			}

			public static RidderIQSDKResult Delete(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
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
					return new RidderIQSDKResult(records.Update());
				}
				catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
				{
					return new RidderIQSDKResult(ex2);
				}
				catch (Exception)
				{
					throw;
				}
			}

			public static RidderIQSDKResult DocumentsAdd(
				Collection<CookieHeaderValue> cookies,
				RidderIQAddDocument add
			)
			{
				try
				{
					// Get the SDK Client
					RidderIQSDK sdk = Core.GetClient(cookies, true);
					if (!File.Exists(add.File))
						throw new FileNotFoundException(add.File);

					// Find record in the DB, prevents duplicates
					var doc = sdk.CreateRecordset(new QueryParameters(
						"R_DOCUMENT",
						"DOCUMENTLOCATION",
						$"DOCUMENTLOCATION = '{add.File}'",
						"DOCUMENTLOCATION",
						1,
						1
					));

					if (doc.RecordCount == 1)
					{
						doc.MoveFirst();
						int docID = doc.GetField<int>("PK_R_DOCUMENT");

						// Check if the document is not already assigned
						string filter = $"{add.Table.Replace("R_", "FK_")} = {add.RecordID} AND FK_DOCUMENT = {docID}";
						SDKRecordset records = sdk.CreateRecordset($"{add.Table}DOCUMENT", null, filter, null);
						if (records.RecordCount == 0)
						{
							// Create new record set with max 1 record
							SDKRecordset createRecord = sdk.CreateRecordset(new QueryParameters($"{add.Table}DOCUMENT", pageSize: 1));
							// Add new record
							createRecord.AddNew();
							// Add the required fields
							List<string> fields = new List<string>();
							foreach (Field f in createRecord.Fields)
								fields.Add(f.Name);

							string ForeignKey = $"FK_{add.Table}";
							string ForeignKey2 = $"FK_{string.Join("_", add.Table.Split('_').Skip(1))}";

							if (fields.Contains(ForeignKey))
								createRecord.SetFieldValue(ForeignKey, add.RecordID);
							else if (fields.Contains(ForeignKey2))
								createRecord.SetFieldValue(ForeignKey2, add.RecordID);
							else
								throw new Exception("Unable to link document!");

							createRecord.SetFieldValue("FK_DOCUMENT", docID);
							// Apply Ridder DataChanges
							createRecord.UseDataChanges = true;
							// Update the record in order to create it
							ISDKResult createResult = createRecord.Update();
							return new RidderIQSDKResult(createResult);
						}

						return new RidderIQSDKResult(false, docID);
					}
					else
					{
						// Create new file using the SDK
						ISDKResult createResult = sdk.EventsAndActions.CRM.Actions.AddDocument(
							add.File,
							add.Revision,
							add.SaveAsLocation,
							add.Table,
							add.RecordID,
							add.Description
						);
						return new RidderIQSDKResult(createResult);
					}
				}
				catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
				{
					return new RidderIQSDKResult(ex2);
				}
				catch (Exception)
				{
					throw;
				}
			}

			public static RidderIQRecords DocumentsGet(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID,
				string columns,
				string filter,
				string sort
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

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
				RidderIQRecords result = new RidderIQRecords(qp)
				{
					Data = records.AsEnumerable().Select(x => x.ToDictionary()).ToList(),
					Columns = qp.Columns,
					Filter = filter,
					Sort = qp.Sort
				};
				// Return the result
				return result;
			}

			public static RidderIQSDKResult ExecuteWorkflows(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID,
				Guid workflowID,
				List<RidderIQWorkFlowEventParameter> jsonParameters = null
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

				Dictionary<string, object> parameters = new Dictionary<string, object>();

				if (jsonParameters != null && jsonParameters.Count > 0)
				{
					foreach (RidderIQWorkFlowEventParameter jsonParameter in jsonParameters)
					{
						switch (jsonParameter.Type)
						{
							case RidderIQWorkflowParamaterType.String:
								parameters.Add(jsonParameter.Key, jsonParameter.Value.ToString().Trim());
								break;

							case RidderIQWorkflowParamaterType.Int:
								parameters.Add(jsonParameter.Key, Convert.ToInt32(jsonParameter.Value));
								break;

							case RidderIQWorkflowParamaterType.Double:
								parameters.Add(jsonParameter.Key, Convert.ToDouble(jsonParameter.Value));
								break;

							case RidderIQWorkflowParamaterType.Long:
								parameters.Add(jsonParameter.Key, Convert.ToInt64(jsonParameter.Value));
								break;

							case RidderIQWorkflowParamaterType.DateTime:
								parameters.Add(jsonParameter.Key, Convert.ToDateTime(jsonParameter.Value));
								break;

							default:
								throw new NotImplementedException($"Type {jsonParameter.Type} is currently not implemented");
						}
					}
				}

				try
				{
					ISDKResult sdkResult = null;

					if (parameters == null || parameters.Count == 0)
						sdkResult = sdk.ExecuteWorkflowEvent(table, Convert.ToInt32(recordID), workflowID);
					else
						sdkResult = sdk.ExecuteWorkflowEvent(table, Convert.ToInt32(recordID), workflowID, parameters);
					return new RidderIQSDKResult(sdkResult);
				}
				catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
				{
					return new RidderIQSDKResult(ex2);
				}
				catch (Exception)
				{
					throw;
				}
			}

			public static RidderIQRecords GetList(
				Collection<CookieHeaderValue> cookies,
				string table,
				string columns,
				string filter,
				string sort,
				int page = 1,
				int pageSize = Core.MaxPageSize
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

				// Define the Query parameters
				QueryParameters qp = new QueryParameters(
					table?.ToUpper(),
					(columns == null ? null : string.Join(",", columns.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x)))?.ToUpper(),
					filter?.ToUpper(),
					(sort == null ? null : string.Join(",", sort.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x)))?.ToUpper(),
					page >= 1 ? page : 1,
					pageSize <= Core.MaxPageSize ? pageSize : Core.MaxPageSize
				);

				// Create recordset from the QueryParameters
				var records = sdk.CreateRecordset(qp);
				// Move the pointer to the start
				records.MoveFirst();
				// Create result
				RidderIQRecords result = new RidderIQRecords(qp)
				{
					Data = records.AsEnumerable().Select(x => x.ToDictionary()).ToList(),
					Columns = columns,
					Filter = filter,
					Sort = qp.Sort,
					RecordCount = records.RecordCount,
					Page = qp.Page
				};
				// Check if a next page is available (one more record than input given)
				if (result.Data.Count >= qp.Page)
				{
					// Find next record
					// Set single column
					qp.Columns = $"PK_{qp.TableName}";
					// Calculate page, given a single pagesize must be returned
					qp.Page = (qp.Page * qp.PageSize) + 1;
					// Set the pageSize tot 1: Single record
					qp.PageSize = 1;
					// Check if a next record is present
					if (sdk.CreateRecordset(qp).RecordCount > 0)
						// Set flag for hase more
						result.HasMore = true;
				}

				// Verify columns
				VerifyColumns(result);

				// Return the result
				return result;
			}

			public static string GetRecordTag(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				return sdk.GetRecordTag(table, recordID);
			}

			public static Dictionary<string, object> GetSingle(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID,
				string columns
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
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

				// Verify columns
				VerifyColumns(result, columns);

				if (!string.IsNullOrWhiteSpace(columns))
				{
					List<string> keyNotPresent = columns
						.Split(',')
						.Where(x => !result.ContainsKey(x))
						.ToList();

					if (keyNotPresent.Count > 0)
						throw new Exception($"Onvoldoende lees rechten op {table}: {string.Join(", ", keyNotPresent)}");
				}

				// Return the result
				return result;
			}

			public static List<RidderIQWorflowVisibility> GetWorkflows(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

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
					var hasPermission = Permissions.CheckPermissionsWorkflows(
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

			public static List<Dictionary<string, object>> TableWorkflows(
				Collection<CookieHeaderValue> cookies,
				string table
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

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

			public static RidderIQSDKResult Update(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID,
				Dictionary<string, object> data
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
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
					var newValue = field.Value;
					if (newValue == null)
						newValue = DBNull.Value;
					record.Fields[field.Key].Value = newValue;
				}

				try
				{
					// Update the record: Save
					return new RidderIQSDKResult(record.Update());
				}
				catch (FaultException<Ridder.Common.TranslationMessageInfo> ex2)
				{
					return new RidderIQSDKResult(ex2);
				}
				catch (Exception)
				{
					throw;
				}
			}

			public static bool WorkflowsGetVisible(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID,
				Guid workflowID
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

				// Load the table record
				SDKRecordset tableRecord = sdk.CreateRecordset($"{table}", null, $"PK_{table} = '{recordID}'", null);
				tableRecord.MoveFirst();

				// Get the current workflowstate of the table record
				var tableRecordWorkflowState = tableRecord.GetField<Guid>("FK_WORKFLOWSTATE");

				SDKRecordset workflowEvent = sdk.CreateRecordset(
					"M_WORKFLOWEVENT",
					null,
					$"PK_M_WORKFLOWEVENT = '{workflowID}' AND TYPE != 3",
					"SEQUENCENUMBER, NAME"
				);
				workflowEvent.MoveFirst();
				// Check if any records are found
				if (workflowEvent.RecordCount == 0)
					throw new KeyNotFoundException();

				List<RidderIQWorflowVisibility> result = new List<RidderIQWorflowVisibility>();

				var hasPermission = Permissions.CheckPermissionsWorkflows(
					cookies,
					new RidderIQPermissionWorkflowReqeust(
						(RidderIQDesignerScope)workflowEvent.GetField<int>("SCOPE"),
						workflowEvent.GetField<Guid>("PK_M_WORKFLOWEVENT")
					)
				).First().Result;

				if (!hasPermission)
					return false;

				// Step 1: Get a list of states that the records requires to be in
				var states = sdk.CreateRecordset(
					"M_WORKFLOWEVENTSTATES",
					"FK_STATE",
					$"FK_WORKFLOWEVENT = '{workflowEvent.GetField<Guid>("PK_M_WORKFLOWEVENT")}'",
					null
				).AsEnumerable().Select(x => x.GetField<Guid>("FK_STATE")).ToList();

				if (states.Count > 0 && !states.Contains(tableRecordWorkflowState))
					return false;

				var systemVisibility = workflowEvent.GetField<byte[]>("AVAILABILITY");
				if (systemVisibility != default)
					if (sdk.GetCalculatedColumnOutput(systemVisibility, table, recordID) is bool b)
						if (b == false)
							return false;

				var customVisibility = workflowEvent.GetField<byte[]>("USERAVAILABILITY");
				if (customVisibility != default)
					if (sdk.GetCalculatedColumnOutput(customVisibility, table, recordID) is bool b)
						if (b == false)
							return false;

				var userVisibility = workflowEvent.GetField<byte[]>("USERAVAILABILITY");
				if (userVisibility != default)
					if (sdk.GetCalculatedColumnOutput(userVisibility, table, recordID) is bool b)
						if (b == false)
							return false;

				return true;
			}

			public static List<RidderIQWorflowVisibility> WorkflowsGetVisible(
				Collection<CookieHeaderValue> cookies,
				string table,
				string recordID
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);

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
					var hasPermission = Permissions.CheckPermissionsWorkflows(
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
		}

		private static void VerifyColumns(RidderIQRecords result) => VerifyColumns(result?.Data?.First(), result?.Columns);

		private static void VerifyColumns(Dictionary<string, object> data, string columns)
		{
			if (
				data == null ||
				columns == null
			)
				return;

			List<string> keyNotPresent = columns
				.Split(',')
				.Where(x => !string.IsNullOrEmpty(x))
				.Select(x => x.Trim())
				.Where(x => !data.ContainsKey(x))
				.ToList();

			if (keyNotPresent.Count == 0)
				return;

			string table = data.Keys.First(x => x.StartsWith("PK_")).Remove(0, 3);
			throw new Exception($"Onvoldoende lees rechten op {table}: {string.Join(", ", keyNotPresent)}");
		}
	}
}