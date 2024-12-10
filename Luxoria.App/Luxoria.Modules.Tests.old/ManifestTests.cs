using LuxImport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxoria.Modueles.Tests;

public class ManifestTests
{
    [Fact]
    public void CreateManifest_ReturnsManifest()
    {
        // Act
        Manifest result = new("Test", "This is a test manifest", "1.0.0", new("1.0"));

        // Check the properties
        DateTime createdAt = result.CreatedAt;
        DateTime updatedAt = result.UpdatedAt;

        // Assert
        Assert.Equal("Test", result.Name);
        Assert.Equal("This is a test manifest", result.Description);
        Assert.Equal("1.0.0", result.Version);
        Assert.Equal("1.0", result.Luxoria.Version);

        // Verify the CreatedAt and UpdatedAt properties
        Assert.Equal(result.CreatedAt, createdAt);

        // Change a property
        result.Name = "Test 2";

        // Verify the UpdatedAt property
        Assert.NotEqual(result.UpdatedAt, updatedAt);
    }
}