using System.Security.Cryptography;
using System.Text;

namespace Web_Social_network_BE.Handle;

public class MD5Hash
{
    public static string GetHashString(string input)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        var stringBuilder = new StringBuilder();
        foreach (var t in hashBytes)
        {
            stringBuilder.Append(t.ToString("X2"));
        }

        return stringBuilder.ToString();
    }
}