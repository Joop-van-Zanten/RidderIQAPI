using Newtonsoft.Json;
using Ridder.Client.SDK.SDKParameters;
using System.Collections.Generic;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ User info
	/// </summary>
	public class RidderIQUserInfo
	{
		/// <summary>
		/// Public constructor
		/// </summary>
		public RidderIQUserInfo()
		{ }

		/// <summary>
		/// SDK userinfo constructor
		/// </summary>
		/// <param name="sDKUser"></param>
		public RidderIQUserInfo(SDKUser sDKUser)
		{
			CurrentUserId = sDKUser.CurrentUserId;
			DatabaseName = sDKUser.DatabaseName;
			CompanyName = sDKUser.CompanyName;
			CurrentUserName = sDKUser.CurrentUserName;
			DatabaseVersion = sDKUser.DatabaseVersion;
		}

		/// <summary>
		/// Company name
		/// </summary>
		[JsonProperty("companyName")]
		public string CompanyName { get; set; }

		/// <summary>
		/// Current User ID
		/// </summary>
		[JsonProperty("currentUserId")]
		public int CurrentUserId { get; set; }

		/// <summary>
		/// Current username
		/// </summary>
		[JsonProperty("currentUserName")]
		public string CurrentUserName { get; set; }

		/// <summary>
		/// Current Database name
		/// </summary>
		[JsonProperty("databaseName")]
		public string DatabaseName { get; set; }

		/// <summary>
		/// Current Database version
		/// </summary>
		[JsonProperty("databaseVersion")]
		public string DatabaseVersion { get; set; }

		/// <summary>
		/// Get the Employee linked data (R_EMPLOYEE)
		/// </summary>
		[JsonProperty("EmployeeData")]
		public Dictionary<string, object> EmployeeData { get; set; } = new Dictionary<string, object>();

		/// <summary>
		/// Get the user data (R_USER)
		/// </summary>
		[JsonProperty("UserData")]
		public Dictionary<string, object> UserData { get; set; } = new Dictionary<string, object>();

		/// <summary>
		/// Get assigned role names
		/// </summary>
		[JsonProperty("Roles")]
		public List<string> Roles { get; set; } = new List<string>();
	}
}