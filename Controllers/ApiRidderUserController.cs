using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers
{
	/// <summary>
	/// API User controller
	/// </summary>
	[SwaggerControllerName("User")]
	[RoutePrefix("api/user")]
	public class ApiRidderUserController : ApiBase
	{
		/// <summary>
		/// Get user info
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("User")]
		[ResponseType(typeof(RidderUserInfo))]
		public IHttpActionResult GetUser() => Execute(() => ApiRidderIQ.GetUser(Request.GetCookies()));

		/// <summary>
		/// Logged in status
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("Loggedin")]
		[ResponseType(typeof(bool))]
		public IHttpActionResult LoggedIn() => Execute(() => ApiRidderIQ.LoggedIn(Request.GetCookies()));

		/// <summary>
		/// Login
		/// </summary>
		/// <param name="user">User information</param>
		/// <returns></returns>
		[HttpPost()]
		[Route("Login")]
		[ResponseType(typeof(bool))]
		public IHttpActionResult Login([FromBody] RidderCredential user)
		{
			return Execute((ActionResult ar) =>
			{
				ar.Add(ApiRidderIQ.Login(Request.GetCookies(), user));
				return true;
			});
		}

		/// <summary>
		/// Logout
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("Logout")]
		[ResponseType(typeof(bool))]
		public IHttpActionResult Logout()
		{
			return Execute((ActionResult ar) =>
			{
				ar.Add(ApiRidderIQ.Logout(Request.GetCookies()));
				return true;
			});
		}
	}
}