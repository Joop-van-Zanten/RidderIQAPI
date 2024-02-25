using RidderIQAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RidderIQAPI.Api
{
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Get a list of all available administrations
		/// </summary>
		/// <returns></returns>
		public static List<RidderLoginCompany> GetCompanys()
		{
			try
			{
				return RidderSDK
					.GetAdministrations()
					.Select(x => new RidderLoginCompany()
					{
						CompanyAliases = x.CompanyAliases.ToList(),
						CompanyName = x.CompanyName,
						CompanyNumber = x.CompanyNumber,
						DatabaseName = x.DatabaseName,
						Users = x.Users
								.Select(y => y.UserName)
								.Where(y => !string.Equals("ADMINISTRATOR", y, StringComparison.InvariantCultureIgnoreCase))
								.OrderBy(y => y)
								.ToList(),
						Version = x.Version,
					})
					.OrderBy(x => x.DatabaseName)
					.ToList();
			}
			catch { return default; }
		}

		/// <summary>
		/// Get the list off all user within an administration
		/// </summary>
		/// <param name="company">administation to use</param>
		/// <returns></returns>
		public static List<string> GetCompanysUsers(string company)
		{
			try
			{
				// Get the requested administration
				var activeAdministration = RidderSDK
					.GetAdministrations()
					.FirstOrDefault(x => x.CompanyName == company);
				// Check if the database is found
				if (activeAdministration == null)
					throw new KeyNotFoundException();
				// Get all users withing the administration
				return activeAdministration
					.Users
					.Select(x => x.UserName)
					.Where(y => !string.Equals("ADMINISTRATOR", y, StringComparison.InvariantCultureIgnoreCase))
					.OrderBy(x => x)
					.ToList();
			}
			catch
			{
				return default;
			}
		}

		/// <summary>
		/// Get all LoggedIn users
		/// </summary>
		/// <returns></returns>
		public static List<string> LoggedInUsers() => SdkClients?.Select(x => x.Person.Username)?.OrderBy(x => x)?.ToList();

		/// <summary>
		/// Force LogOut for all users
		/// </summary>
		public static void LogoutEverybodyForced()
		{
			if (SdkClients == default)
				return;
			foreach (var item in SdkClients)
				item.Sdk.Logout();
			SdkClients.Clear();
		}
	}
}