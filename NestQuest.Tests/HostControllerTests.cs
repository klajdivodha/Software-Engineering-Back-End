using Xunit;
using Moq;
using NestQuest.Controllers;
using NestQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NestQuest.Tests
{
    public class HostControllerTests
    {

        private readonly Mock<IHostServices> _mockHostServices;
        private readonly HostController _hostController;

        public HostControllerTests()
        {
        _mockHostServices = new Mock<IHostServices>();
        _hostController = new HostController(_mockHostServices.Object);
        }

        [Fact]
        public async Task ChangeEmailAsync_ReturnsNotFound_WhenServiceReturnsMinusOne()
        {
            // Arrange
            _mockHostServices.Setup(s => s.ChangeEmail(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(-1);

            // Act
            var result = await _hostController.ChangeEmailAsync("1", "newemail@example.com");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ChangeEmailAsync_ReturnsConflict_WhenServiceReturnsMinusTwo()
        {
            // Arrange
            _mockHostServices.Setup(s => s.ChangeEmail(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(-2);

            // Act
            var result = await _hostController.ChangeEmailAsync("1", "newemail@example.com");

            // Assert
            Assert.IsType<ConflictResult>(result);
        }

        [Fact]
        public async Task ChangeEmailAsync_ReturnsInternalServerError_WhenServiceReturnsZero()
        {
            // Arrange
            _mockHostServices.Setup(s => s.ChangeEmail(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(0);

            // Act
            var result = await _hostController.ChangeEmailAsync("1", "newemail@example.com");

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task ChangeEmailAsync_ReturnsOk_WhenServiceReturnsPositive()
        {
            // Arrange
            _mockHostServices.Setup(s => s.ChangeEmail(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);

            // Act
            var result = await _hostController.ChangeEmailAsync("1", "newemail@example.com");

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }

}