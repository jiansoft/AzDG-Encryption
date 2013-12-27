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
            for (var j = 0; j < argStrSource.Count; j++) {
                abteReturn[(j * 2)] = abteCrcKey[j % 32];
                abteReturn[(j * 2) + 1] = (byte)(argStrSource[j] ^ abteCrcKey[j % 32]);
            }
            return PrivateKeyCrypt(abteReturn);
        }

        public static String Encrypt(String argStrSource, String argStrPrivateKey = "") {
            return Encrypt(argStrSource, argStrPrivateKey, _strCharSet);
        }

        public static String Encrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
            _strCharSet = argStrCharset;
            _strGenuineKey = string.Format("{0}{1}", StrCompanyKey, argStrPrivateKey);
            return Convert.ToBase64String(DoEncrypt(Encoding.GetEncoding(_strCharSet).GetBytes(argStrSource)));
        }

        private static byte[] DoDecrypt(byte[] argStrSource) {
            argStrSource = PrivateKeyCrypt(argStrSource);
            var abteReturn = new byte[argStrSource.Length / 2];
            for (var j = 0; j < abteReturn.Length; ++j) {
                abteReturn[j] = (byte)(argStrSource[(j * 2)] ^ argStrSource[(j * 2) + 1]);
            }
            return abteReturn;
        }

        public static String Decrypt(String argStrSource, String argStrPrivateKey = "") {
            return Decrypt(argStrSource, argStrPrivateKey, _strCharSet);
        }

        public static String Decrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
            _strGenuineKey = string.Format("{0}{1}", StrCompanyKey, argStrPrivateKey);
            return Encoding.GetEncoding(argStrCharset).GetString(DoDecrypt(Convert.FromBase64String(argStrSource)));
        }
    }
}
