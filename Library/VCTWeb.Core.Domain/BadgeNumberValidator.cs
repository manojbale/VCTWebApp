// -----------------------------------------------------------------------
// <copyright file="BadgeNumberValidator.cs" company="irissoftware">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------


namespace VCTWeb.Core.Domain.CustomValidators
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
    
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class BadgeNumberValidator : Validator
    {
        public BadgeNumberValidator()
            : base(null, null)
        {
        }

        protected override string DefaultMessageTemplate
        {
            get { return string.Empty; }
        }

        protected override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            int productTypeId;
            
            if (objectToValidate == null)
                this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
            else if (!int.TryParse(Convert.ToString(objectToValidate).Substring(1), out productTypeId))
                this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
        }
    }
}
