using DigitalAssetManagement.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DigitalAssetManagement.Application.Common.Attributes
{
    public class PasswordAttribute: ValidationAttribute
    {
        private const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~`!@#$%^&*()-_+=\?{}[\]:;',.])[a-zA-Z\d~`!@#$%^&*()-_+=\?{}[\]:;',.]{8,}$";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && !Regex.IsMatch((string) value, pattern))
            {
                throw new BadRequestException(ExceptionMessage.InvalidPassword);
            }

            return ValidationResult.Success;
        }
    }
}
