using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TestWordGenerator {
    public class HashCalculator {
        private readonly object generatorLock = new object();
        SHA256Managed sha256 = null;
        SHA384Managed sha384 = null;
        SHA512Managed sha512 = null;

        public HashCalculator() {
            sha384 = new SHA384Managed();
            sha256 = new SHA256Managed();
            sha512 = new SHA512Managed();
        }

        public void calculateHash(PasswordHashData passwordHashData) {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(passwordHashData.Password);
            passwordHashData.HashSha384 = getHashSha384(bytes);
            passwordHashData.HashSha256 = getHashSha256(bytes);
            passwordHashData.HashSha512 = getHashSha512(bytes);
        }

        public string getHashSha384(byte[] input) {
            byte[] hash = sha384.ComputeHash(input);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }

        public string getHashSha256(byte[] input) {
            byte[] hash = sha256.ComputeHash(input);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }

        public string getHashSha512(byte[] input) {
            byte[] hash = sha256.ComputeHash(input);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }

}
