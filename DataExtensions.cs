using Ridder.Client.SDK.SDKDataAcces;
using Ridder.Common.ADO;
using RidderIQAPI.Api;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace RidderIQAPI
{
	/// <summary>
	/// Data extensions
	/// </summary>
	public static class DataExtensions
	{
		/// <summary>
		/// Characters for the random string generator
		/// </summary>
		public const string randomChars = "abcdefghijklmonpqrstuvwqyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		private static readonly Random random = new Random();

		/// <summary>
		/// Add a Cookkie to the response headers
		/// </summary>
		/// <param name="response"></param>
		/// <param name="cookie"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static HttpResponseMessage AddCoockie(this HttpResponseMessage response, CookieHeaderValue cookie)
		{
			// Add the Cookie
			response.Headers.AddCookies(new[] { cookie });
			// Return the response
			return response;
		}

		/// <summary>
		/// SDK records as Ienumerable
		/// </summary>
		/// <param name="records"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static IEnumerable<SDKRecordset> AsEnumerable(this SDKRecordset records)
		{
			records.MoveFirst();
			while (!records.EOF)
			{
				yield return records;
				records.MoveNext();
			}
		}

		/// <summary>
		/// Create a JSON response
		/// </summary>
		/// <param name="c"></param>
		/// <param name="json"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		//[DebuggerStepThrough]
		public static HttpResponseMessage CreateJsonResponse(this ApiController c, object json, System.Net.HttpStatusCode code = System.Net.HttpStatusCode.OK)
		{
			// Create response
			var response = c.Request.CreateResponse(code, json);

			// Set response media to JSON
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			// Build cookies
			List<CookieHeaderValue> cookieHeaders = new List<CookieHeaderValue>();
			foreach (CookieHeaderValue cookieHeader in c.Request.Headers.GetCookies())
			{
				// Loop through all current cookies
				foreach (var cookie in cookieHeader.Cookies)
				{
					// Get the value
					string cookieValue = cookie.Value;
					// Check for a specific name
					if (cookie.Name == ApiRidderIQ.Core.CookieIqToken)
					{
						// Get the SDK Credentials
						RidderIQCredential client = ApiRidderIQ.Core.GetIqCredential(c.Request.Headers.GetCookies());
						if (client != null)
							// Override the value with a new serialized value
							cookieValue = client.SerializeJSON().OpenSSLEncrypt();
					}
					// Add an item to the list, extending bij 90 days
					cookieHeaders.Add(
						new CookieHeaderValue(cookie.Name, cookieValue)
						{
							Path = "/",
							Expires = DateTime.Now.AddDays(90)
						}
					);
				}
			}
			// Check for coockies
			if (cookieHeaders.Count > 0)
				// Add the cookies to the Headers
				response.Headers.AddCookies(cookieHeaders);

			// Return the response
			return response;
		}

		/// <summary>
		/// Deserialise XML to a Type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json">string to be Decrypted</param>
		/// <returns></returns>
		public static T DeserializeJSON<T>(this string json) where T : class
		{
			try
			{
				// Deserialise the string
				return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
			}
			catch { return default; }
		}

		/// <summary>
		/// Deserialise XML to a Type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xml">string to be Decrypted</param>
		/// <returns></returns>
		public static T DeserializeXML<T>(this string xml) where T : class
		{
			try
			{
				// Create result
				T result = default;
				// Deserialse the string
				using (TextReader reader = new StringReader(xml))
					result = new XmlSerializer(typeof(T)).Deserialize(reader) as T;
				// return result
				return result;
			}
			catch { return default; }
		}

		/// <summary>
		/// Get cookies from the Reqeust message
		/// </summary>
		/// <param name="Request"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static Collection<CookieHeaderValue> GetCookies(this HttpRequestMessage Request) => Request.Headers.GetCookies();

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

		/// <summary>
		/// Check if a string is Base64 format
		/// </summary>
		/// <param name="input">Input string to be checked</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static bool IsBase64String(this string input) => (input.Trim().Length % 4 == 0) && Regex.IsMatch(input.Trim(), @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

		/// <summary>
		/// SSL Decrypt a string to readable content
		/// </summary>
		/// <param name="encrypted">hashed string to be converted to text</param>
		/// <returns></returns>
		//[DebuggerStepThrough]
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
		[DebuggerStepThrough]
		public static string OpenSSLEncrypt(this string text)
		{
			try
			{
				return Cryptography.OpenSSLEncrypt(text);
			}
			catch { return default; }
		}

		/// <summary>
		/// Build a random string using 'randomChars'
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static string RandomString(this int length) => new string(Enumerable.Repeat(randomChars, length).Select(s => s[random.Next(s.Length)]).ToArray());

		/// <summary>
		/// Deserialize an object to JSON
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string SerializeJSON<T>(this T obj) where T : class
		{
			try
			{
				// Serialize the object to a JSON string
				return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			}
			catch { return default; }
		}

		/// <summary>
		/// Serialise a Type to xml
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">Object to be Encrypted</param>
		/// <returns></returns>
		public static string SerializeXML<T>(this T obj) where T : class
		{
			try
			{
				// Create result
				string result = default;

				// No namespaces
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add("", "");

				using (var sww = new StringWriter())
				using (XmlWriter writer = XmlWriter.Create(sww))
				{
					new XmlSerializer(typeof(T)).Serialize(writer, obj, ns);
					result = sww.ToString();
				}

				// return result
				return result;
			}
			catch { return default; }
		}

		/// <summary>
		/// SDK record to Dictionary
		/// </summary>
		/// <param name="records"></param>
		/// <returns></returns>
		//[DebuggerStepThrough]
		public static Dictionary<string, object> ToDictionary(this SDKRecordset records)
		{
			string tablename = null;
			string fkr = null;

			// Create the result
			Dictionary<string, object> result = new Dictionary<string, object>();

			List<string> keys = new List<string>();
			foreach (ADODB.Field item in records.Fields)
			{
				string key = item.Name.ToUpper();
				if (key.StartsWith("PK_"))
				{
					tablename = key.Remove(0, 3);
					fkr = $"FK_{tablename}".ToUpper();
				}
				keys.Add(key);
			}

			// Find the primary key, set it as the first item
			var pk = keys.FirstOrDefault(x => x.StartsWith("PK_", StringComparison.InvariantCultureIgnoreCase));
			if (pk != default)
				result.Add(pk, records.GetField(pk).Value);

			// Loop through all remaining keys orderred by the key
			foreach (var key in keys.Where(x => !x.StartsWith("PK_", StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x))
			{
				if (key == fkr)
					continue;

				// Add the field to the result
				result.Add(key, records.GetField(key).Value);
			}

			// Return the result
			return result;
		}
	}
}