using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ permission workflow request
	/// </summary>
	public class RidderIQPermissionWorkflowReqeust
	{
		/// <summary>
		/// RidderIQ permission workflow request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="workflowEvent">Workflow event</param>
		public RidderIQPermissionWorkflowReqeust(RidderIQDesignerScope scope, Guid workflowEvent)
		{
			Scope = scope;
			WorkflowEvent = workflowEvent;
		}

		/// <summary>
		/// Workflow
		/// </summary>
		[JsonProperty("workflowEvent")]
		[JsonRequired]
		public Guid WorkflowEvent { get; set; }

		/// <summary>
		/// Designer scope
		/// </summary>
		[JsonProperty("scope")]
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonRequired]
		public RidderIQDesignerScope Scope { get; set; }
	}
}