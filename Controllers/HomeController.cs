using System.Web.Mvc;

namespace RidderIQAPI.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";
			return Redirect("/swagger");
			return View();
		}
	}
}