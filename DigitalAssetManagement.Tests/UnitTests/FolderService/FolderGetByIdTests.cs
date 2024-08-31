using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace DigitalAssetManagement.Tests.UnitTests.Folders
{
    public partial class FolderGetByIdTests: Setup
    {
        private static object[]? _userDoesNotHaveAccessPermissionOfFolderTestCases = null;
        public static object[] UserDoesNotHaveAccessPermissionOfFolderTestCases
        {
            get
            {
                if (_userDoesNotHaveAccessPermissionOfFolderTestCases == null)
                {
                    _userDoesNotHaveAccessPermissionOfFolderTestCases = new object[Data.ArraySize];

                    for (int i = 0; i < Data.ArraySize; i++)
                    {
                        _userDoesNotHaveAccessPermissionOfFolderTestCases[i] = new object[]
                        {
                            Data.Instance.Users[i],
                            i
                        };
                    }
                }
                return _userDoesNotHaveAccessPermissionOfFolderTestCases;
            }
        }

        [Test]
        [TestCaseSource(nameof(UserDoesNotHaveAccessPermissionOfFolderTestCases))]
        public void Get_GivenUserDoesNotHaveAccessPermissionOfFolder_ThrowForbiddenException(User user, int folderId)
        {
            // Arrange
            _userService!.Setup(us => us.GetLoginUserAsync()).ReturnsAsync(user);
            _unitOfWork!.Setup(uow => uow.PermissionRepository.GetFirstOnConditionAsync(p => p.FolderId == folderId && p.UserId == user.Id))
                .ReturnsAsync((Permission?) null);

            // Act
            AsyncTestDelegate creation = async () => {
                await _folderService!.Get(folderId);
            };

            // Assert
            var exception = Assert.ThrowsAsync<ForbiddenException>(creation);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.UnallowedFolderAccess));
        }

        private static object[]? _folderNotFoundTestCases = null;
        public static object[]? FolderNotFoundTestCases
        {
            get
            {
                if (_folderNotFoundTestCases == null)
                {
                    _folderNotFoundTestCases = new object[Data.ArraySize];
                    for (int i = 0; i < Data.ArraySize; i++)
                    {
                        _folderNotFoundTestCases[i] = new object[]
                        {
                            Data.Instance.Users[i],
                            i,
                            Data.Instance.ReaderPermissions[i]
                        };
                    }
                }
                return _folderNotFoundTestCases;
            }
        }

        // TODO: don't know how to set up expression tree the right way
        [Test]
        [TestCaseSource(nameof(FolderNotFoundTestCases))]
        public void Get_GivenFolderDoesNotExist_ThrowNotFoundException(User user, int folderId, Permission permission)
        {
            // Arrange
            _userService!.Setup(us => us.GetLoginUserAsync()).ReturnsAsync(user);
            _unitOfWork!.Setup(uow => uow.PermissionRepository.GetFirstOnConditionAsync(p => p.UserId == user.Id && p.FolderId == folderId)).ReturnsAsync(permission);
            _unitOfWork!.Setup(uow => uow.FolderRepository.GetByIdAsync(folderId)).ReturnsAsync((Folder?) null);

            // Act
            AsyncTestDelegate creation = async () => {
                await _folderService!.Get(folderId);
            };

            // Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(creation);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.FolderNotFound));
        }

        private static object[]? _getFolderSuccessTestCases = null;
        public static object[]? GetFolderSuccessTestCases
        {
            get
            {
                if (_getFolderSuccessTestCases == null)
                {
                    _getFolderSuccessTestCases = new object[Data.ArraySize];
                    for (int i = 0; i < Data.ArraySize / 2; i++)
                    {
                        _getFolderSuccessTestCases[i] = new object[]
                        {
                            Data.Instance.Users[i],
                            i,
                            Data.Instance.ReaderPermissions[i],
                            new Folder {
                                Id = i,
                                FolderName = $"Folder {i}",
                                ParentFolder = new Folder
                                {
                                    Id = i - 1,
                                    FolderName = $"Folder {i - 1}"
                                },
                                SubFolders = [
                                    new Folder { Id = i + 1, FolderName = $"Folder {i + 1}" }
                                ],
                                Files = [
                                    new Domain.Entities.File { Id = i, FileName = $"File {i}", FileContent = "" }
                                ]
                            },
                            new FolderDetailResponseDto
                            {
                                Id = i,
                                FolderName = $"Folder {i}",
                                ParentFolder = new FolderResponseDto { Id = i - 1, FolderName = $"Folder {i - 1}" },
                                SubFolders = [
                                    new FolderResponseDto {Id = i + 1, FolderName = $"Folder {i + 1}"}
                                ],
                                Files = [
                                    new FileResponseDto { Id = i, FileName = $"File {i}"}
                                ]
                            }
                        };
                    }

                    for (int i = Data.ArraySize / 2; i < Data.ArraySize; i++)
                    {
                        _getFolderSuccessTestCases[i] = new object[]
                        {
                            Data.Instance.Users[i],
                            i,
                            Data.Instance.ReaderPermissions[i],
                            new Folder {
                                Id = i,
                                FolderName = $"Folder {i}",
                                ParentDrive = new Drive { Id = i, DriverName = $"Drive {i}", OwnerId = i },
                                SubFolders = [
                                    new Folder { Id = i + 1, FolderName = $"Folder {i + 1}" }
                                ],
                                Files = [
                                    new Domain.Entities.File { Id = i, FileName = $"File {i}", FileContent = "" }
                                ]
                            },
                            new FolderDetailResponseDto
                            {
                                Id = i,
                                FolderName = $"Folder {i}",
                                ParentDrive = new DriveResponseDto { Id = i, DriverName = $"Drive {i}" },
                                SubFolders = [
                                    new FolderResponseDto {Id = i + 1, FolderName = $"Folder {i + 1}"}
                                ],
                                Files = [
                                    new FileResponseDto { Id = i, FileName = $"File {i}"}
                                ]
                            }
                        };
                    }
                }
                return _getFolderSuccessTestCases;
            }
        }

        [Test]
        [TestCaseSource(nameof(GetFolderSuccessTestCases))]
        public async Task Get_GivenRightArguments_Success(User user, int folderId, Permission permission, Folder folder, FolderDetailResponseDto expected)
        {
            // Arrange
            _userService!.Setup(us => us.GetLoginUserAsync()).ReturnsAsync(user);
            _unitOfWork!.Setup(uow => uow.PermissionRepository.GetFirstOnConditionAsync(p => p.UserId == user.Id && (int?) typeof(Permission).GetProperty(nameof(Permission.FolderId))!.GetValue(p) == folderId)).ReturnsAsync(permission);
            _unitOfWork!.Setup(uow => uow.FolderRepository.GetByIdAsync(folderId)).ReturnsAsync(folder);

            // Act
            var actual = await _folderService!.Get(folderId);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
