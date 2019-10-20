//  Copyright (C) 2019 Ben Staniford
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutoHostfileLib
{
    internal sealed class TrafficEncryptor
    {
        private static readonly Lazy<TrafficEncryptor> _encryptor = new Lazy<TrafficEncryptor>(() => new TrafficEncryptor());
        public static TrafficEncryptor Instance { get { return _encryptor.Value; } }

        private string _password;
        private const int Rfc2898KeygenIterations = 100;
        private const int AesKeySizeInBits = 128;
        private Aes _aesAlg = new AesManaged();
        private Rfc2898DeriveBytes _rfc2898;

        // This isn't great, we should find a way to use a proper salt to improve encryption security
        private byte[] Salt = new byte[16]{0xa, 0xf, 0x3, 0x5, 0x1, 0xc, 0x4, 0x1, 0x9, 0x3, 0x7, 0xc, 0x1, 0xe, 0xf , 0x7};

        private TrafficEncryptor()
        {
            _password = Config.Instance.GetSharedKey();
            _aesAlg.Padding = PaddingMode.PKCS7;
            _aesAlg.KeySize = AesKeySizeInBits;
            int keyStrengthInBytes = _aesAlg.KeySize / 8;
            _rfc2898 = new Rfc2898DeriveBytes(_password, Salt, Rfc2898KeygenIterations);
            _aesAlg.Key = _rfc2898.GetBytes(keyStrengthInBytes);
            _aesAlg.IV = _rfc2898.GetBytes(keyStrengthInBytes);
        }

        internal string Decrypt(byte[] cipherText)
        {
            byte[] plainText = null;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, _aesAlg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(cipherText, 0, cipherText.Length);
                }
                plainText = memoryStream.ToArray();
            }

            return Encoding.Unicode.GetString(plainText);
        }

        internal byte[] Encrypt(string plaintextMessage)
        {
            byte[] rawPlaintext = Encoding.Unicode.GetBytes(plaintextMessage);
            byte[] cipherText = null;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, _aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(rawPlaintext, 0, rawPlaintext.Length);
                }

                cipherText = memoryStream.ToArray();
            }

            return cipherText;
        }
    }
}
