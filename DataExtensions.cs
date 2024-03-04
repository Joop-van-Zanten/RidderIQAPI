using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Ridder.Common.ADO;
using System.Collections.ObjectModel;
using Ridder.Client.SDK.SDKDataAcces;
using System.Diagnostics;

namespace RidderIQAPI
{
	public static class DataExtensions
	{
		private const string randomChars = "abcdefghijklmonpqrstuvwqyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		private static readonly Random random = new Random();

		[DebuggerStepThrough()]
		public static string RandomString(this int length) => new string(Enumerable.Repeat(randomChars, length).Select(s => s[random.Next(s.Length)]).ToArray());

		/// <summary>
		/// Check if a string is Base64 format
		/// </summary>
		/// <param name="input">Input string to be checked</param>
		/// <returns></returns>
		[DebuggerStepThrough()]
		public static bool IsBase64String(this string input) => (input.Trim().Length % 4 == 0) && Regex.IsMatch(input.Trim(), @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

		[DebuggerStepThrough()]
		public static HttpResponseMessage AddCoockie(this HttpResponseMessage response, CookieHeaderValue cookie)
		{
			response.Headers.AddCookies(new[] { cookie });
			return response;
		}

		public static HttpResponseMessage CreateJsonResponse(this ApiController c, object json, System.Net.HttpStatusCode code = System.Net.HttpStatusCode.OK)
		{
			// Create response
			var response = c.Request.CreateResponse(code, json);
			// Set response media to JSON
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			List<CookieHeaderValue> cookieHeaders = new List<CookieHeaderValue>();
			foreach (CookieHeaderValue cookieHeader in c.Request.Headers.GetCookies())
			{
				foreach (var cookie in cookieHeader.Cookies)
				{
					cookieHeaders.Add(
						new CookieHeaderValue(cookie.Name, cookie.Value)
						{
							Path = "/",
							Expires = DateTime.Now.AddDays(90)
						}
					);
				}
			}
			if (cookieHeaders.Count > 0)
			{
				response.Headers.AddCookies(cookieHeaders);
			}
			return response;
		}

		/// <summary>
		/// SSL Decrypt a string to readable content
		/// </summary>
		/// <param name="encrypted">hashed string to be converted to text</param>
		/// <returns></returns>
		[DebuggerStepThrough()]
		public static string OpenSSLDecrypt(this string encrypted)
		{
			try
			{
				return Cryptography.OpenSSLDecrypt(encrypted);
			}
			catch { return default; }
		}

		/// <summary>
		/// SSL Encrypt readable content to unreadable content
		/// </summary>
		/// <param name="text">text to be converted to a hashed string</param>
		/// <returns></returns>
		[DebuggerStepThrough()]
		public static string OpenSSLEncrypt(this string text)
		{
			try
			{
				return Cryptography.OpenSSLEncrypt(text);
			}
			catch { return default; }
		}

		/// <summary>
		/// Serialise a Type to xml
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">Object to be Encrypted</param>
		/// <returns></returns>
		public static string Serialize<T>(this T obj) where T : class
		{
			try
			{
				// Create result
				string result = default;

				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add("", "");

				// Create string writer
				using (var sww = new StringWriter())
				{
					// Create an XML writer
					using (XmlWriter writer = XmlWriter.Create(sww))
					{
						new XmlSerializer(typeof(T)).Serialize(writer, obj, ns);
						result = sww.ToString();
					}
				}
				// return result
				return result;
			}
			catch { return default; }
		}

		/// <summary>
		/// Deserialise XML to a Type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xml">string to be Decrypted</param>
		/// <returns></returns>
		public static T Deserialize<T>(this string xml) where T : class
		{
			try
			{
				// Create result
				T result = default;
				using (TextReader reader = new StringReader(xml))
				{
					result = new XmlSerializer(typeof(T)).Deserialize(reader) as T;
				}
				// return result
				return result;
			}
			catch { return default; }
		}

		/// <summary>
		/// Get a Field with the requested type
		/// </summary>
		/// <typeparam name="T">Requested result Type</typeparam>
		/// <param name="rec">Recordset</param>
		/// <param name="key">Field name</param>
		/// <returns></returns>
		public static T GetField<T>(this BaseRecordset rec, string key)
		{
			try
			{
				if ((object)rec.GetField(key) == DBNull.Value)
					return default;
				var fieldValue = rec.GetField(key.ToUpper()).Value;
				if (fieldValue is DBNull)
					return default;
				return (T)fieldValue;
			}
			catch
			{
				return default;
			}
		}

		[DebuggerStepThrough()]
		public static Collection<CookieHeaderValue> GetCookies(this HttpRequestMessage Request) => Request.Headers.GetCookies();

		[DebuggerStepThrough()]
		public static IEnumerable<SDKRecordset> AsEnumerable(this SDKRecordset records)
		{
			records.MoveFirst();
			while (!records.EOF)
			{
				yield return records;
				records.MoveNext();
			}
		}

		[DebuggerStepThrough()]
		public static Dictionary<string, object> ToDictionary(this SDKRecordset records)
		{
			// Create the result
			Dictionary<string, object> result = new Dictionary<string, object>();

			List<string> keys = new List<string>();
			foreach (ADODB.Field item in records.Fields)
				keys.Add(item.Name);

			// Find the primary key, set it as the first item
			var pk = keys.FirstOrDefault(x => x.StartsWith("PK_", StringComparison.InvariantCultureIgnoreCase));
			if (pk != default)
				result.Add(pk, records.GetField(pk).Value);

			// Loop through all remaining keys orderred by the key
			foreach (var key in keys.Where(x => !x.StartsWith("PK_", StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x))
			{
				// Add the field to the result
				result.Add(key, records.GetField(key).Value);
			}

			// Return the result
			return result;
		}
	}
}