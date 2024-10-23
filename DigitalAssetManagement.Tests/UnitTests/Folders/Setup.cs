using AutoMapper;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;
using DigitalAssetManagement.Infrastructure.Common.Mappers;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using DigitalAssetManagement.Infrastructure.Repositories;
using DigitalAssetManagement.Infrastructure.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Moq;
using NUnit.Framework;

namespace DigitalAssetManagement.Tests.UnitTests.Folders
{
    public class Setup
    {
        //protected Mock
        ////protected Mock<UserService>? _userService;
        //protected IMapper? _mapper;
        //protected SystemFolderHelper? _folderHelper;
        //protected FolderService? _folderService;
        ////protected PermissionService? _permissionService;
        ////protected Mock<IBackgroundJobClient> _backgroundJobClient;
        //[SetUp]
        //public void Init()
        //{
        ////    _unitOfWork = new Mock<UnitOfWork>();
        //    _mapper = (new MapperConfiguration(config => config.AddProfile<UserMappingProfile>())).CreateMapper();
        //    _folderHelper = new SystemFolderHelperImplementation(new HostingEnvironment());
        ////    _userService = new Mock<UserService>();
        ////    _permissionService = new PermissionServiceImplementation(_unitOfWork.Object, _userService.Object);
        ////    _backgroundJobClient = new Mock<IBackgroundJobClient>();
        //    _folderService = new FolderServiceImplementation(_mapper, _folderHelper, _userService.Object, _permissionService, _backgroundJobClient.Object);
        //}
    }
}
