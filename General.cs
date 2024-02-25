using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RidderIQAPI
{
	public static class General
	{
		private const string randomChars = "abcdefghijklmonpqrstuvwqyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		private static readonly Random random = new Random();

		/// <summary>
		/// Create reandom string
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string RandomString(int length) => new string(Enumerable.Repeat(randomChars, length).Select(s => s[random.Next(s.Length)]).ToArray());

		public static Dictionary<string, string> ParseQueryString(string queryString)
		{
			if (string.IsNullOrEmpty(queryString))
				return default;
			var nvc = HttpUtility.ParseQueryString(queryString);
			return nvc.AllKeys.ToDictionary(k => k, k => nvc[k]);
		}
	}
}