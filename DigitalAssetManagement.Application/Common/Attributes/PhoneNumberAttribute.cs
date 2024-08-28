using DigitalAssetManagement.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DigitalAssetManagement.Application.Common.Attributes
{
    public class PhoneNumberAttribute: ValidationAttribute
    {
        private const string pattern = @"(?:0|+84)\d{9}";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && !Regex.IsMatch((string) value, pattern))
            {
                throw new BadRequestException(ExceptionMessage.PhoneNumberSupport);
            }
            return ValidationResult.Success;
        }
    }
}
