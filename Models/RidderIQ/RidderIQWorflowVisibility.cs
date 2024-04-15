using System;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ Workflow visibility
	/// </summary>
	public class RidderIQWorflowVisibility
	{
		/// <summary>
		/// Action
		/// </summary>
		public string Action { get; set; }

		/// <summary>
		/// Caption
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// Event
		/// </summary>
		public Guid Event { get; set; }

		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// SequenceNumber
		/// </summary>
		public int SequenceNumber { get; set; }

		/// <summary>
		/// Workflow state is valid
		/// </summary>
		public bool State { get; internal set; } = true;

		/// <summary>
		/// Designer Scope
		/// </summary>
		public RidderIQDesignerScope Scope { get; internal set; }
	}
}