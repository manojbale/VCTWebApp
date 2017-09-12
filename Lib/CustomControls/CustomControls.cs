using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;
using System;

namespace CustomControls
{


    [System.Web.UI.ToolboxData("<{0}:MyPropertyProxyValidator1 runat=server />")]
    public class MyPropertyProxyValidator : PropertyProxyValidator
    {
        protected override bool EvaluateIsValid()
        {
            Validator validator = new ValidationIntegrationHelper(this).GetValidator();
            if (validator != null)
            {
                ValidationResults results = validator.Validate(this);
                //if (this.DisplayMode == ValidationSummaryDisplayMode.SingleParagraph)
                //    base.ErrorMessage = FormatErrorMessage(results);
                //else
                    base.ErrorMessage = FormatErrorMessage(results, this.DisplayMode);
                return results.IsValid;
            }
            base.ErrorMessage = "";
            return true;
        }

        internal static string FormatErrorMessage(ValidationResults results, ValidationSummaryDisplayMode displayMode)
        {
            string str;
            string str2;
            string str3;
            string str4;
            StringBuilder builder = new StringBuilder();
            switch (displayMode)
            {
                case ValidationSummaryDisplayMode.List:
                    str = string.Empty;
                    str2 = string.Empty;
                    str3 = "<br/>";
                    str4 = string.Empty;
                    break;

                case ValidationSummaryDisplayMode.SingleParagraph:
                    str = string.Empty;
                    str2 = string.Empty;
                    str3 = " ";
                    str4 = "<br/>";
                    break;

                default:
                    str = "<ul>";
                    str2 = "<li>";
                    str3 = "</li>";
                    str4 = "</ul>";
                    break;
            }
            if (!results.IsValid)
            {
                SVResource rs = new SVResource();
                List<string> msgKeys = new List<string>();
                builder.Append(str);
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                  
                    string directory = HttpContext.Current.Request.ApplicationPath.ToString();
                    string relativePath = System.IO.Path.Combine(directory, "Images\\information.png");

                    string msgKey = rs.GetString(result.Message);
                    if (msgKey != string.Empty)
                    {
                        if (!msgKeys.Contains(msgKey))
                        {
                            msgKeys.Add(msgKey);
                            builder.Append(str2);
                            // str = string.Format("<img alt=\"{0}\" src=\"{1}\" />", msgKey, "../Images/information.png");
                            str = string.Format("<img alt=\"{0}\" src=\"{1}\" title=\"{2}\"/>", msgKey, relativePath, msgKey);
                  
                            builder.Append(str);
                            builder.Append(str3);
                        }
                    }
                }
                builder.Append(str4);
                msgKeys.Clear();
                rs = null;
            }
            return builder.ToString();
         

        }
    }
    public class SVResource
    {
        private Dictionary<string, string> cultureDictionary = null;
        private CultureInfo currentCulture = null;

        public void RefreshCultureValues()
        {
            ResXResourceReader resxReader = null;
            cultureDictionary = new Dictionary<string, string>();

            switch (GetAppSettingsValue("DefaultCulture"))
            {
                case "en-US":
                    resxReader = new ResXResourceReader(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Resources/EnglishCulture.resx"));
                    currentCulture = new CultureInfo("en-US");
                    break;

                case "es-ES":
                    resxReader = new ResXResourceReader(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Resources/SpanishCulture.resx"));
                    currentCulture = new CultureInfo("es-ES");
                    break;

                default:
                    resxReader = new ResXResourceReader(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Resources/EnglishCulture.resx"));
                    currentCulture = new CultureInfo("en-US");
                    break;
            }

            IDictionaryEnumerator resxEnumerator = resxReader.GetEnumerator();

            foreach (DictionaryEntry entry in resxReader)
            {
                cultureDictionary.Add(entry.Key.ToString(), entry.Value.ToString());
            }
        }

        public string GetString(string key)
        {
            if (cultureDictionary == null || cultureDictionary.Count == 0)
                RefreshCultureValues();

            if (cultureDictionary.ContainsKey(key))
                return cultureDictionary[key];
            else
                return string.Empty;
        }

        public CultureInfo GetCurrentCulture()
        {
            return currentCulture;
        }
        public string GetAppSettingsValue(string key)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                return ConfigurationManager.AppSettings[key];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
