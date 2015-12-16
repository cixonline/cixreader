// *****************************************************
// CIXReader
// Crypto.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 28/11/2013 8:26
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CIXClient
{
    /// <summary>
    /// Implements a simple string cipher class.
    /// </summary>
    internal static class StringCipher
    {
        private const int SaltLength = 8;

        /// <summary>
        /// Encrypt the plaintext string with the given passphrase and returns the
        /// encrypted string in base64 format.
        /// </summary>
        /// <param name="plainText">The string to be encrypted</param>
        /// <param name="passPhrase">The passphrase to use in the encryption</param>
        /// <returns>The encrypted string</returns>
        internal static string Encrypt(string plainText, string passPhrase)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;

            byte[] cipherTextBytes;

            try
            {
                byte[] salt1 = new byte[SaltLength];
                using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetBytes(salt1);
                }

                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(passPhrase, salt1, 10000);

                // Create AES algorithm
                aes = new AesManaged
                {
                    Key = rfc2898.GetBytes(32),
                    IV = rfc2898.GetBytes(16)
                };

                // Create Memory and Crypto Streams
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                // Encrypt Data
                byte[] data = Encoding.UTF8.GetBytes(plainText);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                byte[] encryptionStream = memoryStream.ToArray();

                // Append the salt to the end of the encrypted string
                cipherTextBytes = new byte[encryptionStream.Length + salt1.Length];
                Buffer.BlockCopy(encryptionStream, 0, cipherTextBytes, 0, encryptionStream.Length);
                Buffer.BlockCopy(salt1, 0, cipherTextBytes, encryptionStream.Length, salt1.Length);
            }
            finally
            {
                if (cryptoStream != null)
                {
                    cryptoStream.Close();
                    memoryStream = null; // Because CryptoStream will dispose of it.
                }
                if (memoryStream != null)
                {
                    memoryStream.Close();
                }
                if (aes != null)
                {
                    aes.Clear();
                }
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Decrypt the encoded string with the given passphrase and returns the
        /// plaintext string.
        /// </summary>
        /// <param name="cipherText">The encoded string to be decrypted</param>
        /// <param name="passPhrase">The passphrase to use in the decryption</param>
        /// <returns>The decrypted string</returns>
        internal static string Decrypt(string cipherText, string passPhrase)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;

            byte[] decryptBytes;

            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // Split the cipher text into the data and salt portions as they were
                // combined by the encryption.
                byte[] salt = new byte[SaltLength];
                Buffer.BlockCopy(cipherTextBytes, cipherTextBytes.Length - SaltLength, salt, 0, SaltLength);

                byte[] data = new byte[cipherTextBytes.Length - SaltLength];
                Buffer.BlockCopy(cipherTextBytes, 0, data, 0, cipherTextBytes.Length - SaltLength);

                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(passPhrase, salt, 10000);

                // Create AES algorithm
                aes = new AesManaged
                {
                    Key = rfc2898.GetBytes(32),
                    IV = rfc2898.GetBytes(16)
                };

                // Create Memory and Crypto Streams
                memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                // Decrypt data
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                // Get the decrypted string
                decryptBytes = memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
                if (aes != null)
                {
                    aes.Clear();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
        }
    }
}