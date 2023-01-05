using System.Security.Cryptography;

namespace BlazorBingo;

public class Utility
{
    // https://stackoverflow.com/questions/51999902/rngcryptoserviceprovider-getbytes-how-to-restrict-return-values
    // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rngcryptoserviceprovider.getbytes?view=net-6.0

    public const string URL_KEY_CHARS = "bcdfghjkmnpqrstvwxyz23456789";
    public const string BINGO_KEY_CHARS = "BCDFGHJKMNPQRSTVWXYZ0123456789";

    public static string GenerateKey(int keySize, string possibleCharacters)
    {
        if (keySize < 1 || keySize > 256) { throw new ArgumentOutOfRangeException("keySize", "size must be between 1 and 256."); }
        if (string.IsNullOrWhiteSpace(possibleCharacters)) { throw new ArgumentException("possibleCharacters cannot be null, empty, or whitespace.", "possibleCharacters"); }
        var numPossibleCharacters = possibleCharacters.Length;
        int limit = (256 / numPossibleCharacters) * numPossibleCharacters; // use limit to avoid bias towards some characters
        var randomBytes = RandomBytesInRange(limit, keySize);
        var key = new char[keySize];
        for (int i = 0; i < keySize; i++)
        {
            key[i] = possibleCharacters[randomBytes[i] % numPossibleCharacters];
        }
        return new string(key);
    }

    private const int SECURE_BYTE_BUFFER_SIZE = 32;
    private static byte[] RandomBytesInRange(int exclusiveUpperBound, int countRequired)
    {
        var randomBytes = new byte[countRequired];            
        using (var crypto = RandomNumberGenerator.Create())
        {
            byte[] buffer = new byte[SECURE_BYTE_BUFFER_SIZE];
            int ix = SECURE_BYTE_BUFFER_SIZE;
            int countProduced = 0;
            while (countProduced < countRequired)
            {
                if (ix == SECURE_BYTE_BUFFER_SIZE)
                {
                    crypto.GetBytes(buffer);
                    ix = 0;
                }
                while (ix < SECURE_BYTE_BUFFER_SIZE)
                {
                    if (buffer[ix] < exclusiveUpperBound)
                    {
                        randomBytes[countProduced] = buffer[ix];
                        countProduced++;
                        if (countProduced == countRequired) break;
                    }
                    ix++;
                }
            }
        }
        return randomBytes;
    }
}
