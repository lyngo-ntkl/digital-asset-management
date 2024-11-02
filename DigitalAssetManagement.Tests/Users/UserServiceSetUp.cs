using NUnit.Framework;
using Moq;
namespace DigitalAssetManagement.Tests.Users
{
    public class UserServiceSetUp
    {
        //private Mock<UnitOfWork>? _unitOfWork;
        //private IMapper? _mapper;
        //private Mock<IConfiguration>? _configuration;
        //private HashingHelper? _hashingHelper;
        //private JwtHelper? _jwtHelper;
        //private Mock<IHttpContextAccessor>? _httpContextAccessor;
        //private UserService? _service;
        //[SetUp]
        //public void Init()
        //{
        //    _unitOfWork = new Mock<UnitOfWork>();
        //    _configuration = new Mock<IConfiguration>();
        //    _hashingHelper = new HashingHelperImplementation(_configuration.Object);
        //    _jwtHelper = new JwtHelperImplementation(_configuration.Object);
        //    _mapper = (new MapperConfiguration(config => config.AddProfile<UserMappingProfile>())).CreateMapper(x =>
        //    {
        //        if (x == typeof(PasswordMappingAction))
        //        {
        //            return new PasswordMappingAction(_hashingHelper);
        //        }
        //        return null;
        //    });
        //    _httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    _service = new UserServiceImplementation(_unitOfWork.Object, _mapper, _hashingHelper, _jwtHelper, _httpContextAccessor.Object);

        //    _configuration.Setup(c => c.GetSection("hashing:saltByteSize").Value).Returns(Data.TestByteSize);
        //    _configuration.Setup(c => c.GetSection("hashing:hashByteSize").Value).Returns(Data.TestByteSize);
        //    _configuration.Setup(c => c.GetSection("hashing:iteration").Value).Returns(Data.TestIteration);
        //    _configuration.Setup(c => c["jwt:key"]).Returns(Data.JwtKey);
        //    _configuration.Setup(c => c["jwt:issuer"]).Returns(Data.JwtIssuer);
        //}

        //private static object[]? _registerTestCases = null;
        //public static object[] RegisterTestCases
        //{
        //    get
        //    {
        //        if (_registerTestCases == null)
        //        {
        //            _registerTestCases = new object[Data.ArraySize];
        //            for (int i = 0; i < Data.ArraySize; i++)
        //            {
        //                _registerTestCases[i] = new object[] { Data.Instance.Users[i].Email, Data.Instance.EmailPasswordRegistrationRequests![i] };
        //            }
        //        }
        //        return _registerTestCases;
        //    }
        //}

        //[Test]
        //[TestCaseSource(nameof(RegisterTestCases))]
        //public void Register_GivenExistedEmail_ThrowBadRequestException(string email, EmailPasswordRegistrationRequest request)
        //{
        //    // Arrange
        //    _unitOfWork!.Setup(uow => uow.UserRepository.ExistByEmail(email)).Returns(true);

        //    // Act
        //    AsyncTestDelegate registration = async () =>
        //    {
        //        await _service!.Register(request);
        //    };

        //    // Assert
        //    var exception = Assert.ThrowsAsync<BadRequestException>(registration);
        //    Assert.That(exception, Is.Not.Null);
        //    Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.RegisteredEmail));
        //}

        //[Test]
        //[TestCaseSource(nameof(RegisterTestCases))]
        //public void Register_GivenRightRequest_NotThrowException(string email, EmailPasswordRegistrationRequest request)
        //{
        //    // Arrange
        //    _unitOfWork!.Setup(uow => uow.UserRepository.ExistByEmail(email)).Returns(false);

        //    // Act
        //    AsyncTestDelegate registration = async () =>
        //    {
        //        await _service!.Register(request);
        //    };

        //    // Assert
        //    Assert.DoesNotThrowAsync(registration);
        //}

        //private static object[]? _emailPasswordAuthUnregisterEmailTestCases = null;
        //public static object[] EmailPasswordAuthUnregisteredEmailTestCases
        //{
        //    get
        //    {
        //        if (_emailPasswordAuthUnregisterEmailTestCases == null)
        //        {
        //            _emailPasswordAuthUnregisterEmailTestCases = new object[Data.ArraySize];
        //            for (int i = 0; i < Data.ArraySize; i++)
        //            {
        //                _emailPasswordAuthUnregisterEmailTestCases[i] = new object[] {
        //                    new EmailPasswordAuthRequest { Email = $"user{i}@example.com", Password = Data.TestPassword }
        //                };
        //            }
        //        }
        //        return _emailPasswordAuthUnregisterEmailTestCases;
        //    }
        //}
        //[Test]
        //[TestCaseSource(nameof(EmailPasswordAuthUnregisteredEmailTestCases))]
        //public void LoginWithEmailPassword_GivenUnregisteredEmail_ThrowBadRequestException(EmailPasswordAuthRequest request)
        //{
        //    // Arrange
        //    _unitOfWork!.Setup(uow => uow.UserRepository.GetByEmailAsync(request.Email)).ReturnsAsync((User?) null);

        //    // Act
        //    AsyncTestDelegate auth = async () => await _service!.LoginWithEmailPassword(request);

        //    // Assert
        //    var exception = Assert.ThrowsAsync<BadRequestException>(auth);
        //    Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.UnregisteredEmail));
        //}

        //private static object[]? _emailPasswordAuthWrongPasswordTestCases = null;
        //public static object[] EmailPasswordAuthWrongPasswordTestCases
        //{
        //    get
        //    {
        //        if (_emailPasswordAuthWrongPasswordTestCases == null)
        //        {
        //            _emailPasswordAuthWrongPasswordTestCases = new object[Data.ArraySize];
        //            for (int i = 0; i < Data.ArraySize; i++)
        //            {
        //                _emailPasswordAuthWrongPasswordTestCases[i] = new object[] {
        //                    new EmailPasswordAuthRequest { Email = $"user{i}@example.com", Password = Data.WrongTestPassword },
        //                    Data.Instance.Users[i]
        //                };
        //            }
        //        }
        //        return _emailPasswordAuthWrongPasswordTestCases;
        //    }
        //}
        //[Test]
        //[TestCaseSource(nameof(EmailPasswordAuthWrongPasswordTestCases))]
        //public void LoginWithEmailPassword_GivenUnmatchedPassword_ThrowBadRequestException(EmailPasswordAuthRequest request, User user)
        //{
        //    // Arrange
        //    _unitOfWork!.Setup(uow => uow.UserRepository.GetByEmailAsync(request.Email)).ReturnsAsync(user);

        //    // Act
        //    AsyncTestDelegate auth = async () => await _service!.LoginWithEmailPassword(request);

        //    // Assert
        //    var exception = Assert.ThrowsAsync<BadRequestException>(auth);
        //    Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.UnmatchedPassword));
        //}

        //private static object[]? _emailPasswordAuthTestCases = null;
        //public static object[] EmailPasswordAuthTestCases
        //{
        //    get
        //    {
        //        if (_emailPasswordAuthTestCases == null)
        //        {
        //            _emailPasswordAuthTestCases = new object[Data.ArraySize];
        //            for (int i = 0; i < Data.ArraySize; i++)
        //            {
        //                _emailPasswordAuthTestCases[i] = new object[] {
        //                    new EmailPasswordAuthRequest { Email = $"user{i}@example.com", Password = Data.TestPassword },
        //                    Data.Instance.Users[i]
        //                };
        //            }
        //        }
        //        return _emailPasswordAuthTestCases;
        //    }
        //}
        //[Test]
        //[TestCaseSource(nameof(EmailPasswordAuthTestCases))]
        //public async Task LoginWithEmailPassword_GivenRightRequest_ReturnAuthToken(EmailPasswordAuthRequest request, User user)
        //{
        //    // Arrange
        //    _unitOfWork!.Setup(uow => uow.UserRepository.GetByEmailAsync(request.Email)).ReturnsAsync(user);

        //    // Act
        //    var authResponse = await _service!.LoginWithEmailPassword(request);

        //    // Assert
        //    Assert.That(authResponse, Is.Not.Null);
        //    Assert.That(authResponse.AccessToken, Is.Not.Empty);
        //}
    }
}
