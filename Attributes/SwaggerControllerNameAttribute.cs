using System;

namespace RidderIQAPI.Attributes
{
	/// <summary>
	/// Swagger Controller name attribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SwaggerControllerNameAttribute : Attribute
	{
		/// <summary>
		/// Get the controller name (Group)
		/// </summary>
		public string ControllerName { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public SwaggerControllerNameAttribute(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			ControllerName = name;
		}
	}
}