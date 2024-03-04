using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers
{
	/// <summary>
	/// API User controller
	/// </summary>
	[SwaggerControllerName("Records")]
	[RoutePrefix("api/records")]
	public class ApiRidderRecordsController : ApiBase
	{
		/// <summary>
		/// Create record
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="columns">Columns to be returns, null/empty for all columns</param>
		/// <param name="fields">values to be updated</param>
		/// <param name="UseDataChanges">Use Datachanges</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("{table}")]
		[ResponseType(typeof(RidderSDKResult))]
		public IHttpActionResult RecordsCreate(
			string table,
			string columns,
			Dictionary<string, object> fields,
			[Optional][DefaultParameterValue(true)] bool UseDataChanges
		)
		{
			return Execute(() => ApiRidderIQ.RecordsCreate(Request.GetCookies(), table, columns, fields, UseDataChanges));
		}

		/// <summary>
		/// Delete record
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <returns></returns>
		[HttpDelete()]
		[Route("{table}/{recordID}")]
		[ResponseType(typeof(RidderSDKResult))]
		public IHttpActionResult RecordsDelete(
			string table,
			string recordID
		)
		{
			return Execute(() => ApiRidderIQ.RecordsDelete(Request.GetCookies(), table, recordID));
		}

		/// <summary>
		/// Execute a workflow event for a Database record
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <param name="workflowID">Workflow to be executed</param>
		/// <param name="parameters">Parameters if required</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("{table}/{recordID}/Workflows/{workflowID}")]
		[ResponseType(typeof(RidderSDKResult))]
		public IHttpActionResult RecordsExecuteWorkflows(
			string table,
			string recordID,
			Guid workflowID,
			[DefaultParameterValue(null)][FromBody] List<RidderWorkFlowEventParameter> parameters
		)
		{
			return Execute(() =>
				ApiRidderIQ.RecordsExecuteWorkflows(Request.GetCookies(), table, recordID, workflowID, parameters)
			);
		}

		/// <summary>
		/// Get record linked documents
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <param name="columns">Columns to be loaded</param>
		/// <param name="filter">Query filter</param>
		/// <param name="sort">Sort by columns: Comma seperated values</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/{recordID}/Documents")]
		[ResponseType(typeof(RidderRecords))]
		public IHttpActionResult RecordsGetDocuments(
			string table,
			string recordID,
			[Optional][DefaultParameterValue(null)] string columns,
			[Optional][DefaultParameterValue(null)] string filter,
			[Optional][DefaultParameterValue(null)] string sort
		)
		{
			return Execute(() => ApiRidderIQ.RecordsGetDocuments(Request.GetCookies(), table, recordID, columns, filter, sort));
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
		[ResponseType(typeof(RidderRecords))]
		public IHttpActionResult RecordsGetList(
			string table,
			[Optional][DefaultParameterValue(null)] string columns,
			[Optional][DefaultParameterValue(null)] string filter,
			[Optional][DefaultParameterValue(null)] string sort,
			[Optional][DefaultParameterValue(1)] int page,
			[Optional][DefaultParameterValue(ApiRidderIQ.MaxPageSize)] int pageSize
		)
		{
			return Execute(() => ApiRidderIQ.RecordsGetList(Request.GetCookies(), table, columns, filter, sort, page, pageSize));
		}

		/// <summary>
		/// Get the record tag of a record
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
			return Execute(() => ApiRidderIQ.RecordsGetRecordTag(Request.GetCookies(), table, recordID));
		}

		/// <summary>
		/// Get record
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
			return Execute(() => ApiRidderIQ.RecordsGetSingle(Request.GetCookies(), table, recordID, columns));
		}

		/// <summary>
		/// Get available workflows for a table record
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{table}/{recordID}/Workflows")]
		[ResponseType(typeof(List<RidderWorflowVisibility>))]
		public IHttpActionResult RecordsGetWorkflows(
			string table,
			string recordID
		)
		{
			return Execute(() => ApiRidderIQ.RecordsGetWorkflows(Request.GetCookies(), table, recordID));
		}

		/// <summary>
		/// Get all available workflows for a specific table
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
			return Execute(() => ApiRidderIQ.RecordsTableWorkflows(Request.GetCookies(), table));
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
		[ResponseType(typeof(RidderSDKResult))]
		public IHttpActionResult RecordsUpdate(
			string table,
			string recordID,
			Dictionary<string, object> data
		)
		{
			return Execute(() => ApiRidderIQ.RecordsUpdate(Request.GetCookies(), table, recordID, data));
		}
	}
}