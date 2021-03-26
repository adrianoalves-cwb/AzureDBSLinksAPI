using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;


namespace DBSLinksAPI.Services
{
    public class EncryptionServices
    {
        private static Aes CreateCipher(string AesKey)
        {
            Aes cipher = Aes.Create();  
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = HexToByteArray(AesKey);
            return cipher;
        }

        public static string EncryptString(string PlainText, string AesKey)
        {
            Aes cipher = CreateCipher(AesKey);

            string IV = Convert.ToBase64String(cipher.IV);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(PlainText);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            string CypherText = Convert.ToBase64String(cipherText);
            return IV + " " + CypherText;
        }

        public static string DecryptString(string CipherText, string AesKey)
        {
            string[] tempCipherText = CipherText.Split(' ');

            string IV = tempCipherText[0].ToString();
            CipherText = tempCipherText[1].ToString();

            Aes cipher = CreateCipher(AesKey);

            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] cipherText = Convert.FromBase64String(CipherText);
            byte[] plainText = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(plainText);
        }

        private static byte[] HexToByteArray(string hexString)
        {
            if (0 != (hexString.Length % 2))
            {
                throw new ApplicationException("Hex string must be multiple of 2 in length");
            }

            int byteCount = hexString.Length / 2;
            byte[] byteValues = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                byteValues[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteValues;
        }

        private static string ByteArrayToHex(byte[] data)
        {
            //This converts the 64 byte hash into the string hex representation of byte values 
            // (shown by default as 2 hex characters per byte) that looks like 
            // "FB-2F-85-C8-85-67-F3-C8-CE-9B-79-9C-7C-54-64-2D-0C-7B-41-F6...", each pair represents
            // the byte value of 0-255.  Removing the "-" separator creates a 128 character string 
            // representation in hex
            return BitConverter.ToString(data).Replace("-", "");
        }

        //public static byte[] GetRandomData(int bits)
        //{
        //    var result = new byte[bits / 8];
        //    RandomNumberGenerator.Create().GetBytes(result);
        //    return result;
        //}
    }
}
