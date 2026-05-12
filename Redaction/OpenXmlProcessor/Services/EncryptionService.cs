using System.Text;

namespace OpenXmlProcessor.Services;

public class EncryptionService : IEncryptionService
{
    public string Encrypt(string plainText)
    {
        // Implement encryption logic here
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    public string Decrypt(string encryptedText)
    {
        // Implement decryption logic here
        return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
    }
}
