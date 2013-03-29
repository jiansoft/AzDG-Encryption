using System;
using System.Security.Cryptography;
using System.Text;

namespace jIAnSoft.Framework.Security.Cryptography {
    public static class Md5 {
        public static string Encrypt(string text) {
            return Encrypt(text, 0xfde9);
        }

        public static string Encrypt(string text, int codepage) {
            var bytes = Encoding.GetEncoding(codepage).GetBytes(text);
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(bytes)).ToLower().Replace("-", "");
        }
    }
}