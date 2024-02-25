using Ridder.Common;
using System.Collections.Generic;
using System.ServiceModel;

namespace RidderIQAPI
{
	public class RidderSDKResult
	{
		public RidderSDKResult(Ridder.Client.SDK.ISDKResult result)
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

		public RidderSDKResult(FaultException<TranslationMessageInfo> ex2)
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

		public bool HasError { get; }
		public bool Success => !HasError;
		public object PrimaryKey { get; }

		public List<ResultMessage> Messages { get; }

		public object[] PrimaryKeys { get; }

		public string ResultString { get; }

		public class ResultMessage
		{
			public ResultMessage()
			{
			}

			public ResultMessage(Ridder.Client.SDK.ResultMessage item)
			{
				MessageType = (MessageTypes)item.MessageType;
				Message = item.Message;
			}

			public ResultMessage(MessageTypes error, string message)
			{
				MessageType = error;
				Message = message;
			}

			public MessageTypes MessageType { get; }

			public string Message { get; }
		}

		public enum MessageTypes
		{
			Error,
			Warning,
			Message
		}
	}
}