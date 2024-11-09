using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.UseCases.Users.Create;
using NUnit.Framework;
using Moq;
using DigitalAssetManagement.UseCases.UnitOfWork;
using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.Infrastructure.Common.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.Tests.Users.Create
{
    [TestFixture]
    public class UserRegistrationTest
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IMetadataPermissionUnitOfWork> _unitOfWork = new();
        private readonly Mock<IConfiguration> _configuration = new();
        private readonly Mock<IHostEnvironment> _env = new();
        private UserRegistration _userRegistration;
        private IHashingHelper _hashingHelper;
        private ISystemFolderHelper _folderHelper;

        private static object[]? _registerTestCases = null;

        [SetUp]
        public void SetUp()
        {
            _hashingHelper = new HashingHelperImplementation(_configuration.Object);
            _folderHelper = new SystemFolderHelperImplementation(_env.Object);
            _userRegistration = new UserRegistrationHandler(_userRepository.Object, _unitOfWork.Object, _hashingHelper, _folderHelper);

            _configuration.Setup(c => c.GetSection("hashing:saltByteSize").Value).Returns("");
            _configuration.Setup(c => c.GetSection("hashing:iteration").Value).Returns("");
            _configuration.Setup(c => c.GetSection("hashing:hashByteSize").Value).Returns("");
            _env.Setup(env => env.ContentRootPath).Returns("");

            int size = 10;
            _registerTestCases = new object[size];
            for (int i = 0; i < size; i++)
            {
                _registerTestCases[i] = new object[] {
                    new RegistrationRequest
                    {
                        Email = $"user{i}@example.com",
                        Password = "Qwertyuiop1234567890!!",
                        Name = $"User {i}"
                    }
                };
            }
        }

        [Test]
        [TestCaseSource(nameof(_registerTestCases))]
        public void Register_GivenExistedEmail_ThrowBadRequestException(RegistrationRequest request)
        {
            // Arrange
            _userRepository.Setup(ur => ur.ExistByEmailAsync(request.Email)).ReturnsAsync(true);

            // Act
            async Task registration()
            {
                await _userRegistration.RegisterAsync(request);
            }

            // Assert
            var exception = Assert.ThrowsAsync<BadRequestException>(registration);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.RegisteredEmail));
        }

        [Test]
        [TestCaseSource(nameof(_registerTestCases))]
        public void Register_GivenRightRequest_NotThrowException(RegistrationRequest request)
        {
            // Arrange
            _userRepository.Setup(ur => ur.ExistByEmailAsync(request.Email)).ReturnsAsync(true);

            // Act
            async Task registration()
            {
                await _userRegistration.RegisterAsync(request);
            }

            // Assert
            Assert.DoesNotThrowAsync(registration);
        }
    }
}
