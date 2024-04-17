using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers.RidderIQ
{
	/// <summary>
	/// API Actions and Events Sales controller
	/// </summary>
	[SwaggerControllerName("EventsAndActions/Stock/Actions")]
	[RoutePrefix("api/EventsAndActions/Stock/Actions")]
	public class RidderIQEventsAndActionsStockEventsController : ApiBase
	{
		/// <summary>
		/// EventsAndActions.Stock.Actions.GetPhysicalStockForLotItem
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("GetPhysicalStockForLotItem/{lotID}/{warehouseId}")]
		[ResponseType(typeof(bool))]
		public IHttpActionResult GetPhysicalStockForLotItem(
			int lotID,
			int warehouseId
		)
		{
			return Execute(() => ApiRidderIQ.EventsAndActions.StockActions.GetPhysicalStockForLotItem(Request.GetCookies(), lotID, warehouseId));
		}
	}
}