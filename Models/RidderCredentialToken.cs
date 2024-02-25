using Ridder.Client.SDK;
using System;
using System.Timers;

namespace RidderIQAPI.Models
{
	internal class RidderCredentialToken : IDisposable
	{
		private Timer authTimer;

		internal RidderCredentialToken(RidderCredential person, RidderIQSDK sdk)
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

		public bool Equals(RidderCredentialToken obj)
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
					catch (Exception ex)
					{
					}
				}
			}
			catch
			{
			}
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

		public RidderCredential Person { get; }

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
						catch (Exception ex)
						{
						}
					}
				}
				return sdk;
			}

			set => sdk = value;
		}
	}
}