using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Globalization;

namespace VCTWeb.Core.Domain.CustomValidators
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class ZipCodeValidator : Validator<string>
    {

        private ValueAccess valueAccess;
        internal const string OtherPropName = "Country";
        public ZipCodeValidator(ValueAccess valueAccess)
            : base("TEst", null)
        {
            this.valueAccess = valueAccess;
        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            object myProp1;
            string valueAccessFailureMessage;

            //try to obtain the value of property 1.
            if (this.valueAccess.GetValue(currentTarget, out myProp1, out valueAccessFailureMessage))
            {
                if ((string)myProp1 == "USA")
                {
                    if (objectToValidate != null)
                    {
                        if ((objectToValidate.Length != 5 && objectToValidate.Length != 9))
                            base.LogValidationResult(validationResults, "valZipCode", null, null);
                    }
                }
                else if (Convert.ToString(objectToValidate).Trim() == string.Empty)
                {
                    base.LogValidationResult(validationResults, "valZipCode", null, null);
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                return "Test";
            }
        }
    }
}
