using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models;
using RidderIQAPI.Models.RidderPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers
{
	/// <summary>
	/// API Permissions controller
	/// </summary>
	[SwaggerControllerName("Permissions")]
	[RoutePrefix("api/permissions")]
	public class ApiRidderPermissionController : ApiBase
	{
		/// <summary>
		/// Check Permission: Action
		/// </summary>
		/// <returns></returns>
		/// <param name="scope">Designer scope</param>
		/// <param name="actionName">Permission name</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Action/{scope}/{actionName}")]
		[ResponseType(typeof(Models.RidderPermission.ActionResult))]
		public IHttpActionResult CheckPermissionAction(RidderDesignerScope scope, string actionName) => Execute(() => ApiRidderIQ.CheckPermissionActions(Request.GetCookies(), new ActionReqeust(scope, actionName)).FirstOrDefault());

		/// <summary>
		/// Check Permission: Action (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Action permissions list</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Action")]
		[ResponseType(typeof(IEnumerable<Models.RidderPermission.ActionResult>))]
		public IHttpActionResult CheckPermissionAction([FromBody] ActionReqeust[] checks) => Execute(() => ApiRidderIQ.CheckPermissionActions(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Column
		/// </summary>
		/// <returns></returns>
		/// <param name="tableName">Table</param>
		/// <param name="columnName">Column</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Column/{tableName}/{columnName}")]
		[ResponseType(typeof(ColumnResult))]
		public IHttpActionResult CheckPermissionColumn(string tableName, string columnName) => Execute(() => ApiRidderIQ.CheckPermissionsColumns(Request.GetCookies(), new ColumnRequest(tableName, columnName)).FirstOrDefault());

		/// <summary>
		/// Check Permission: Column (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Column")]
		[ResponseType(typeof(IEnumerable<ColumnResult>))]
		public IHttpActionResult CheckPermissionColumn([FromBody] ColumnRequest[] checks) => Execute(() => ApiRidderIQ.CheckPermissionsColumns(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Custom Permission
		/// </summary>
		/// <returns></returns>
		/// <param name="customPermissionId">Custom permission context</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Custompermission/{customPermissionId}")]
		[ResponseType(typeof(CustomResult))]
		public IHttpActionResult CheckPermissionCustomPermission(Guid customPermissionId) => Execute(() => ApiRidderIQ.CheckPermissionCustomPermission(Request.GetCookies(), customPermissionId).FirstOrDefault());

		/// <summary>
		/// Check Permission: Custom Permission (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Custom permission context</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Custompermission/")]
		[ResponseType(typeof(IEnumerable<CustomResult>))]
		public IHttpActionResult CheckPermissionCustomPermission([FromBody] Guid[] checks) => Execute(() => ApiRidderIQ.CheckPermissionCustomPermission(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: FormPart
		/// </summary>
		/// <returns></returns>
		/// <param name="formPartName">Formpart name</param>
		/// <param name="scope">Designer scope</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Formpart/{scope}/{formPartName}")]
		[ResponseType(typeof(FormPartResult))]
		public IHttpActionResult CheckPermissionFormPart(RidderDesignerScope scope, string formPartName) => Execute(() => ApiRidderIQ.CheckPermissionFormPart(Request.GetCookies(), new FormPartReqeust(scope, formPartName)).FirstOrDefault());

		/// <summary>
		/// Check Permission: FormPart (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Formpart name</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Formpart")]
		[ResponseType(typeof(IEnumerable<FormPartResult>))]
		public IHttpActionResult CheckPermissionFormPart([FromBody] FormPartReqeust[] checks) => Execute(() => ApiRidderIQ.CheckPermissionFormPart(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Report
		/// </summary>
		/// <returns></returns>
		/// <param name="scope">Designer scope</param>
		/// <param name="reportId">Report ID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Report/{scope}/{reportId}")]
		[ResponseType(typeof(ReportResult))]
		public IHttpActionResult CheckPermissionReport(RidderDesignerScope scope, Guid reportId) => Execute(() => ApiRidderIQ.CheckPermissionReport(Request.GetCookies(), new ReportReqeust(scope, reportId)));

		/// <summary>
		/// Check Permission: Report (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Report")]
		[ResponseType(typeof(IEnumerable<ReportResult>))]
		public IHttpActionResult CheckPermissionReport([FromBody] ReportReqeust[] checks) => Execute(() => ApiRidderIQ.CheckPermissionReport(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Script
		/// </summary>
		/// <returns></returns>
		/// <param name="scope">Designer scope</param>
		/// <param name="name">Script name</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Script/{scope}/{name}")]
		[ResponseType(typeof(ScriptResult))]
		public IHttpActionResult CheckPermissionScript(RidderDesignerScope scope, string name) => Execute(() => ApiRidderIQ.CheckPermissionScript(Request.GetCookies(), new ScriptReqeust(scope, name)));

		/// <summary>
		/// Check Permission: Script (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Action permissions list</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Script")]
		[ResponseType(typeof(IEnumerable<ScriptResult>))]
		public IHttpActionResult CheckPermissionScript([FromBody] ScriptReqeust[] checks) => Execute(() => ApiRidderIQ.CheckPermissionScript(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Table
		/// </summary>
		/// <returns></returns>
		/// <param name="tableName">Table name</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Table/{tableName}")]
		[ResponseType(typeof(TableResult))]
		public IHttpActionResult CheckPermissionTable(string tableName) => Execute(() => ApiRidderIQ.CheckPermissionsTable(Request.GetCookies(), tableName).FirstOrDefault());

		/// <summary>
		/// Check Permission: Tables (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Table")]
		[ResponseType(typeof(IEnumerable<TableResult>))]
		public IHttpActionResult CheckPermissionTable([FromBody] string[] checks) => Execute(() => ApiRidderIQ.CheckPermissionsTable(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Workflow
		/// </summary>
		/// <returns></returns>
		/// <param name="scope">Designer scope</param>
		/// <param name="workflowEventId">Workflow ID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Workflow/{scope}/{workflowEventId}")]
		[ResponseType(typeof(WorkflowResult))]
		public IHttpActionResult CheckPermissionWorkflow(RidderDesignerScope scope, Guid workflowEventId) => Execute(() => ApiRidderIQ.CheckPermissionsWorkflows(Request.GetCookies(), new WorkflowReqeust(scope, workflowEventId)).FirstOrDefault());

		/// <summary>
		/// Check Permission: Workflow (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Workflow")]
		[ResponseType(typeof(IEnumerable<WorkflowResult>))]
		public IHttpActionResult CheckPermissionWorkflow([FromBody] WorkflowReqeust[] checks) => Execute(() => ApiRidderIQ.CheckPermissionsWorkflows(Request.GetCookies(), checks));
	}
}