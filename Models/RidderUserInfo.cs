using Newtonsoft.Json;
using Ridder.Client.SDK.SDKParameters;
using System.Collections.Generic;

namespace RidderIQAPI
{
	/// <summary>
	/// Ridder IQ Userinfo
	/// </summary>
	public class RidderUserInfo
	{
		/// <summary>
		/// Public constructor
		/// </summary>
		public RidderUserInfo()
		{
		}

		/// <summary>
		/// SDK userinfo constructor
		/// </summary>
		/// <param name="sDKUser"></param>
		public RidderUserInfo(SDKUser sDKUser)
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

		public Dictionary<string, object> EmployeeData { get; set; } = new Dictionary<string, object>();
		public Dictionary<string, object> UserData { get; set; } = new Dictionary<string, object>();
	}
}