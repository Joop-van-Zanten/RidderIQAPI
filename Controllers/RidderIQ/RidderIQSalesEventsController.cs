using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models.RidderIQ;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers.RidderIQ
{
	/// <summary>
	/// API Actions and Events Sales controller
	/// </summary>
	[SwaggerControllerName("Sales/Event")]
	[RoutePrefix("api/Sales/Events")]
	public class RidderIQSalesEventsController : ApiBase
	{
		/// <summary>
		/// Create shipping order from selected details
		/// </summary>
		/// <param name="obj">Creating shipping order model</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("CreateShippingOrderFromOrderFromSelectedDetails")]
		[ResponseType(typeof(RidderIQSDKResult))]
		public IHttpActionResult CreateShippingOrderFromOrderFromSelectedDetails([FromBody] RidderIQCreateShippingOrder obj)
		{
			return Execute((ActionResult ar) =>
			{
				return ApiRidderIQ.SalesEvents.CreateShippingOrderFromOrderFromSelectedDetails(Request.GetCookies(), obj);
			});
		}
	}
}