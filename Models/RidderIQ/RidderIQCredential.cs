using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ login credentials
	/// </summary>
	[XmlRoot("RidderCredential")]
	public class RidderIQCredential : IEquatable<RidderIQCredential>
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

		/// <summary>
		/// Check if an object equals another
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(RidderIQCredential other)
		{
			return
				Company.Equals(other.Company, StringComparison.InvariantCultureIgnoreCase) &&
				Username.Equals(other.Username, StringComparison.InvariantCultureIgnoreCase) &&
				Password.Equals(other.Password, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Get the hascode of the object
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() => base.GetHashCode();

		/// <summary>
		/// Check ifn an object Equals another object (default)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) => base.Equals(obj);

		/// <summary>
		/// Check if lhs equals rhs
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static bool operator ==(RidderIQCredential lhs, RidderIQCredential rhs)
		{
			if (lhs is null && rhs is null)
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Check if lhs not equals rhs
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static bool operator !=(RidderIQCredential lhs, RidderIQCredential rhs) => !(lhs == rhs);
	}
}