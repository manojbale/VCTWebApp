using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
namespace VCTWeb.Core.Domain.CustomValidators
{
    public class NonEmptyStateValidator : Validator<string>
    {
        private ValueAccess countryValueAccess;
        internal const string COUNTRY = "Country";

        public NonEmptyStateValidator(ValueAccess valueAccess)
            : base(null, null)
        {
            this.countryValueAccess = valueAccess;
        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            object countryValueObj;
            string valueAccessFailureMessage;

            if (this.countryValueAccess.GetValue(currentTarget, out countryValueObj, out valueAccessFailureMessage))
            {
                if (countryValueObj != null)
                {
                    if (Convert.ToString(countryValueObj).Trim() == "USA")
                    {
                        if (objectToValidate != null)
                        {
                            if (string.IsNullOrEmpty(objectToValidate.Trim()))
                                base.LogValidationResult(validationResults, this.MessageTemplate, null, null);
                        }
                    }

                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return string.Empty; }
        }
    }
}
