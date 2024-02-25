using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers
{
	/// <summary>
	/// API User controller
	/// </summary>
	[SwaggerControllerName("Workflows")]
	[RoutePrefix("api/workflows")]
	public class ApiRidderWorkflowController : ApiBase
	{
		#region WorkFlows

		/// <summary>
		/// Check if a Workflow Event is visiable
		/// </summary>
		/// <param name="ID">Workflow to be executed</param>
		/// <param name="recordID">Table record ID, Int or GUID</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("{ID}/Visible/{recordID}")]
		[ResponseType(typeof(string))]
		public IHttpActionResult WorkflowEventAvailable(
			Guid ID,
			string recordID)
		{
			return Execute(() => ApiRidderIQ.WorkflowEventAvailable(Request.GetCookies(), ID, recordID));
		}

		#endregion WorkFlows
	}
}