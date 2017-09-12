using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Collections;
using System.Configuration;

namespace VCTWeb.Core.Domain
{
    public class ConfigSetting
    {
        #region Public Constants

        public const string ALERT_MAIL_FROM = "AlertMailFrom";
        public const string ALERT_MAIL_HOST = "AlertMailHost";
        public const string ALERT_MAIL_MESSAGE_HARVEST_STOP = "AlertMailMessageHarvestStop";
        public const string ALERT_MAIL_MESSAGE_PRODUCT = "AlertMailMessageProduct";
        public const string ALERT_MAIL_MESSAGE_TICKET_RECEIVE = "AlertMailMessageTicketReceive";
        public const string ALERT_MAIL_PORT = "AlertMailPort";
        public const string ALERT_MAIL_RECIPIENT = "AlertMailRecipient";
        public const string ALERT_MAIL_SUBJECT_HARVEST_STOP = "AlertMailSubjectHarvestStop";
        public const string ALERT_MAIL_SUBJECT_PRODUCT = "AlertMailSubjectProduct";
        public const string ALERT_MAIL_SUBJECT_TICKET_RECEIVE = "AlertMailSubjectTicketReceive";
        public const string DOMAIN = "Domain";
        public const string DROP_FOLDER_PATH = "DropFolderPath";
        public const string EDI_EXPORT_FOLDER = "EDIExportFolder";
        public const string ERROR_FOLDER_PATH = "ErrorFolderPath";
        public const string FILE_WATCHER_LOG_FOLDER_PATH = "FileWatcherLogFolderPath";
        public const string FOLDER_TO_SCAN = "FolderToScan";
        public const string INDICATOR_DIGIT = "IndicatorDigit";
        public const string IS_ACTIVE_DIRECTORY = "IsActiveDirectory";
        public const string GRID_PAGE_SIZE = "GridPageSize";
        public const string LABEL_TEMPLATE_STORAGE_PATH = "LabelTemplateStoragePath";
        public const string LOGO_IMAGE_FILE_NAME = "LogoImageFileName";
        public const string POLL_INTERVAL = "PollInterval";
        public const string PRINT_DEFECTIVE_TICKET = "PrintDefectiveTicket";
        public const string SHIPPING_DATE_GAP = "ShippingDateGap";
        public const string PROCESSED_FOLDER_PATH = "ProcessedFolderPath";
        public const string SEARCH_PATTERN = "SearchPattern";
        public const string ACKNOWLEDGEMENT_LOG_DAYS_TO_KEEP = "AcknowledgementLogDaysToKeep";
        public const string DEFAULT_PASSWORD = "DefaultPassword";
        //in case of RFID Create Pallet if this flag is true will create ticket internally from SGTIN then that newly created ticket will assign to Pallet
        public const string AUTO_CREATETICKET_FROM_SGTINS = "AutoCreateTicketFromSGTINs";
        public const string FTP_SERVER_NAME = "FTPServerName";
        public const string FTP_SERVER_PORT = "FTPServerPort";
        public const string FTP_USER_NAME = "FTPUserName";
        public const string FTP_PASSWORD = "FTPPassword";
        public const string FTP_FOLDER_PATH = "FTPFolderPath";
        public const string FTP_CHOICE = "FTPChoice";

        public const string VERSION = "Version";
        public const string BYPASSTICKETCREATION = "AutomaticTicketCreation";
        public const string APPLICATION_NAME = "ApplicationName";

        public const string ENABLE_LICENSING = "EnableLicensing";
        public const string LICENSING_SERVER = "LicensingServer";
        public const string LICENSING_SERVER_PORT = "LicensingServerPort";
        public const string LICENSE_WEB_APP_NAME = "LicenseWebAppName";
        public const string LICENSE_WEB_APP_VERION = "LicenseWebAppVerion";

        #endregion



        #region Constructor

        public ConfigSetting()
        {

        }

        #endregion

        #region Private Methods

        private void InitializeSettings()
        {
            //List<VCTWeb.Core.Domain.Configuration> configurations = new ConfigurationRepository().FetchAllConfigurations();

            //// most probably will be used from WebApplication Calls
            //if (IsSessionAllowed())
            //{
            //    if (configurations != null && configurations.Count > 0)
            //    {
            //        foreach (VCTWeb.Core.Domain.Configuration configuration in configurations)
            //        {
            //            System.Web.HttpContext.Current.Session[configuration.KeyName] = configuration;
            //        }
            //    }
            //}
        }

        private bool IsSessionAllowed()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// Validates the type of the data.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Validated value from the configuration object.</returns>
        private string ValidateDataType(Configuration configuration)
        {
            string value = string.Empty;
            switch (configuration.DataType.Trim().ToLower())
            {
                case "integer":
                case "int":
                    int intResult;
                    if (int.TryParse(configuration.KeyValue, out intResult))
                    {
                        value = configuration.KeyValue;
                    }
                    else
                    {
                        value = "0";
                    }
                    break;
                case "float":
                    float floatResult;
                    if (float.TryParse(configuration.KeyValue, out floatResult))
                    {
                        value = configuration.KeyValue;
                    }
                    else
                    {
                        value = "0.0";
                    }
                    break;
                case "double":
                    double doubleResult;
                    if (double.TryParse(configuration.KeyValue, out doubleResult))
                    {
                        value = configuration.KeyValue;
                    }
                    else
                    {
                        value = "0.0";
                    }
                    break;
                case "boolean":
                case "bool":
                    bool boolResult;
                    if (bool.TryParse(configuration.KeyValue, out boolResult))
                    {
                        value = configuration.KeyValue;
                    }
                    else
                    {
                        value = "false";
                    }
                    break;
                default:
                    value = configuration.KeyValue;
                    break;
            }
            return value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reloads the configuration settings into the collection.
        /// </summary>
        public void Reset()
        {
            this.InitializeSettings();
        }

        /// <summary>
        /// Gets the settings value for given key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the value for key.</returns>
        public static string GetValue(string key)
        {
            try
            {
                return new ConfigurationRepository().GetConfigurationKeyValue(key);
                //ConfigSetting configSetting = new ConfigSetting();
                //if (configSetting.IsSessionAllowed())
                //{
                //    return configSetting.ValidateDataType((Configuration)System.Web.HttpContext.Current.Session[key]);
                //}
                //else
                //{
                //    ConfigurationRepository configurationRepository = new ConfigurationRepository();
                //    return configurationRepository.GetConfigurationKeyValue(key);
                //}
            }
            catch
            {
                throw new Exception("Unable to Retreive value for Config Settings.");
            }
        }

        #endregion
    }
}
