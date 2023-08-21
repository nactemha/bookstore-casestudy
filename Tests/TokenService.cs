using ecommerce.data;
using ecommerce.models;
using ecommerce.service;
using Moq;
using Xunit;

namespace ecommerce.Tests.services
{
    public class TokenServiceTests
    {

        public TokenServiceTests()
        {
           
        }

        [Fact]
        public async Task GetToken_ValidLoginRequest_ReturnsTokenInfo()
        {
            //Arrange
            var mockSettings = new Mock<ISettings>();
            mockSettings.Setup(x => x.GetSecretKey()).Returns("1234567890123456");
            mockSettings.Setup(x => x.GetTokenExperation()).Returns(new TimeSpan(0, 0, 1, 0));
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var tokenService = new TokenService(mockCustomerRepository.Object, mockSettings.Object);
            var loginRequest = new LoginRequest
            {
                Email = "email@test.com",
                Password = "123456"
            };
            //Act
            var tokenInfo = tokenService.GetToken(loginRequest);
            //Assert
            Assert.NotNull(tokenInfo);
            Assert.NotNull(tokenInfo.Token);
            Assert.Equal(0, tokenInfo.ErrorCode);

        }

    }
}