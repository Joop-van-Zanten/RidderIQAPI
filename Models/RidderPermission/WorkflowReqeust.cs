using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace RidderIQAPI.Models.RidderPermission
{
	/// <summary>
	/// Ridder permission workflow request
	/// </summary>
	public class WorkflowReqeust
	{
		/// <summary>
		/// Ridder permission workflow request constructor
		/// </summary>
		/// <param name="scope">Designer scope</param>
		/// <param name="workflowEvent">Workflow event</param>
		public WorkflowReqeust(RidderDesignerScope scope, Guid workflowEvent)
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
		public RidderDesignerScope Scope { get; set; }
	}
}