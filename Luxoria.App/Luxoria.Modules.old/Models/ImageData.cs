namespace Luxoria.Modules.Models;

/// <summary>
/// Represents an image with pixel data, dimensions, and format information.
/// </summary>
public class ImageData
{
    /// <summary>
    /// Gets the raw pixel data of the image.
    /// </summary>
    public ReadOnlyMemory<byte> PixelData { get; }

    /// <summary>
    /// Gets the width of the image in pixels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the image in pixels.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the format of the image (e.g., "PNG", "JPEG").
    /// </summary>
    public FileExtension Format { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageData"/> class.
    /// </summary>
    /// <param name="pixelData">The raw pixel data of the image.</param>
    /// <param name="width">The width of the image in pixels.</param>
    /// <param name="height">The height of the image in pixels.</param>
    /// <param name="format">The format of the image (e.g., "PNG", "JPEG").</param>
    /// <exception cref="ArgumentException">Thrown when width or height is less than or equal to zero, or format is null or empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when pixelData is null.</exception>
    public ImageData(byte[] pixelData, int width, int height, FileExtension format)
    {
        if (width <= 0)
            throw new ArgumentException("Width must be greater than zero.", nameof(width));

        if (height <= 0)
            throw new ArgumentException("Height must be greater than zero.", nameof(height));

        PixelData = pixelData ?? throw new ArgumentNullException(nameof(pixelData));
        Width = width;
        Height = height;
        Format = format;
    }

    /// <summary>
    /// Returns a string representation of the image, including its format and dimensions.
    /// </summary>
    /// <returns>A string representation of the image.</returns>
    public override string ToString()
    {
        return $"{Format} Image: {Width}x{Height}";
    }
}
