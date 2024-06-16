using Konscious.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

namespace H3CinemaBooking.Repository.DTO
{
    public class HashingService
    {

        public string GenerateSalt()
        {
            var salt = new byte[16];  // Adjust the size according to your security requirements
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string base64Salt)
        {
            byte[] salt = Convert.FromBase64String(base64Salt);

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8; // Number of processing cores to use
                argon2.Iterations = 4;
                argon2.MemorySize = 1024 * 1024; // Amount of memory to use, in kibibytes
                return Convert.ToBase64String(argon2.GetBytes(16));
            }
        }
    }

    /*
    public bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = saltBytes,
            DegreeOfParallelism = 8,
            Iterations = 4,
            MemorySize = 1024 * 1024
        };

        var hashBytes = Convert.FromBase64String(hash);
        var newHash = argon2.GetBytes(16);
        return AreHashesEqual(hashBytes, newHash);
    }

    private bool AreHashesEqual(byte[] hash1, byte[] hash2)
    {
        if (hash1.Length != hash2.Length)
            return false;

        for (int i = 0; i < hash1.Length; i++)
        {
            if (hash1[i] != hash2[i])
                return false;
        }
        return true;
    }
    */
}
