using Ridder.Client.SDK;
using RidderIQAPI.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;

namespace RidderIQAPI.Api
{
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		///  Get user
		/// </summary>
		/// <param name="cookies">Cookies for getting the SDK Session</param>
		/// <returns></returns>
		public static RidderUserInfo GetUser(Collection<CookieHeaderValue> cookies)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			RidderUserInfo result = new RidderUserInfo(sdk.GetUserInfo());

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
		/// <returns></returns>
		public static bool LoggedIn(Collection<CookieHeaderValue> cookies)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies);
			// Validate user
			return sdk != null && sdk.LoggedinAndConnected;
		}

		/// <summary>
		/// Login
		/// </summary>
		/// <param name="cookies">Cookies for getting the SDK Session</param>
		/// <param name="person">Person to log in</param>
		/// <returns></returns>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public static CookieHeaderValue Login(Collection<CookieHeaderValue> cookies, RidderCredential person)
		{
			string tokenValue = person.Serialize().OpenSSLEncrypt();

			if (GetClient(cookies) == null)
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

				Register(new RidderCredentialToken(person, sdk));
			}

			var result = new CookieHeaderValue(CookieIqToken, tokenValue)
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
			RidderIQSDK sdk = GetClient(cookies);

			// If the client is not foud user is already logged out
			if (sdk == null)
				return default;

			// Unregister client (and logout)
			UnRegister(cookies);

			// Return result with a Cookie delete
			return new CookieHeaderValue(CookieIqToken, "")
			{
				Path = "/",
				Expires = DateTime.Now.AddDays(-30)
			};
		}
	}
}