using Ridder.Client.SDK;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;

namespace RidderIQAPI.Api
{
	/// <summary>
	/// Ridder IQ API handler
	/// </summary>
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Ridder IQ API User handler
		/// </summary>
		internal static class User
		{
			/// <summary>
			///  Get user
			/// </summary>
			/// <param name="cookies">Cookies for getting the SDK Session</param>
			/// <returns></returns>
			public static RidderIQUserInfo GetUser(Collection<CookieHeaderValue> cookies)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				RidderIQUserInfo result = new RidderIQUserInfo(sdk.GetUserInfo());

				var user = sdk.CreateRecordset("R_USER", null, $"PK_R_USER = {result.CurrentUserId}", null);
				user.MoveFirst();
				result.UserData = user.ToDictionary();

				// Employee is not required
				if (user.GetField<int>("FK_EMPLOYEE") != default)
				{
					var employee = sdk.CreateRecordset("R_EMPLOYEE", null, $"PK_R_EMPLOYEE = {user.GetField<int>("FK_EMPLOYEE")}", null);
					employee.MoveFirst();
					result.EmployeeData = employee.ToDictionary();
				}

				// Return the result
				return result;
			}

			/// <summary>
			/// Logged in status
			/// </summary>
			/// <param name="cookies">Cookies for getting the SDK Session</param>
			/// <param name="validateCookieWithSession">Validate the client userinfo with the cookie credentials</param>
			/// <returns></returns>
			public static bool LoggedIn(Collection<CookieHeaderValue> cookies, bool validateCookieWithSession)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies);

				bool result = false;

				if (sdk != default)
				{
					result = sdk.LoggedinAndConnected;
				}

				if (result && validateCookieWithSession)
				{
					var cred = Core.GetIqCredential(cookies);
					var user = sdk.GetUserInfo();

					if (
						cred.Username != user.CurrentUserName ||
						cred.Company != user.CompanyName
					)
						result = false;
				}

				return result;
			}

			/// <summary>
			/// Login
			/// </summary>
			/// <param name="cookies">Cookies for getting the SDK Session</param>
			/// <param name="person">Person to log in</param>
			/// <returns></returns>
			/// <exception cref="UnauthorizedAccessException"></exception>
			public static CookieHeaderValue Login(Collection<CookieHeaderValue> cookies, RidderIQCredential person)
			{
				string tokenValue = person.SerializeJSON().OpenSSLEncrypt();

				if (Core.GetClient(cookies) == null)
				{
					// Create new SDK client
					RidderIQSDK sdk = new RidderIQSDK();
					// Try to 're-use' existing Ridder SDK connection
					ISDKResult loginResult = sdk.ConnectToPersistedSession(person.Username, person.Password, person.Company);

					// Check if the user is succesfully LoggedIn using Persisted
					if (loginResult != null && loginResult.HasError)
					{
						// Try to create new connection using the credentials
						loginResult = sdk.Login(person.Username, person.Password, person.Company);
						// Check if connection succeeded
						if (loginResult != null && loginResult.HasError)
							// Login Failed
							throw new UnauthorizedAccessException(string.Join("\r\n", loginResult.Messages.Select(x => $"{x.MessageType}: {x.Message}")));
						sdk.PersistSession();
					}
					if (loginResult.HasError)
						throw new UnauthorizedAccessException(string.Join("\r\n", loginResult.Messages.Select(x => $"{x.MessageType}: {x.Message}")));

					Core.Register(new RidderIQCredentialToken(person, sdk));
				}

				var result = new CookieHeaderValue(Core.CookieIqToken, tokenValue)
				{
					Path = "/",
					Expires = DateTime.Now.AddDays(30)
				};
				return result;
			}

			/// <summary>
			/// Logged in status
			/// </summary>
			/// <param name="cookies">Cookies for getting the SDK Session</param>
			/// <returns></returns>
			public static CookieHeaderValue Logout(Collection<CookieHeaderValue> cookies)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies);

				// If the client is not foud user is already logged out
				if (sdk == null)
					return default;

				// Unregister client (and logout)
				Core.UnRegister(cookies);

				// Return result with a Cookie delete
				return new CookieHeaderValue(Core.CookieIqToken, "")
				{
					Path = "/",
					Expires = DateTime.Now.AddDays(-30)
				};
			}
		}
	}
}