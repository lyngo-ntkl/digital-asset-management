using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.UseCases.Users.Read;
using Moq;
using NUnit.Framework;

namespace DigitalAssetManagement.Tests.Users.Read
{
    [TestFixture]
    public class GetUsersTest: UserServiceSetUp
    {
        private GetUsers? _getUsers;
        private Mock<UserRepository>? _userRepository;
        private Mock<Mapper>? _mapper;
        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<UserRepository>();
            _mapper = new Mock<Mapper>();
            _getUsers = new GetUsersHandler(_userRepository.Object, _mapper.Object);
        }
    }
}
