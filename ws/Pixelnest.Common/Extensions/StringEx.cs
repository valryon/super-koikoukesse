using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pixelnest.Common;

namespace System
{
    public static class StringEx
    {
        /// <summary>
        /// Encrypt the current string
        /// </summary>
        /// <seealso cref="Pixelnest.Common.EncryptionHelper"/>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(this string clearText)
        {
            return EncryptionHelper.Encrypt(clearText);
        }

        /// <summary>
        /// Decrypt the current string
        /// </summary>
        /// <seealso cref="Pixelnest.Common.EncryptionHelper"/>
        /// <param name="cipheredText"></param>
        /// <returns></returns>
        public static string Decrypt(this string cipheredText)
        {
            return EncryptionHelper.Encrypt(cipheredText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }
    }
}
