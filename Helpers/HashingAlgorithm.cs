using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public class HashingAlgorithm
    {
        public static string GetHashString(string inputString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            HashAlgorithm algorithm = SHA256.Create();
            byte[] hashBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            foreach (byte hashByte in hashBytes)
            {
                stringBuilder.Append(hashByte.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}