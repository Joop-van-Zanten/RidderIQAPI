using Ridder.Client.SDK;
using System;
using System.Timers;

namespace RidderIQAPI.Models.RidderIQ
{
	/// <summary>
	/// RidderIQ Credential token
	/// </summary>
	internal class RidderIQCredentialToken : IDisposable
	{
		private readonly Timer authTimer;

		internal RidderIQCredentialToken(RidderIQCredential person, RidderIQSDK sdk)
		{
			Person = person;
			Sdk = sdk;

			// Return whether the user is logged in
			if (sdk.LoggedinAndConnected)
			{
				authTimer = new Timer(1000 * 60);
				authTimer.Elapsed += AuthTimer_Elapsed;
				authTimer.AutoReset = false;
				authTimer.Start();
			}
		}

		public bool Equals(RidderIQCredentialToken obj)
		{
			if (obj is null || obj.Person is null)
				throw new NotImplementedException();

			if (Person.Company.Equals(obj.Person.Company, StringComparison.InvariantCultureIgnoreCase) == false)
				return false;
			if (Person.Username.Equals(obj.Person.Username, StringComparison.InvariantCultureIgnoreCase) == false)
				return false;
			if (Person.Password.Equals(obj.Person.Password, StringComparison.InvariantCultureIgnoreCase) == false)
				return false;

			return true;
		}

		private void AuthTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				try
				{
					Sdk.GetUserInfo();
				}
				catch (Exception)
				{
				}

				if (!sdk.LoggedinAndConnected)
				{
					try
					{
						lock (this)
						{
							// (Re)Connect to persistend session
							sdk.ConnectToPersistedSession(
								Person.Username,
								Person.Password,
								Person.Company
							);
							// Check if not connected and loggedin
							if (!sdk.LoggedinAndConnected)
							{
								// Revalidate login credentials
								sdk.RevalidateLogin(
									Person.Username,
									Person.Password,
									Person.Company
								);
							}
						}
					}
					catch { }
				}
			}
			catch { }
			finally
			{
				authTimer.Start();
			}
		}

		public void Dispose()
		{
			authTimer?.Stop();
			sdk?.Logout();
		}

		public RidderIQCredential Person { get; }

		private RidderIQSDK sdk;

		internal RidderIQSDK Sdk
		{
			get
			{
				if (!sdk.LoggedinAndConnected)
				{
					lock (this)
					{
						try
						{
							sdk.RevalidateLogin(Person.Username, Person.Password, Person.Company);
						}
						catch { }
					}
				}
				return sdk;
			}

			set => sdk = value;
		}
	}
}