using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RidderIQAPI.Api.ApiRidderIQ
{
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Get a list of all available administrations
		/// </summary>
		/// <returns></returns>
		public static List<RidderIQLoginCompany> GetCompanys()
		{
			try
			{
				return RidderSDK
					.GetAdministrations()
					.Select(x => new RidderIQLoginCompany()
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
					.FirstOrDefault(x => x.CompanyName == company)
					?? throw new KeyNotFoundException();
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
		public static Dictionary<string, List<string>> LoggedInUsers()
		{
			return SdkClients
				.GroupBy(x => x.Person.Username)
				.OrderBy(x => x.Key)
				.ToDictionary(
					k => k.Key,
					v => v.Select(y => y.Person.Company).OrderBy(x => x).ToList()
				);
		}

		/// <summary>
		/// Force LogOut for all users
		/// </summary>
		public static List<string> LogoutEverybodyForced()
		{
			if (SdkClients == default)
				return default;
			List<string> result = SdkClients.Select(x => x.Person.Username).ToList();
			lock (SdkClients)
			{
				SdkClients.ForEach(x => x.Sdk.Logout());
				SdkClients.Clear();
			}
			return result;
		}
	}
}