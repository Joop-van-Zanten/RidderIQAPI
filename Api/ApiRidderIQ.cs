using Ridder.Client.SDK;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace RidderIQAPI.Api
{
	/// <summary>
	/// Ridder IQ API handler
	/// </summary>
	internal static partial class ApiRidderIQ
	{
		internal static class Core
		{
			#region config

			/// <summary>
			/// Maximum page size
			/// </summary>
			public const int MaxPageSize = 200;

			/// <summary>
			/// Ridder IQ Double precision
			/// </summary>
			public const int IQ_DoublePrecision = 8;

			#endregion config

			#region Sessions

			internal const string CookieIqToken = "RidderIQ_Token";

			internal static readonly RidderIQSDK RidderSDK = new RidderIQSDK();

			private static readonly List<RidderIQCredentialToken> sdkClients = new List<RidderIQCredentialToken>();
			internal static List<RidderIQCredentialToken> SdkClients => sdkClients.Where(x => x != null).ToList();

			internal static RidderIQSDK GetClient(Collection<CookieHeaderValue> cookies, bool throwException = false)
			{
				try
				{
					RidderIQCredential cred = GetIqCredential(cookies);
					if (cred is null)
					{
						if (throwException)
							throw new UnauthorizedAccessException();
						return default;
					}
					RidderIQSDK result = GetClient(cred);
					if (result == default)
					{
						result = new RidderIQSDK();
						// Try to 're-use' existing Ridder SDK connection
						ISDKResult loginResult = result.ConnectToPersistedSession(cred.Username, cred.Password, cred.Company);

						// Check if the user is succesfully LoggedIn using Persisted
						if (loginResult != null && loginResult.HasError)
						{
							// Try to create new connection using the credentials
							loginResult = result.Login(cred.Username, cred.Password, cred.Company);
							// Check if connection succeeded
							if (loginResult != null && loginResult.HasError)
								// Login Failed
								throw new UnauthorizedAccessException();
							result.PersistSession();
						}
						if (loginResult.HasError)
							throw new UnauthorizedAccessException();

						Register(new RidderIQCredentialToken(cred, result));
					}
					return result;
				}
				catch { }
				if (throwException)
					throw new UnauthorizedAccessException();
				return default;
			}

			internal static void Register(RidderIQCredentialToken token)
			{
				if (!sdkClients.Any(x => x.Equals(token)))
					sdkClients.Add(token);
			}

			internal static void UnRegister(Collection<CookieHeaderValue> cookies)
			{
				RidderIQCredential cred = GetIqCredential(cookies);
				if (cred is null)
					return;
				RidderIQCredentialToken item = sdkClients.FirstOrDefault(x => x.Person == cred);
				if (item != null)
				{
					lock (sdkClients)
					{
						item.Dispose();
						sdkClients.Remove(item);
					}
				}
			}

			private static RidderIQSDK GetClient(RidderIQCredential cred) => sdkClients.Where(x => x != null).FirstOrDefault(x => x.Person == cred)?.Sdk;

			internal static RidderIQCredential GetIqCredential(Collection<CookieHeaderValue> cookies)
			{
				try
				{
					foreach (var cookieHeader in cookies)
					{
						if (cookieHeader.Cookies.Any(x => x.Name == CookieIqToken))
						{
							var cValue = cookieHeader.Cookies.First(x => x.Name == CookieIqToken)?.Value;

							if (!cValue.IsBase64String())
								cValue = HttpUtility.UrlDecode(cValue);
							string decrypted = cValue?.OpenSSLDecrypt();
							if (decrypted == default)
								return default;
							return decrypted?.DeserializeJSON<RidderIQCredential>() ?? (decrypted?.DeserializeXML<RidderIQCredential>());
						}
					}

					return default;
				}
				catch (Exception)
				{
					return default;
				}
			}

			#endregion Sessions
		}
	}
}