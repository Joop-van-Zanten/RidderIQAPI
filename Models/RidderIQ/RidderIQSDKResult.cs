using Ridder.Common;
using System.Collections.Generic;
using System.ServiceModel;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ SDK Result
	/// </summary>
	public class RidderIQSDKResult
	{
		/// <summary>
		/// Create new Ridder IQ SDK result
		/// </summary>
		/// <param name="hasError">Error present?</param>
		/// <param name="primaryKey">Primary key of the record</param>
		public RidderIQSDKResult(bool hasError, object primaryKey)
		{
			HasError = hasError;
			PrimaryKey = primaryKey;
		}

		/// <summary>
		/// Constructor using the SDK result
		/// </summary>
		/// <param name="result"></param>
		public RidderIQSDKResult(Ridder.Client.SDK.ISDKResult result)
		{
			HasError = result.HasError;
			PrimaryKey = result?.PrimaryKey;
			PrimaryKeys = result?.PrimaryKeys;
			if (result.Messages != null)
			{
				Messages = new List<ResultMessage>();

				foreach (var item in result.Messages)
				{
					Messages.Add(new ResultMessage(item));
				}
			}

			ResultString = result.GetResult();
		}

		/// <summary>
		/// Constructor using an Exception
		/// </summary>
		/// <param name="ex2"></param>
		public RidderIQSDKResult(FaultException<TranslationMessageInfo> ex2)
		{
			HasError = true;
			ResultString = ex2.Message;
			Messages = new List<ResultMessage>()
			{
				{
					new ResultMessage(MessageTypes.Error, ex2.Message)
				}
			};
		}

		/// <summary>
		/// Check if the transaction has an error
		/// </summary>
		public bool HasError { get; }

		/// <summary>
		/// Check if the transaction is successful
		/// </summary>
		public bool Success => !HasError;

		/// <summary>
		/// Get the PK
		/// </summary>
		public object PrimaryKey { get; }

		/// <summary>
		/// Get a list of all messages
		/// </summary>
		public List<ResultMessage> Messages { get; }

		/// <summary>
		/// Get the PK's
		/// </summary>
		public object[] PrimaryKeys { get; }

		/// <summary>
		/// Result string
		/// </summary>
		public string ResultString { get; }

		/// <summary>
		/// Result Message
		/// </summary>
		public class ResultMessage
		{
			/// <summary>
			/// Default constructor
			/// </summary>
			public ResultMessage()
			{ }

			/// <summary>
			/// Constructor using the SDK
			/// </summary>
			/// <param name="item"></param>
			public ResultMessage(Ridder.Client.SDK.ResultMessage item)
			{
				MessageType = (MessageTypes)item.MessageType;
				Message = item.Message;
			}

			/// <summary>
			/// New message given type and string
			/// </summary>
			/// <param name="messageType"></param>
			/// <param name="message"></param>
			public ResultMessage(MessageTypes messageType, string message)
			{
				MessageType = messageType;
				Message = message;
			}

			/// <summary>
			/// Message type
			/// </summary>
			public MessageTypes MessageType { get; }

			/// <summary>
			/// Message string
			/// </summary>
			public string Message { get; }
		}

		/// <summary>
		/// Message types
		/// </summary>
		public enum MessageTypes
		{
			/// <summary>
			/// Error
			/// </summary>
			Error,

			/// <summary>
			/// Warning
			/// </summary>
			Warning,

			/// <summary>
			/// Message
			/// </summary>
			Message
		}
	}
}