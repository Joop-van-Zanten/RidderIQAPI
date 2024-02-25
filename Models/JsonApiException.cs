using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RidderIQAPI.Models
{
	public class JsonApiException
	{
		public JsonApiException()
		{
		}

		public JsonApiException(Exception ex)
		{
			Message = ex.Message;
			InnerException = Regex.Replace(ex.StackTrace, @"\t|\n|\r", "");
		}

		public string Message { get; set; }
		public string InnerException { get; set; }
	}
}