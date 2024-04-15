using RidderIQAPI.Api.ApiRidderIQ;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models.RidderIQ;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers.RidderIQ
{
	/// <summary>
	/// API User controller
	/// </summary>
	[SwaggerControllerName("Ridder/User")]
	[RoutePrefix("api/user")]
	public class RidderIQUserController : ApiBase
	{
		/// <summary>
		/// Get user info
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("")]
		[ResponseType(typeof(RidderIQUserInfo))]
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
		public IHttpActionResult Login([FromBody] RidderIQCredential user)
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