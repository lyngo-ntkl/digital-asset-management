using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace DigitalAssetManagement.Tests.UnitTests.Folders
{
    [TestFixture]
    public class FolderCreationTests: Setup
    {
        private static object[]? _userDoesNotHavePermissionOfFolderTestCases = null;
        public static object[] UserDoesNotHavePermissionOfFolderTestCases
        {
            get
            {
                if (_userDoesNotHavePermissionOfFolderTestCases == null)
                {
                    _userDoesNotHavePermissionOfFolderTestCases = new object[Data.ArraySize];

                    for (int i = 0; i < Data.ArraySize; i++)
                    {
                        _userDoesNotHavePermissionOfFolderTestCases[i] = new object[]
                        {
                            Data.Instance.Users[i],
                            new FolderCreationRequestDto { FolderName = $"Folder {i}", ParentFolderId = i },
                            new Permission { Id = i, Role = Domain.Enums.Role.Reader, FolderId = i, UserId = i, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow}
                        };
                    }
                }
                return _userDoesNotHavePermissionOfFolderTestCases;
            }
        }

        [Test]
        [TestCaseSource(nameof(UserDoesNotHavePermissionOfFolderTestCases))]
        public void Create_GivenUserDoesNotHavePermissionOfParentFolder_ThrowForbiddenException(User user, FolderCreationRequestDto request, Permission permission)
        {
            // Arrange
            _userService!.Setup(us => us.GetLoginUserAsync()).ReturnsAsync(user);
            _unitOfWork!.Setup(uow => uow.PermissionRepository.GetOnConditionAsync(p => p.FolderId == request.ParentFolderId && p.UserId == user.Id))
                .ReturnsAsync(permission);

            // Act
            AsyncTestDelegate creation = async () => {
                await _folderService!.Create(request);
            };

            // Assert
            var exception = Assert.ThrowsAsync<ForbiddenException>(creation);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.UnallowedFolderModification));
        }

        private static object[]? _userIsNotOwnerOfDriveTestCases = null;
        public static object[] UserIsNotOwnerOfDriveTestCases
        {
            get
            {
                if (_userIsNotOwnerOfDriveTestCases == null)
                {
                    _userIsNotOwnerOfDriveTestCases = new object[Data.ArraySize];
                    for (int i = 0; i < Data.ArraySize; i++)
                    {
                        _userIsNotOwnerOfDriveTestCases[i] = new object[]
                        {
                            Data.Instance.Users[i],
                            new FolderCreationRequestDto { FolderName = $"Folder {i}", ParentDriveId = i },
                            new Drive { Id = i, UserId = i + 1, DriverName = $"Drive {i}", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
                        };
                    }
                }
                return _userIsNotOwnerOfDriveTestCases;
            }
        }

        [Test]
        [TestCaseSource(nameof(UserIsNotOwnerOfDriveTestCases))]
        public void Create_GivenUserIsNotOwnerOfDrive_ThrowForbiddenException(User user, FolderCreationRequestDto request, Drive drive)
        {
            // Arrange
            _userService!.Setup(us => us.GetLoginUserAsync()).ReturnsAsync(user);
            _unitOfWork!.Setup(uow => uow.DriveRepository.GetById(request.ParentDriveId!.Value)).Returns(drive);

            // Act
            AsyncTestDelegate creation = async () => {
                await _folderService!.Create(request);
            };

            // Assert
            var exception = Assert.ThrowsAsync<ForbiddenException>(creation);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.UnallowedFolderModification));
        }
    }
}
