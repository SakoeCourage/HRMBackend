using System;
using System.ComponentModel.DataAnnotations;

namespace HRMBackend.Utilities.ExtendedDataAnotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredIf: ValidationAttribute
    {
        private readonly string dependentPropertyName;
        private readonly object targetValue;

        public RequiredIf(string dependentPropertyName, object targetValue)
        {
            this.dependentPropertyName = dependentPropertyName;
            this.targetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentPropertyValue = validationContext.ObjectInstance.GetType().GetProperty(dependentPropertyName)?.GetValue(validationContext.ObjectInstance);

            if (dependentPropertyValue != null && dependentPropertyValue.Equals(targetValue) && value == null)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required when {dependentPropertyName} has the value {targetValue}.");
            }

            return ValidationResult.Success;
        }
    }
}
