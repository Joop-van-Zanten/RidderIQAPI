using RidderIQAPI.Api.ApiRidderIQ;
using RidderIQAPI.Attributes;
using RidderIQAPI.Models.RidderIQ;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace RidderIQAPI.Controllers.RidderIQ
{
	/// <summary>
	/// API Anonymous controller
	/// </summary>
	[SwaggerControllerName("Ridder/Anonymous")]
	[RoutePrefix("api/anonymous")]
	public class RidderIQAnonymousController : ApiBase
	{
		/// <summary>
		/// Get a list of all available administrations
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("Companies")]
		[ResponseType(typeof(List<RidderIQLoginCompany>))]
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
		[ResponseType(typeof(Dictionary<string, List<string>>))]
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
		[ResponseType(typeof(List<string>))]
		[ApiExplorerSettings(IgnoreApi = true)]
		public IHttpActionResult LogoutEverybodyForced()
		{
			return Execute(() => ApiRidderIQ.LogoutEverybodyForced());
		}
	}
}