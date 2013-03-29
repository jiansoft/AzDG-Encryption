//.Net3.5
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace jIAnSoft.Framework.Security.Cryptography {
    public class AzDG {
        private static string _strCharSet = "utf-8";
        private const string StrCompanyKey = "Private key";
        private static string _strGenuineKey = "";

        private static byte[] PrivateKeyCrypt(IList<byte> argAbteSource) {
            var abteEncryptKey = Encoding.GetEncoding(_strCharSet).GetBytes(Md5.Encrypt(_strGenuineKey).ToLower());
            var abteReturn = new byte[argAbteSource.Count];
            for (var i = 0; i < argAbteSource.Count; ++i) {
                abteReturn[i] = (byte)(argAbteSource[i] ^ abteEncryptKey[i % 32]);
            }
            return abteReturn;
        }

        private static byte[] DoEncrypt(IList<byte> argStrSource) {
            var abteCrcKey =
                Encoding.GetEncoding(_strCharSet).GetBytes(
                    Md5.Encrypt(
                        DateTime.Now.Ticks.ToString(
                            CultureInfo.InvariantCulture)).ToLower());
            var abteReturn = new byte[argStrSource.Count * 2];

            for (int i = 0, j = 0; i < argStrSource.Count; ++i, ++j) {
                abteReturn[j] = abteCrcKey[i % 32];
                ++j;
                abteReturn[j] = (byte)(argStrSource[i] ^ abteCrcKey[i % 32]);
            }

            return PrivateKeyCrypt(abteReturn);
        }

        public static String Encrypt(String argStrSource) {
            return Encrypt(argStrSource, "");
        }

        public static String Encrypt(String argStrSource, String argStrPrivateKey) {
            return Encrypt(argStrSource, argStrPrivateKey, _strCharSet);
        }

        public static String Encrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
            _strCharSet = argStrCharset;
            _strGenuineKey = StrCompanyKey + argStrPrivateKey;
            return Convert.ToBase64String(DoEncrypt(Encoding.GetEncoding(_strCharSet).GetBytes(argStrSource)));
        }

        private static byte[] DoDecrypt(byte[] argStrSource) {
            argStrSource = PrivateKeyCrypt(argStrSource);
            var abteReturn = new byte[argStrSource.Length / 2];
            for (int i = 0, j = 0; i < argStrSource.Length; ++i, ++j) {
                abteReturn[j] = (byte)(argStrSource[i] ^ argStrSource[++i]);
            }
            return abteReturn;
        }

        public static String Decrypt(String argStrSource) {
            return Decrypt(argStrSource, "");
        }

        public static String Decrypt(String argStrSource, String argStrPrivateKey) {
            return Decrypt(argStrSource, argStrPrivateKey, _strCharSet);
        }

        public static String Decrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
            _strCharSet = argStrCharset;
            _strGenuineKey = string.Format("{0}{1}", StrCompanyKey, argStrPrivateKey);
            return Encoding.GetEncoding(_strCharSet).GetString(DoDecrypt(Convert.FromBase64String(argStrSource)));
        }
    }
}
