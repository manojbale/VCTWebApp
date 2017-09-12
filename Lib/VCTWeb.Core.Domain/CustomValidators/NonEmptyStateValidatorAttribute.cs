using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Reflection;


namespace VCTWeb.Core.Domain.CustomValidators
{
    public class NonEmptyStateValidatorAttribute : ValueValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return null;
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            PropertyInfo propertyInfo = ownerType.GetProperty(NonEmptyStateValidator.COUNTRY);
            if (propertyInfo == null)
            {
            }
            return new NonEmptyStateValidator(memberValueAccessBuilder.GetPropertyValueAccess(propertyInfo));
        }
    }
}
