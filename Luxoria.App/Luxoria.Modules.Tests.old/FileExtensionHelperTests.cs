using Luxoria.Modules.Models;
using Luxoria.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxoria.Modueles.Tests
{
    public class FileExtensionHelperTests
    {
        [Fact]
        public void GetFileExtension_ReturnsFileExtension()
        {
            // Act
            FileExtension result = FileExtensionHelper.ConvertToEnum(".png");
            // Assert
            Assert.Equal(FileExtension.PNG, result);
        }

        [Fact]
        public void GetFileExtension_ReturnsUnknown()
        {
            // Act
            FileExtension result = FileExtensionHelper.ConvertToEnum(".abc");
            // Assert
            Assert.Equal(FileExtension.UNKNOWN, result);
        }

        [Fact]
        public void GetFileExtension_ConvertEnumToString()
        {
            // Act
            string result = FileExtensionHelper.ConvertToString(FileExtension.PNG);
            // Assert
            Assert.Equal("PNG", result);
        }
    }
}
