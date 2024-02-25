using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace RidderIQAPI.Models
{
	/// <summary>
	/// Ridder login credentials
	/// </summary>
	public class RidderCredential : IEquatable<RidderCredential>
	{
		/// <summary>
		/// Company
		/// </summary>
		[JsonProperty("company")]
		[XmlAttribute]
		public string Company { get; set; }

		/// <summary>
		/// Username
		/// </summary>
		[JsonProperty("username")]
		[XmlAttribute]
		public string Username { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		[JsonProperty("password")]
		[XmlAttribute]
		public string Password { get; set; }

		public bool Equals(RidderCredential other)
		{
			return
				Company.Equals(other.Company, StringComparison.InvariantCultureIgnoreCase) &&
				Username.Equals(other.Username, StringComparison.InvariantCultureIgnoreCase) &&
				Password.Equals(other.Password, StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool operator ==(RidderCredential lhs, RidderCredential rhs)
		{
			if (lhs is null && rhs is null)
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(RidderCredential lhs, RidderCredential rhs) => !(lhs == rhs);
	}
}