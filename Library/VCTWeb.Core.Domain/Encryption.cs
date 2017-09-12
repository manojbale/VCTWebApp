#region Header

/********************************************************************************
 * Class Name:      Encryption.cs                                                      
 *
 * Module Name:     Scanware
 * 
 * Author:          Rajendra Negi
 * 
 * Description:     This page is used to encrypt and decrypt the input string.
 * 
 * Creation Date:   19/01/2009 (dd/mm/yyyy)                                              
 *                                                                              
 * Date             Modified By             Change                                  
 * ------------------------------------------------------------------------------

  *******************************************************************************/

#endregion Header

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VCTWeb.Core.Domain
{
	public class Encryption
	{
		#region Static Fields

		private static byte[] _key = { };
		private static byte[] _iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
		private static string encryptionKey = "1!629a#pe";

		#endregion Static Fields

		#region Public Methods

		/// <summary>
		/// Decrypts the specified input string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>Return decrypt string</returns>
		public static string Decrypt(string input)
		{
			Byte[] inputByteArray = new Byte[input.Length];
			try
			{
			    _key = System.Text.Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8));
			    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			    inputByteArray = Convert.FromBase64String(input);
			    MemoryStream ms = new MemoryStream();
			    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(_key, _iv), CryptoStreamMode.Write))
			    {
			        cs.Write(inputByteArray, 0, inputByteArray.Length);
			        cs.FlushFinalBlock();
			    }
			    Encoding encoding = Encoding.UTF8;
			    return encoding.GetString(ms.ToArray());

			}
			catch (Exception ex)
			{
			    throw ex;
			}
		}

		/// <summary>
		/// Encrypts the specified input string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>Return encrypt string</returns>
		public static string Encrypt(string input)
		{
			try
			{
			    _key = System.Text.Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8));
			    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			    Byte[] inputByteArray = Encoding.UTF8.GetBytes(input);
			    MemoryStream ms = new MemoryStream();
			    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(_key, _iv), CryptoStreamMode.Write))
			    {
			        cs.Write(inputByteArray, 0, inputByteArray.Length);
			        cs.FlushFinalBlock();
			    }
			    return Convert.ToBase64String(ms.ToArray());
			}
			catch (Exception ex)
			{
			    throw ex;
			}
		}

		#endregion Public Methods
	}
}