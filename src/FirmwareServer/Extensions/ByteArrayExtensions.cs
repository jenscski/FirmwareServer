using System.Security.Cryptography;
using System.Text;

namespace FirmwareServer.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ComputeMD5Hash(this byte[] buffer)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(buffer);

                var sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public static string ComputeSHA256Hash(this byte[] buffer)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(buffer);

                var sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
