namespace DigitalAssetManagement.UseCases.Common
{
    public class ExceptionMessage
    {
        public const string InvalidPassword = "Password contains at least 8 characters including at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character.";
        public const string PhoneNumberSupport = "We currently support Vietnam phone number only";
        public const string RegisteredEmail = "Email has been registered";
        public const string UnmatchedPassword = "Wrong password";
        public const string UnregisteredEmail = "Email hasn't been registered";
        public const string UnallowedModification = "User doesn't have permission to modify this folder, file or drive";
        public const string MetadataNotFound = "File or folder not found";
        public const string UnallowedAccess = "User doesn't have permission to access this folder, file or drive";
        public const string FileNotFound = "File not found";
        public const string FolderNotFound = "Folder not found";
        public const string UnallowedMovement = "User does not have permission to move folder or file to other drive or folder";
        public const string UnallowedPermissionCreation = "User does not have permission to add new permission to this file or folder";
        public const string UserNotFound = "User not found";
        public const string UnsupportedAssetType = "Unsupported asset type";
        public const string NoPermission = "User doesn't have permission to take this action to this resource";
    }
}