using System.Security.Cryptography;
using System.Text;

namespace Database
{
    public static class Utilities
    {
        public static string Sha256(string randomString)
        {
            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto) hash.Append(theByte.ToString("x2"));
            return hash.ToString();
        }
    }
}