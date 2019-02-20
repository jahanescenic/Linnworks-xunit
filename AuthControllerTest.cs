using Xunit;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LinnworksTest.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LinnworksTest;
using Moq;
using LinnworksTest.Controllers;

namespace linnworks_xunit
{
   
    public class AuthControllerTest
    {
        [Fact]
        public async Task LoginPost_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<ITokenRepository>();
            mockRepo.Setup(repo => repo.IsValidTokenAsync(Guid.NewGuid()))
                .ReturnsAsync(null);
            var controller = new AuthController(mockRepo.Object);
            controller.ModelState.AddModelError("login_failure", "Invalid token.");
            var newToken = new AuthController.Account();

            // Act
            var result = await controller.Login(newToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
    }
}