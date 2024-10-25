using DigitalAssetManagement.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssetManagement.Application.Common.Attributes
{
    public class XorAttribute: ValidationAttribute
    {
        private readonly string _property1;
        private readonly string _property2;

        public XorAttribute(string property1, string property2)
        {
            _property1 = property1;
            _property2 = property2;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var propertyValue1 = value.GetType().GetProperty(_property1)?.GetValue(value);
            var propertyValue2 = value.GetType().GetProperty(_property2)?.GetValue(value);

            if ((propertyValue1 == null && propertyValue2 == null) ||
                (propertyValue1 != null && propertyValue2 != null))
            {
                throw new BadRequestException($"If {propertyValue1} is null, {propertyValue2} is not null and vice versa");
            }

            return ValidationResult.Success;
        }
    }
}
