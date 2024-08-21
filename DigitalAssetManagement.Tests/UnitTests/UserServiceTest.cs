using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Infrastructure.Common.Mappers;
using DigitalAssetManagement.Infrastructure.Services;
using NUnit.Framework;
using Moq;
using AutoMapper;
using DigitalAssetManagement.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Common;
namespace DigitalAssetManagement.Tests.UnitTests
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<UnitOfWork>? _unitOfWork;
        private IMapper? _mapper;
        private Mock<IConfiguration>? _configuration;
        private HashingHelper? _hashingHelper;
        private UserService? _service;
        [SetUp]
        public void Init()
        {
            _unitOfWork = new Mock<UnitOfWork>();
            _configuration = new Mock<IConfiguration>();
            _hashingHelper = new HashingHelperImplementation(_configuration.Object);
            _mapper = (new MapperConfiguration(config => config.AddProfile<MappingProfile>())).CreateMapper(x =>
            {
                if (x == typeof(PasswordMappingAction))
                {
                    return new PasswordMappingAction(_hashingHelper);
                }
                return null;
            });
            _service = new UserServiceImplementation(_unitOfWork.Object, _mapper, _hashingHelper);

            _configuration.Setup(c => c.GetSection("hashing:saltByteSize").Value).Returns(Data.TestByteSize);
            _configuration.Setup(c => c.GetSection("hashing:hashByteSize").Value).Returns(Data.TestByteSize);
            _configuration.Setup(c => c.GetSection("hashing:iteration").Value).Returns(Data.TestIteration);
        }

        [Test]
        [TestCaseSource(nameof(RegisterTestCases))]
        public void Register_GivenExistedEmail_ThrowBadRequestException(string email, EmailPasswordRegistrationRequest request)
        {
            // Arrange
            _unitOfWork!.Setup(uow => uow.UserRepository.ExistByEmail(email)).Returns(true);

            // Act
            AsyncTestDelegate registration = async () =>
            {
                await _service!.Register(request);
            };

            // Assert
            var exception = Assert.ThrowsAsync<BadRequestException>(registration);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.RegisteredEmail));
        }

        public static object[] RegisterTestCases
        {
            get
            {
                object[] data = new object[Data.ArraySize];
                for (int i = 0; i < Data.ArraySize; i++)
                {
                    data[i] = new object[] { Data.Instance.Users[i].Email, Data.Instance.EmailPasswordRegistrationRequests![i] };
                }
                return data;
            }
        }

        [Test]
        [TestCaseSource(nameof(RegisterTestCases))]
        public void Register_GivenRightRequest_NotThrowException(string email, EmailPasswordRegistrationRequest request)
        {
            // Arrange
            _unitOfWork!.Setup(uow => uow.UserRepository.ExistByEmail(email)).Returns(false);

            // Act
            AsyncTestDelegate registration = async () =>
            {
                await _service!.Register(request);
            };

            // Assert
            Assert.DoesNotThrowAsync(registration);
        }
    }
}
