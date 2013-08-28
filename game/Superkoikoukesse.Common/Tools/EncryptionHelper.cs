// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Superkoikoukesse.Common.Utils
{
	/// <summary>
	/// Secured encryption & decryption
	/// </summary>
    public static class EncryptionHelper
    {
		private const char KEY_FILL_CHARACTER = '_';
		private const int KEY_LENGTH = 32;

		/// <summary>
		/// Application encryption key. Not constant!
		/// </summary>
		private static string EncryptionKey = "j7gdft5'(eqA84Mo"; // 16 octets MAX

		/// <summary>
		/// Setup the encryption key
		/// </summary>
		/// <param name="key">string of 0 to 16 chars</param>
		public static void SetKey(string key) {

			if (string.IsNullOrEmpty (key)) {
				throw new ApplicationException("Encryption key cannot be empty!");
			}
      if (key.Length > KEY_LENGTH) {
        throw new ApplicationException("Encryption key must be "+KEY_LENGTH+" chars max!");
			}

			Logger.I("Setting encryption key.");
			EncryptionKey = key;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipheredtext">Texte en base 64</param>
        /// <returns></returns>
		public static String Decrypt(String cipheredText)
		{
			string finalKey = EncryptionKey.PadRight(KEY_LENGTH, KEY_FILL_CHARACTER);
			
			RijndaelManaged crypto = null;
			MemoryStream mStream = null;
			ICryptoTransform decryptor = null;
			CryptoStream cryptoStream = null;
			
			try
			{
				byte[] cipheredData = Convert.FromBase64String(cipheredText);
				
				crypto = new RijndaelManaged();
				crypto.KeySize = 256;
//				crypto.Padding = PaddingMode.PKCS7;
				crypto.Mode = CipherMode.CBC;
				
				decryptor = crypto.CreateDecryptor(Encoding.UTF8.GetBytes(finalKey), Encoding.UTF8.GetBytes(finalKey));
				
				mStream = new System.IO.MemoryStream(cipheredData);
				cryptoStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);
				StreamReader creader = new StreamReader(cryptoStream, Encoding.UTF8);
				
				String data = creader.ReadToEnd();
				return data;
				//return Encoding.UTF8.GetString(Encoding.Default.GetBytes(data));
			}
			finally
			{
				if (crypto != null)
				{
					crypto.Clear();
				}
				
				if (cryptoStream != null)
				{
					cryptoStream.Close();
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
		public static String Encrypt(String plainText)
		{
			string finalKey = EncryptionKey.PadRight(KEY_LENGTH, KEY_FILL_CHARACTER);
			
			RijndaelManaged crypto = null;
			MemoryStream mStream = null;
			ICryptoTransform encryptor = null;
			CryptoStream cryptoStream = null;
			
			byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			
			try
			{
				crypto = new RijndaelManaged();
				crypto.KeySize = 256;
				crypto.Mode = CipherMode.CBC;

				encryptor = crypto.CreateEncryptor(Encoding.UTF8.GetBytes(finalKey), Encoding.UTF8.GetBytes(finalKey));
				
				mStream = new MemoryStream();
				cryptoStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write);
				cryptoStream.Write(plainBytes, 0, plainBytes.Length);
			}
			finally
			{
				if (crypto != null)
					crypto.Clear();
				
				cryptoStream.Close();
			}
			
			return Convert.ToBase64String(mStream.ToArray());
		}


    }

}
