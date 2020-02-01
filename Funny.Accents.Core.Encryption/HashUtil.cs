using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Funny.Accents.Core.Encryption
{
    public class HashUtil
    {
        public static byte[] GenerateSalt(int saltLenght)
        {
            byte[] salt = new byte[saltLenght];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }/*End of GenerateSalt method*/

        public static string GenerateHash(string valueToHash,
            byte[] salt, KeyDerivationPrf KeyDerivationPrf = KeyDerivationPrf.HMACSHA512,
            int iterationCount = 10000, int numBytesRequested = 256 / 8)
        {
            string hashedValue = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: valueToHash,
                salt: salt,
                prf: KeyDerivationPrf,
                iterationCount: iterationCount,
                numBytesRequested: numBytesRequested));

            return hashedValue;
        }/*End of GenerateHash method*/
    }/*End of HashUtil class*/
}/*End of Funny.Accents.Core.Encryption namespace*/
