using Newtonsoft.Json;
using System.Xml.Serialization;

namespace RidderIQAPI
{
	/// <summary>
	/// WorkflowEvent parameter
	/// </summary>
	public class RidderWorkFlowEventParameter
	{
		/// <summary>
		/// Workflow parameter Type
		/// </summary>
		[JsonProperty("type")]
		[XmlAttribute]
		public RidderWorkflowParamaterType Type { get; set; }

		/// <summary>
		/// Workflow parameter Key
		/// </summary>
		[JsonProperty("key")]
		[XmlAttribute]
		public string Key { get; set; }

		/// <summary>
		/// Workflow parameter Value
		/// </summary>
		[JsonProperty("value")]
		[XmlAttribute]
		public object Value { get; set; }
	}
}