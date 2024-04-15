using Newtonsoft.Json;
using System.Xml.Serialization;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// Ridder Add Document object
	/// </summary>
	public class RidderIQAddDocument
	{
		/// <summary>
		/// Table
		/// </summary>
		[JsonProperty("table")]
		[XmlAttribute("table")]
		public string Table { get; set; }

		/// <summary>
		/// Record ID
		/// </summary>
		[JsonProperty("recordID")]
		[XmlAttribute("recordID")]
		public int RecordID { get; set; }

		/// <summary>
		/// File
		/// </summary>
		[JsonProperty("file")]
		[XmlAttribute("file")]
		public string File { get; set; }

		/// <summary>
		/// Revision
		/// </summary>
		[JsonProperty("revision")]
		[XmlAttribute("revision")]
		public string Revision { get; set; }

		/// <summary>
		/// Description
		/// </summary>
		[JsonProperty("description")]
		[XmlAttribute("description")]
		public string Description { get; set; }

		/// <summary>
		/// Dave as location
		/// </summary>
		[JsonProperty("saveAsLocation")]
		[XmlAttribute("saveAsLocation")]
		public bool SaveAsLocation { get; set; } = true;
	}
}