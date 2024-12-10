using Luxoria.SDK.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Luxoria.SDK.Services
{
    public class Sha256Service : IFileHasherService
    {
        /// <summary>
        /// Computes the SHA-256 hash of the specified file.
        /// </summary>
        /// <param name="filePath">The path to the file to hash.</param>
        /// <returns>The SHA-256 hash of the file as a lowercase hexadecimal string.</returns>
        /// <exception cref="ArgumentException">Thrown when the file path is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
        public string ComputeFileHash(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            using (FileStream stream = File.OpenRead(filePath))
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
