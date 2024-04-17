using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers.RidderIQ
{
	/// <summary>
	/// API Permissions controller
	/// </summary>
	[SwaggerControllerName("Permissions")]
	[RoutePrefix("api/permissions")]
	public class RidderIQPermissionController : ApiBase
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
		[ResponseType(typeof(RidderIQPermissionActionResult))]
		public IHttpActionResult CheckPermissionAction(RidderIQDesignerScope scope, string actionName) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionActions(Request.GetCookies(), new RidderIQPermissionActionReqeust(scope, actionName)).FirstOrDefault());

		/// <summary>
		/// Check Permission: Action (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Action permissions list</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Action")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionActionResult>))]
		public IHttpActionResult CheckPermissionAction([FromBody] RidderIQPermissionActionReqeust[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionActions(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Column
		/// </summary>
		/// <returns></returns>
		/// <param name="tableName">Table</param>
		/// <param name="columnName">Column</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Column/{tableName}/{columnName}")]
		[ResponseType(typeof(RidderIQPermissionColumnResult))]
		public IHttpActionResult CheckPermissionColumn(string tableName, string columnName) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionsColumns(Request.GetCookies(), new RidderIQPermissionColumnRequest(tableName, columnName)).FirstOrDefault());

		/// <summary>
		/// Check Permission: Column (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Column")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionColumnResult>))]
		public IHttpActionResult CheckPermissionColumn([FromBody] RidderIQPermissionColumnRequest[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionsColumns(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Custom Permission
		/// </summary>
		/// <returns></returns>
		/// <param name="customPermissionId">Custom permission context</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Custompermission/{customPermissionId}")]
		[ResponseType(typeof(RidderIQPermissionCustomResult))]
		public IHttpActionResult CheckPermissionCustomPermission(Guid customPermissionId) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionCustomPermission(Request.GetCookies(), customPermissionId).FirstOrDefault());

		/// <summary>
		/// Check Permission: Custom Permission (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Custom permission context</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Custompermission/")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionCustomResult>))]
		public IHttpActionResult CheckPermissionCustomPermission([FromBody] Guid[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionCustomPermission(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: FormPart
		/// </summary>
		/// <returns></returns>
		/// <param name="formPartName">Formpart name</param>
		/// <param name="scope">Designer scope</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Formpart/{scope}/{formPartName}")]
		[ResponseType(typeof(RidderIQPermissionFormPartResult))]
		public IHttpActionResult CheckPermissionFormPart(RidderIQDesignerScope scope, string formPartName) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionFormPart(Request.GetCookies(), new RidderIQPermissionFormPartReqeust(scope, formPartName)).FirstOrDefault());

		/// <summary>
		/// Check Permission: FormPart (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Formpart name</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Formpart")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionFormPartResult>))]
		public IHttpActionResult CheckPermissionFormPart([FromBody] RidderIQPermissionFormPartReqeust[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionFormPart(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Report
		/// </summary>
		/// <returns></returns>
		/// <param name="scope">Designer scope</param>
		/// <param name="reportId">Report ID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Report/{scope}/{reportId}")]
		[ResponseType(typeof(RidderIQPermissionReportResult))]
		public IHttpActionResult CheckPermissionReport(RidderIQDesignerScope scope, Guid reportId) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionReport(Request.GetCookies(), new RidderIQPermissionReportReqeust(scope, reportId)));

		/// <summary>
		/// Check Permission: Report (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Report")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionReportResult>))]
		public IHttpActionResult CheckPermissionReport([FromBody] RidderIQPermissionReportReqeust[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionReport(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Script
		/// </summary>
		/// <returns></returns>
		/// <param name="scope">Designer scope</param>
		/// <param name="name">Script name</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Script/{scope}/{name}")]
		[ResponseType(typeof(RidderIQPermissionScriptResult))]
		public IHttpActionResult CheckPermissionScript(RidderIQDesignerScope scope, string name) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionScript(Request.GetCookies(), new RidderIQPermissionScriptReqeust(scope, name)));

		/// <summary>
		/// Check Permission: Script (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Action permissions list</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Script")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionScriptResult>))]
		public IHttpActionResult CheckPermissionScript([FromBody] RidderIQPermissionScriptReqeust[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionScript(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Table
		/// </summary>
		/// <returns></returns>
		/// <param name="tableName">Table name</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Table/{tableName}")]
		[ResponseType(typeof(RidderIQPermissionTableResult))]
		public IHttpActionResult CheckPermissionTable(string tableName) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionsTable(Request.GetCookies(), tableName).FirstOrDefault());

		/// <summary>
		/// Check Permission: Tables (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Table")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionTableResult>))]
		public IHttpActionResult CheckPermissionTable([FromBody] string[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionsTable(Request.GetCookies(), checks));

		/// <summary>
		/// Check Permission: Workflow
		/// </summary>
		/// <returns></returns>
		/// <param name="scope">Designer scope</param>
		/// <param name="workflowEventId">Workflow ID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Workflow/{scope}/{workflowEventId}")]
		[ResponseType(typeof(RidderIQPermissionWorkflowResult))]
		public IHttpActionResult CheckPermissionWorkflow(RidderIQDesignerScope scope, Guid workflowEventId) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionsWorkflows(Request.GetCookies(), new RidderIQPermissionWorkflowReqeust(scope, workflowEventId)).FirstOrDefault());

		/// <summary>
		/// Check Permission: Workflow (Multiple)
		/// </summary>
		/// <returns></returns>
		/// <param name="checks">Multiple permission checks</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Workflow")]
		[ResponseType(typeof(IEnumerable<RidderIQPermissionWorkflowResult>))]
		public IHttpActionResult CheckPermissionWorkflow([FromBody] RidderIQPermissionWorkflowReqeust[] checks) => Execute(() => ApiRidderIQ.Permissions.CheckPermissionsWorkflows(Request.GetCookies(), checks));
	}
}