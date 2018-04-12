using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace timepunch{
public class PasswordUtils
{
    public static byte[] generateSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
        return salt;
    }
    public static string passwordEncrypt(byte[] salt, string password)
    {
        string savedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 32));
        Console.WriteLine($"Hashed: {savedHash}");
        return savedHash;
    }
    public static bool checkPassword(string storedHash, byte[] salt, string password)
    {
        string passedInHash = passwordEncrypt(salt, password);
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