using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AESMethod
{
    private static readonly string key = "WgZ9H9n2tY7dF5jD8sR2U7vA4pX9qV6r";
    private static readonly string aesIV = "M3pX5vU8qL2bW9eR";
    public static string EncryptText(string textToEncrypt)
    {
        using (Aes aesTransform = Aes.Create())
        {
            aesTransform.Key = Encoding.UTF8.GetBytes(key);
            aesTransform.IV = Encoding.UTF8.GetBytes(aesIV);
            ICryptoTransform encryptor = aesTransform.CreateEncryptor(aesTransform.Key, aesTransform.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(textToEncrypt);
                    }
                    byte[] encryptedBytes = memoryStream.ToArray();
                    memoryStream.Close();
                    cryptoStream.Close();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
    }

    public static string DecryptText(string encryptedText)
    {
        using (Aes aesTransform = Aes.Create())
        {
            aesTransform.Key = Encoding.UTF8.GetBytes(key);
            aesTransform.IV = Encoding.UTF8.GetBytes(aesIV);
            ICryptoTransform decryptor = aesTransform.CreateDecryptor(aesTransform.Key, aesTransform.IV);
            using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText)))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamDecrypt = new StreamReader(cryptoStream))
                    {
                        return streamDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
