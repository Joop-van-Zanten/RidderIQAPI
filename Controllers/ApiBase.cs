using RidderIQAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;

namespace RidderIQAPI.Controllers
{
	/// <summary>
	/// Base API controller
	/// </summary>
	public class ApiBase : ApiController
	{
		/// <summary>
		/// Execute an Action and return an ActionResult
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public IHttpActionResult Execute(Func<ActionResult, object> action)
		{
			try
			{
				// Create action Result
				var ar = new ActionResult();
				// Execute the action
				var jsonData = action(ar);
				// Create the Json response message
				var message = this.CreateJsonResponse(jsonData);
				// Add cookies if present
				foreach (var item in ar.Cookies)
					message.AddCoockie(item);
				// Return the Json message
				return ResponseMessage(message);
			}
			catch (KeyNotFoundException ex)
			{
				return ResponseMessage(this.CreateJsonResponse(new JsonApiException(ex), HttpStatusCode.NotFound));
			}
			catch (UnauthorizedAccessException ex)
			{
				return ResponseMessage(this.CreateJsonResponse(new JsonApiException(ex), HttpStatusCode.Unauthorized));
			}
			catch (Exception ex)
			{
				return ResponseMessage(this.CreateJsonResponse(new JsonApiException(ex), HttpStatusCode.InternalServerError));
			}
		}

		/// <summary>
		/// Execute an Action and return an ActionResult
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public IHttpActionResult Execute(Func<object> action)
		{
			try
			{
				return ResponseMessage(this.CreateJsonResponse(action()));
			}
			catch (KeyNotFoundException ex)
			{
				return ResponseMessage(this.CreateJsonResponse(new JsonApiException(ex), HttpStatusCode.NotFound));
			}
			catch (UnauthorizedAccessException ex)
			{
				return ResponseMessage(this.CreateJsonResponse(new JsonApiException(ex), HttpStatusCode.Unauthorized));
			}
			catch (Exception ex)
			{
				return ResponseMessage(this.CreateJsonResponse(new JsonApiException(ex), HttpStatusCode.InternalServerError));
			}
		}

		/// <summary>
		/// Action result
		/// </summary>
		public class ActionResult
		{
			/// <summary>
			/// Action Cookies
			/// </summary>
			public List<CookieHeaderValue> Cookies = new List<CookieHeaderValue>();

			/// <summary>
			/// Add a cookie to the headers
			/// </summary>
			/// <param name="cookie"></param>
			/// <returns></returns>
			public void Add(CookieHeaderValue cookie) => Cookies.Add(cookie);
		}
	}
}