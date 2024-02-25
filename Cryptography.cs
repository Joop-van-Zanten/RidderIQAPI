using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RidderIQAPI
{
	/// <summary>
	/// Cryptography Handler
	/// </summary>
	public static class Cryptography
	{
		internal static byte[] cryptKey;
		internal static byte[] cryptIV;

		/// <summary>
		/// Initialiser for the Cryptography
		/// </summary>
		/// <param name="key">secret key for the symmetric algorithm</param>
		/// <param name="iv">initialization vector for the symmetric algorithm.r</param>
		public static void Initialise(string key, string iv)
		{
			if (Initialised)
				return;
			// Argument checks
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Null/Empty string not allowed", nameof(key));
			if (string.IsNullOrEmpty(iv))
				throw new ArgumentException("Null/Empty string not allowed", nameof(iv));
			if (key.Length != 32)
				throw new ArgumentException("Length must be 32 characters", nameof(key));
			if (iv.Length != 32)
				throw new ArgumentException("Length must be 16 characters", nameof(iv));
			//Store (new) Key and IV
			cryptKey = Encoding.ASCII.GetBytes(key);
			cryptIV = Encoding.ASCII.GetBytes(iv);
			// Set flaf for initialised is True
			Initialised = true;
		}

		internal static void InitialiseRandom() => Initialise(32.RandomString(), 32.RandomString());

		/// <summary>
		/// Wether or not the Cryptography is Initialised
		/// </summary>
		public static bool Initialised { get; private set; }

		/// <summary>
		/// SSL Decrypt a string to readable content
		/// </summary>
		/// <param name="encrypted">hashed string to be converted to text</param>
		/// <returns></returns>
		public static string OpenSSLDecrypt(string encrypted)
		{
			// Initialised check
			if (!Initialised)
				throw new InvalidOperationException("Cryptography is not yet Initialised");
			// Check if the input is null or empty
			if (string.IsNullOrEmpty(encrypted))
				throw new ArgumentException("Value cannot be Null or Empty", nameof(encrypted));
			// Check if the input is Base964 encoded
			if (!encrypted.IsBase64String())
				// Return the input string, since its not a valid Base64 string
				return encrypted;
			// Create a result string
			string result = null;
			// Declare the RijndaelManaged object used to decrypt the data, with the specified key and IV.
			RijndaelManaged aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7, KeySize = 256, BlockSize = 256, Key = cryptKey, IV = cryptIV };
			// Create the streams used for decryption.
			using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encrypted)))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Read))
				{
					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
					{
						// Read the decrypted bytes from the decrypting stream and place them in a string.
						result = srDecrypt.ReadToEnd();
					}
				}
			}
			// Return the result
			return result;
		}

		/// <summary>
		/// SSL Encrypt readable content to unreadable content
		/// </summary>
		/// <param name="text">text to be converted to a hashed string</param>
		/// <returns></returns>
		public static string OpenSSLEncrypt(string text)
		{
			// Initialised check
			if (!Initialised)
				throw new InvalidOperationException("Cryptography is not yet Initialised");
			// Check arguments
			if (string.IsNullOrEmpty(text))
				throw new ArgumentException("Value cannot be Null or Empty", nameof(text));
			// Declare the RijndaelManaged object used to encrypt the data.
			RijndaelManaged aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7, KeySize = 256, BlockSize = 256, Key = cryptKey, IV = cryptIV };
			// Create the streams used for encryption.
			MemoryStream msEncrypt = new MemoryStream();
			try
			{
				// Create an encryptor to perform the stream transform.
				using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Write))
				{
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
					{
						//Write all data to the stream.
						swEncrypt.Write(text);
						swEncrypt.Flush();
					}
				}
			}
			finally
			{
				// Clear the RijndaelManaged object.
				aesAlg?.Clear();
			}
			// Return the result
			return Convert.ToBase64String(msEncrypt.ToArray());
		}
	}
}