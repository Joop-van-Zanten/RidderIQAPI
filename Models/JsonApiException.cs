using System;
using System.Text.RegularExpressions;

namespace RidderIQAPI.Models
{
	/// <summary>
	/// JSON Api exception
	/// </summary>
	public class JsonApiException
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public JsonApiException()
		{
		}

		/// <summary>
		/// Constructor with the Exception
		/// </summary>
		/// <param name="ex"></param>
		public JsonApiException(Exception ex)
		{
			Message = ex.Message;
			InnerException = Regex.Replace(ex.StackTrace, @"\t|\n|\r", "");
		}

		/// <summary>
		/// Exception Message
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Inner Exception
		/// </summary>
		public string InnerException { get; set; }
	}
}