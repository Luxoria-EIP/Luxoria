namespace Luxoria.SDK.Interfaces;

public interface IFileHasherService
{
    /// <summary>
    /// Computes the SHA-256 hash of the specified file.
    /// </summary>
    /// <param name="filePath">The path to the file to hash.</param>
    /// <returns>The SHA-256 hash of the file as a lowercase hexadecimal string.</returns>
    /// <exception cref="ArgumentException">Thrown when the file path is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    string ComputeFileHash(string filePath);
}
