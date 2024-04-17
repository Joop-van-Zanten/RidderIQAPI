using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers.RidderIQ
{
	/// <summary>
	/// API Actions and Events Sales controller
	/// </summary>
	[SwaggerControllerName("Stock/Actions")]
	[RoutePrefix("api/Stock/Actions")]
	public class RidderIQStockEventsController : ApiBase
	{
		/// <summary>
		/// Get physical stock for lot item
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
			return Execute(() => ApiRidderIQ.StockActions.GetPhysicalStockForLotItem(Request.GetCookies(), lotID, warehouseId));
		}

		/// <summary>
		/// Get physical stock for item
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("GetPhysicalStockForItem/{itemID}/{warehouseId}")]
		[ResponseType(typeof(bool))]
		public IHttpActionResult GetPhysicalStockForItem(
			int itemID,
			int warehouseId
		)
		{
			return Execute(() => ApiRidderIQ.StockActions.GetPhysicalStockForItem(Request.GetCookies(), itemID, warehouseId));
		}
	}
}