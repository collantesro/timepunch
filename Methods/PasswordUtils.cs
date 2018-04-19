using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace timepunch
{
    public class PasswordUtils
    {
        public static string generateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string sSalt = Convert.ToBase64String(salt);
            return sSalt;
        }
        public static string passwordEncrypt(string sSalt, string password)
        {
            byte[] salt = Convert.FromBase64String(sSalt);
            string savedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32));
            return savedHash;
        }
        public static bool checkPassword(string storedHash, string sSalt, string password)
        {
            string passedInHash = passwordEncrypt(sSalt, password);
            if (storedHash == passedInHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
