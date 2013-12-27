//.Net3.5
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace jIAnSoft.Framework.Security.Cryptography {
    public class AzDG {
        private static string _strCharSet = "utf-8";
        private const string Cipher = "Private key";
        private static string _strGenuineKey = "";

        private static byte[] PrivateKeyCrypt(IList<byte> inputData) {
            var cipherHash = Encoding.GetEncoding(_strCharSet).GetBytes(Md5.Encrypt(_strGenuineKey).ToLower());
            var outData = new byte[inputData.Count];
            for (var i = 0; i < inputData.Count; ++i) {
                outData[i] = (byte)(inputData[i] ^ cipherHash[i % 32]);
            }
            return outData;
        }

        private static byte[] DoEncrypt(IList<byte> inputData) {
            var noise =
                Encoding.GetEncoding(_strCharSet).GetBytes(
                    Md5.Encrypt(
                        DateTime.Now.Ticks.ToString(
                            CultureInfo.InvariantCulture)).ToLower());
            var outData = new byte[inputData.Count * 2];
            for (var j = 0; j < inputData.Count; j++) {
                outData[(j * 2)] = noise[j % 32];
                outData[(j * 2) + 1] = (byte)(inputData[j] ^ noise[j % 32]);
            }
            return PrivateKeyCrypt(outData);
        }

        public static String Encrypt(String argStrSource, String argStrPrivateKey = "") {
            return Encrypt(argStrSource, argStrPrivateKey, _strCharSet);
        }

        public static String Encrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
            _strCharSet = argStrCharset;
            _strGenuineKey = string.Format("{0}{1}", Cipher, argStrPrivateKey);
            return Convert.ToBase64String(DoEncrypt(Encoding.GetEncoding(_strCharSet).GetBytes(argStrSource)));
        }

        private static byte[] DoDecrypt(byte[] inputData) {
            inputData = PrivateKeyCrypt(inputData);
            var outData = new byte[inputData.Length / 2];
            for (var j = 0; j < outData.Length; ++j) {
                outData[j] = (byte)(inputData[(j * 2)] ^ inputData[(j * 2) + 1]);
            }
            return outData;
        }

        public static String Decrypt(String argStrSource, String argStrPrivateKey = "") {
            return Decrypt(argStrSource, argStrPrivateKey, _strCharSet);
        }

        public static String Decrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
            _strGenuineKey = string.Format("{0}{1}", Cipher, argStrPrivateKey);
            return Encoding.GetEncoding(argStrCharset).GetString(DoDecrypt(Convert.FromBase64String(argStrSource)));
        }
    }
}
