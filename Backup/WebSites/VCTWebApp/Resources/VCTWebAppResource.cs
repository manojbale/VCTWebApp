using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Resources;
using VCTWeb.Core.Domain;
using System.Collections;
using System.IO;

namespace VCTWebApp.Resources
{
    public class VCTWebAppResource
    {
        private ConfigurationRepository configurationRepository = new ConfigurationRepository();
        private  Dictionary<string, string> cultureDictionary = null;
        private  CultureInfo currentCulture = null;

        public  void RefreshCultureValues()
        {
            ResXResourceReader resxReader = null;
            cultureDictionary = new Dictionary<string, string>();
            string language = configurationRepository.GetConfigurationKeyValue(Constants.Dictionary.Language.ToString());
            //switch ((new Helper()).GetAppSettingsValue("DefaultCulture"))
            switch (language)
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

        public  string GetString(string key)
        {
            if (cultureDictionary == null || cultureDictionary.Count == 0)
                RefreshCultureValues();

            if (cultureDictionary.ContainsKey(key))
                return cultureDictionary[key];
            else
                return string.Empty;
        }

        public  CultureInfo GetCurrentCulture()
        {
            return currentCulture;
        }
    }
}
