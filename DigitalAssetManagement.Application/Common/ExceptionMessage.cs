namespace DigitalAssetManagement.Application.Common
{
    public class ExceptionMessage
    {
        public const string InvalidPassword = "Password contains at least 8 characters including at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character.";
        public const string PhoneNumberSupport = "We currently support Vietnam phone number only";
        public const string RegisteredEmail = "Email has been registered";
        public const string UnmatchedPassword = "Wrong password";
        public const string UnregisteredEmail = "Email hasn't been registered";
    }
}
