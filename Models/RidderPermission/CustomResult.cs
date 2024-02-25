using Newtonsoft.Json;
using System;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission custom result
	/// </summary>
	public class CustomResult
	{
		/// <summary>
		/// Ridder permission custom result constructor
		/// </summary>
		/// <param name="customPermission">Custom</param>
		/// <param name="result">Result</param>
		public CustomResult(Guid customPermission, bool result)
		{
			CustomPermission = customPermission;
			Result = result;
		}

		/// <summary>
		/// Permission ID
		/// </summary>
		[JsonProperty("customPermission")]
		public Guid CustomPermission { get; set; }

		/// <summary>
		/// Permission result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }
	}
}