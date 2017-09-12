using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace VCTWeb.Core.Domain.CustomValidators
{
    public class ZipCodeValidatorAttribute : ValueValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            throw new InvalidOperationException("A member value access builder is needed.");
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            PropertyInfo propertyInfo = ownerType.GetProperty(ZipCodeValidator.OtherPropName);
            if (propertyInfo == null)
            {
                //throw new InvalidOperationException(String.Format(Resources.MyProp2ValidatorAttributeCouldNotFindProperty, new string[] { ownerType.Name, MyProp2Validator.OtherPropName }));
            }
            return new ZipCodeValidator(memberValueAccessBuilder.GetPropertyValueAccess(propertyInfo));
        }
    }
}