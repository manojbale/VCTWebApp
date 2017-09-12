// -----------------------------------------------------------------------
// <copyright file="BadgeNumberValidatorAttribute.cs" company="irissoftware">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace VCTWeb.Core.Domain.CustomValidators
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

    public class BadgeNumberValidatorAttribute : ValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new BadgeNumberValidator();
        }
    }
}
