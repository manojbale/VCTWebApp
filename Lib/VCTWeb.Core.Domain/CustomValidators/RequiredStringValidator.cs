using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace VCTWeb.Core.Domain.CustomValidators
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class RequiredStringValidator : Validator
    {
        public RequiredStringValidator() : base(null, null)
        {
        }

        protected override string DefaultMessageTemplate
        {
            get { return string.Empty; }
        }

        protected override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == null)
            {
                this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
            }
            else if (Convert.ToString(objectToValidate).Trim() == string.Empty)
            {
                this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
            }
        }
    }
}
