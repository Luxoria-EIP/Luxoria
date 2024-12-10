using System;
using Luxoria.Modules.Models;

namespace Luxoria.Modules.Utils
{
    /// <summary>
    /// Helper class for operations related to the FileExtension enum.
    /// </summary>
    public class FileExtensionHelper
    {
        /// <summary>
        /// Converts a string to the corresponding FileExtension enum value.
        /// </summary>
        /// <param name="value">The string representation of the file extension.</param>
        /// <returns>The corresponding FileExtension enum value, or FileExtension.UNKNOWN if the string is invalid.</returns>
        public static FileExtension ConvertToEnum(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return FileExtension.UNKNOWN;
            }

            // Normalize the input by trimming, removing leading dot, and converting to uppercase
            string normalizedValue = value.TrimStart('.').ToUpperInvariant();

            // Attempt to parse the string into the FileExtension enum
            if (Enum.TryParse(normalizedValue, out FileExtension result))
            {
                return result;
            }

            // Return UNKNOWN if parsing fails
            return FileExtension.UNKNOWN;
        }

        /// <summary>
        /// Checks if a given string is a valid representation of a FileExtension enum.
        /// </summary>
        /// <param name="value">The string representation of the file extension.</param>
        /// <returns>True if the string is valid; otherwise, false.</returns>
        public static bool IsValidFileExtension(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string normalizedValue = value.TrimStart('.').ToUpperInvariant();
            return Enum.TryParse(normalizedValue, out FileExtension _);
        }

        /// <summary>
        /// Converts a FileExtension enum value to its string representation.
        /// </summary>
        /// <param name="value">The FileExtension enum value to convert.</param>
        /// <returns>The string representation of the enum value, or "UNKNOWN" if the value is not valid.</returns>
        public static string ConvertToString(FileExtension value)
        {
            // Get the name of the enum value
            return Enum.GetName(typeof(FileExtension), value) ?? "UNKNOWN";
        }
    }
}
