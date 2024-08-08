using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers.RidderIQ
{
	/// <summary>
	/// API User controller
	/// </summary>
	[SwaggerControllerName("Records")]
	[RoutePrefix("api/records")]
	public class RidderIQRecordsController : ApiBase
	{
		/// <summary>
		/// Show a document
		/// </summary>
		/// <param name="file">File</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Documents/{file}/")]
		public HttpResponseMessage RecordsShowDocument(
			string file
		)
		{
			//var docRecord = ApiRidderIQ.Records.GetSingle(Request.GetCookies(), "R_DOCUMENT", documentID.ToString(), null);
			string docFile = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(file));
			if (File.Exists(docFile))
			{
				HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
				FileStream fileStream = File.OpenRead(docFile);
				response.Content = new StreamContent(fileStream);
				response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

				return response;
			}

			return default;
		}

		/// <summary>
		/// Create record
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="fields">values to be updated</param>
		/// <param name="UseDataChanges">Use Datachanges</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("{table}")]
		[ResponseType(typeof(RidderIQSDKResult))]
		public IHttpActionResult RecordsCreate(
			string table,
			Dictionary<string, object> fields,
			[Optional][DefaultParameterValue(true)] bool UseDataChanges
		)
		{
			return Execute(() => ApiRidderIQ.Records.Create(Request.GetCookies(), table, fields, UseDataChanges));
		}

		/// <summary>
		/// Delete record
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <returns></returns>
		[HttpDelete()]
		[Route("{table}/{recordID}")]
		[ResponseType(typeof(RidderIQSDKResult))]
		public IHttpActionResult RecordsDelete(
			string table,
			string recordID
		)
		{
			return Execute(() => ApiRidderIQ.Records.Delete(Request.GetCookies(), table, recordID));
		}

		/// <summary>
		/// Add document to a record
		/// </summary>
		/// <param name="add">Document to be added</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("{table}/{recordID}/Documents")]
		[ResponseType(typeof(RidderIQSDKResult))]
		public IHttpActionResult RecordsDocumentsAdd([FromBody] RidderIQAddDocument add)
		{
			return Execute(() => ApiRidderIQ.Records.DocumentsAdd(Request.GetCookies(), add));
		}

		/// <summary>
		/// Get record documents
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <param name="columns">Columns to be loaded</param>
		/// <param name="filter">Query filter</param>
		/// <param name="sort">Sort by columns: Comma seperated values</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/{recordID}/Documents")]
		[ResponseType(typeof(RidderIQRecords))]
		public IHttpActionResult RecordsDocumentsGet(
			string table,
			string recordID,
			[Optional][DefaultParameterValue(null)] string columns,
			[Optional][DefaultParameterValue(null)] string filter,
			[Optional][DefaultParameterValue(null)] string sort
		)
		{
			return Execute(() => ApiRidderIQ.Records.DocumentsGet(Request.GetCookies(), table, recordID, columns, filter, sort));
		}

		/// <summary>
		/// Execute workflow event
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <param name="workflowID">Workflow to be executed</param>
		/// <param name="parameters">Parameters if required</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("{table}/{recordID}/Workflows/{workflowID}")]
		[ResponseType(typeof(RidderIQSDKResult))]
		public IHttpActionResult RecordsExecuteWorkflows(
			string table,
			string recordID,
			Guid workflowID,
			[DefaultParameterValue(null)][FromBody] List<RidderIQWorkFlowEventParameter> parameters
		)
		{
			return Execute(() =>
				ApiRidderIQ.Records.ExecuteWorkflows(Request.GetCookies(), table, recordID, workflowID, parameters)
			);
		}

		/// <summary>
		/// Get records
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="columns">Columns to be loaded</param>
		/// <param name="filter">Query filter</param>
		/// <param name="sort">Sort by columns: Comma seperated values</param>
		/// <param name="page">Page to be loaded</param>
		/// <param name="pageSize">Page size: 1-200</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}")]
		[ResponseType(typeof(RidderIQRecords))]
		public IHttpActionResult RecordsGetList(
			string table,
			[Optional][DefaultParameterValue(null)] string columns,
			[Optional][DefaultParameterValue(null)] string filter,
			[Optional][DefaultParameterValue(null)] string sort,
			[Optional][DefaultParameterValue(1)] int page,
			[Optional][DefaultParameterValue(ApiRidderIQ.Core.MaxPageSize)] int pageSize
		)
		{
			return Execute(() => ApiRidderIQ.Records.GetList(Request.GetCookies(), table, columns, filter, sort, page, pageSize));
		}

		/// <summary>
		/// Get record's RecordTag
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/{recordID}/Recordtag")]
		[ResponseType(typeof(string))]
		public IHttpActionResult RecordsGetRecordTag(
			string table,
			string recordID
		)
		{
			return Execute(() => ApiRidderIQ.Records.GetRecordTag(Request.GetCookies(), table, recordID));
		}

		/// <summary>
		/// Get record by ID
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <param name="columns">Columns to be loaded</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/{recordID}")]
		[ResponseType(typeof(Dictionary<string, object>))]
		public IHttpActionResult RecordsGetSingle(
			string table,
			string recordID,
			[Optional][DefaultParameterValue(null)] string columns
		)
		{
			return Execute(() => ApiRidderIQ.Records.GetSingle(Request.GetCookies(), table, recordID, columns));
		}

		/// <summary>
		/// Get available workflows
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/{recordID}/Workflows")]
		[ResponseType(typeof(List<RidderIQWorflowVisibility>))]
		public IHttpActionResult RecordsGetWorkflows(
			string table,
			string recordID
		)
		{
			return Execute(() => ApiRidderIQ.Records.GetWorkflows(Request.GetCookies(), table, recordID));
		}

		/// <summary>
		/// check if a workflow is available
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <param name="workflowID">Workflow ID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/{recordID}/Workflows/{workflowID}")]
		[ResponseType(typeof(bool))]
		public IHttpActionResult RecordsGetWorkflows(
			string table,
			string recordID,
			Guid workflowID
		)
		{
			return Execute(() => ApiRidderIQ.Records.WorkflowsGetVisible(Request.GetCookies(), table, recordID, workflowID));
		}

		/// <summary>
		/// Get table workflows
		/// </summary>
		/// <param name="table">Table name</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/Workflows")]
		[ResponseType(typeof(List<Dictionary<string, object>>))]
		public IHttpActionResult RecordsTableWorkflows(
			string table
		)
		{
			return Execute(() => ApiRidderIQ.Records.TableWorkflows(Request.GetCookies(), table));
		}

		/// <summary>
		/// Update record
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">ID</param>
		/// <param name="data">values to be updated</param>
		/// <returns></returns>
		[HttpPatch()]
		[Route("{table}/{recordID}")]
		[ResponseType(typeof(RidderIQSDKResult))]
		public IHttpActionResult RecordsUpdate(
			string table,
			string recordID,
			Dictionary<string, object> data
		)
		{
			return Execute(() => ApiRidderIQ.Records.Update(Request.GetCookies(), table, recordID, data));
		}
	}
}