using RidderIQAPI.Api;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers
{
	/// <summary>
	/// API Anonymous controller
	/// </summary>
	[SwaggerControllerName("Anonymous")]
	[RoutePrefix("api/anonymous")]
	public class ApiRidderAnonymousController : ApiBase
	{
		/// <summary>
		/// Get a list of all available administrations
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("Companies")]
		[ResponseType(typeof(List<RidderLoginCompany>))]
		public IHttpActionResult GetCompanys()
		{
			return Execute(() => ApiRidderIQ.GetCompanys());
		}

		/// <summary>
		/// Get the list off all user within an administration
		/// </summary>
		/// <param name="company">administation to use</param>
		/// <returns></returns>
		[HttpGet()]
		[Route("Companies/{company}/users")]
		[ResponseType(typeof(List<string>))]
		public IHttpActionResult GetCompanysUsers(string company)
		{
			// Arguments checks
			if (string.IsNullOrWhiteSpace(company))
				return BadRequest();
			return Execute(() => ApiRidderIQ.GetCompanysUsers(company));
		}

		/// <summary>
		/// Ridder IQ Logged iner names
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("LoggedInUsers")]
		[ResponseType(typeof(List<string>))]
		public IHttpActionResult LoggedInUsers()
		{
			return Execute(() => ApiRidderIQ.LoggedInUsers());
		}

		/// <summary>
		/// Ridder IQ Logout
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("LogoutEverybodyForced")]
		[ResponseType(typeof(string))]
		[ApiExplorerSettings(IgnoreApi = true)]
		public IHttpActionResult LogoutEverybodyForced()
		{
			return Execute(() =>
			{
				ApiRidderIQ.LogoutEverybodyForced();
				return "OK";
			});
		}
	}
}