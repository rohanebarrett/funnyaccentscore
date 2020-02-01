using System;
using System.IO;
using System.Security.Cryptography;

namespace Funny.Accents.Core.Encryption
{
    public class AesEncryptor
    {
        public static byte[] GenerateKey() => Aes.Create().Key;

        public static byte[] GenerateIv() => Aes.Create().IV;

        public static byte[] EncryptString(string plainText, string Key, string IV)
        {
            return EncryptStringToBytes(plainText,
                Convert.FromBase64String(Key), Convert.FromBase64String(IV));
        }/*End of EncryptStringToBytes method*/

        public static byte[] EncryptString(string plainText, byte[] Key, byte[] IV)
        {
            return EncryptStringToBytes(plainText, Key, IV);
        }/*End of EncryptStringToBytes method*/

        public static string DecryptString(string cipherText, string Key, string IV)
        {
            return DecryptStringFromBytes(Convert.FromBase64String(cipherText),
                Convert.FromBase64String(Key), Convert.FromBase64String(IV));
        }

        public static string DecryptString(byte[] cipherText, byte[] Key, byte[] IV)
        {
            return DecryptStringFromBytes(cipherText, Key, IV);
        }/*End of DecryptStringFromBytes method*/

        public static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0) { throw new ArgumentNullException("plainText"); }
            if (Key == null || Key.Length <= 0) { throw new ArgumentNullException("Key"); }
            if (IV == null || IV.Length <= 0) { throw new ArgumentNullException("IV"); }

            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0) { throw new ArgumentNullException("cipherText"); }
            if (Key == null || Key.Length <= 0) { throw new ArgumentNullException("Key"); }
            if (IV == null || IV.Length <= 0) { throw new ArgumentNullException("IV"); }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
    }/*End of AesEncryptor*/
}/*End of Funny.Accents.Core.Encryption namespace*/
