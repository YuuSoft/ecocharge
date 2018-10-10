using System;
using System.Security.Cryptography;

namespace BLL.Utils
{
    public static class PasswordHash
    {
        public static string Hash(string password)
        {
            string passwordToSave = string.Empty;

            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[128]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 50000);

            byte[] hash = pbkdf2.GetBytes(64);

            byte[] hashBytes = new byte[192];

            Array.Copy(salt, 0, hashBytes, 0, 128);
            Array.Copy(hash, 0, hashBytes, 128, 64);

            passwordToSave = Convert.ToBase64String(hashBytes);

            return passwordToSave;
        }

        public static bool Compare(string password, string storedPassword)
        {
            byte[] newHashBytes = Convert.FromBase64String(storedPassword);
            byte[] newSalt = new byte[128];
            Array.Copy(newHashBytes, 0, newSalt, 0, 128);

            var newPbkdf2 = new Rfc2898DeriveBytes(password, newSalt, 50000);
            byte[] newHash = newPbkdf2.GetBytes(64);

            for (int i = 0; i < 64; i++)
                if (newHashBytes[i + 128] != newHash[i])
                    return false;

            return true;
        }
    }
}