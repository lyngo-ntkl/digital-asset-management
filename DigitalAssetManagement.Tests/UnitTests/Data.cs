using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;
using DigitalAssetManagement.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DigitalAssetManagement.Tests.UnitTests
{
    public sealed class Data
    {
        //private static readonly object _lock = new object();
        //private static Data? _instance = null;
        //private Mock<IConfiguration>? _configuration;
        //private HashingHelper? _hashingHelper;
        //private Data() {
        //    _configuration = new Mock<IConfiguration>();
        //    _hashingHelper = new HashingHelperImplementation(_configuration.Object);

        //    _configuration.Setup(c => c.GetSection("hashing:saltByteSize").Value).Returns(TestByteSize);
        //    _configuration.Setup(c => c.GetSection("hashing:hashByteSize").Value).Returns(TestByteSize);
        //    _configuration.Setup(c => c.GetSection("hashing:iteration").Value).Returns(TestIteration);
        //}
        //public static Data Instance
        //{
        //    get
        //    {
        //        lock (_lock)
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new Data();
        //            }
        //            return _instance;
        //        }
        //    }
        //}

        //public static string TestByteSize = "32";
        //public static string TestIteration = "10";
        //public static string JwtKey = "This is the key used to sign and verify json web token, the key size must be greater than 512 bits";
        //public static string JwtIssuer = "lyntk";
        //public static string TestPassword = "Qwertyuiop1234567890!!";
        //public static string WrongTestPassword = "Qwerty123456789!";
        //public static int ArraySize = 10;

        //private User[]? _users = null;
        //private EmailPasswordRegistrationRequest[]? _emailPasswordRegistrationRequests;
        //public EmailPasswordRegistrationRequest[]? EmailPasswordRegistrationRequests {
        //    get
        //    {
        //        if (_emailPasswordRegistrationRequests == null)
        //        {
        //            _emailPasswordRegistrationRequests = new EmailPasswordRegistrationRequest[ArraySize];
        //            for (int i = 0; i < ArraySize; i++)
        //            {
        //                _emailPasswordRegistrationRequests[i] = new EmailPasswordRegistrationRequest { Email = $"user{i}@example.com", Name = $"User {i}", Password = TestPassword };
        //            }
        //        }
        //        return _emailPasswordRegistrationRequests;
        //    }
        //}
        //public User[] Users
        //{
        //    get
        //    {
        //        if (_users == null)
        //        {
        //            _hashingHelper!.Hash(TestPassword, out string salt, out string hash);
        //            _users = new User[ArraySize];
        //            for (int i = 0; i < ArraySize; i++)
        //            {
        //                _users[i] = new User
        //                {
        //                    Id = i,
        //                    Email = $"user{i}@example.com",
        //                    PasswordHash = hash,
        //                    PasswordSalt = salt,
        //                    Name = $"User {i}",
        //                    CreatedDate = DateTime.UtcNow,
        //                    ModifiedDate = DateTime.UtcNow
        //                };
        //            }
        //        }
        //        return _users;
        //    }
        //}

        //public Permission[]? _readerPermissions = null;
        //public Permission[] ReaderPermissions
        //{
        //    get
        //    {
        //        if (_readerPermissions == null)
        //        {
        //            _readerPermissions = new Permission[ArraySize];
        //            for (int i = 0; i < ArraySize; i++)
        //            {
        //                _readerPermissions[i] = new Permission { Id = i, Role = Domain.Enums.Role.Reader, FolderId = i, UserId = i, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow };
        //            }
        //        }
        //        return _readerPermissions;
        //    }
        //}
    }
}
