using System.Web.Mvc;

namespace RidderIQAPI.Controllers
{
	/// <summary>
	/// Home controller
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// Inder page
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			// Redirect to swagger
			ViewBag.Title = "Home Page";
			return Redirect("/swagger");
		}
	}
}