using AutoMapper;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common.Mappers;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using DigitalAssetManagement.Infrastructure.Repositories;
using DigitalAssetManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DigitalAssetManagement.Tests.UnitTests.Folders
{
    public class Setup
    {
        protected Mock<UnitOfWork>? _unitOfWork;
        protected Mock<UserService>? _userService;
        protected IMapper? _mapper;
        protected FolderService? _folderService;
        protected PermissionService? _permissionService;
        [SetUp]
        public void Init()
        {
            _unitOfWork = new Mock<UnitOfWork>();
            _mapper = (new MapperConfiguration(config => config.AddProfile<UserMappingProfile>())).CreateMapper();
            _userService = new Mock<UserService>();
            _permissionService = new PermissionServiceImplementation(_unitOfWork.Object);
            _folderService = new FolderServiceImplementation(_unitOfWork.Object, _mapper, _userService.Object, _permissionService);
        }
    }
}
