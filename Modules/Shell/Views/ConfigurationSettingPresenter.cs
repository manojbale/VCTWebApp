using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class ConfigurationSettingPresenter : Presenter<IConfigurationSettingView>
    {
        #region Instance Variables

        private readonly ConfigurationRepository configurationRepository;
        private Configuration configuration;

        #endregion

        #region Constructors

        public ConfigurationSettingPresenter() :
            this(new ConfigurationRepository(HttpContext.Current.User.Identity.Name)) { }

        public ConfigurationSettingPresenter(ConfigurationRepository service)
        {
            configurationRepository = service;
            configuration = new Configuration();
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            View.ConfigurationsByGroupList = configurationRepository.FetchConfigurationsByGroup(KeyGroup);
        }

        public override void OnViewInitialized()
        {
            View.ConfigurationGroupList = configurationRepository.FetchAllConfigurationGroups();
        }

        #endregion

        #region Public Properties

        public string KeyName
        {
            get { return configuration.KeyName; }
            set { configuration.KeyName = value; }
        }

        public string KeyValue
        {
            get { return configuration.KeyValue; }
            set { configuration.KeyValue = value; }
        }

        public string DataType
        {
            get { return configuration.DataType; }
            set { configuration.DataType = value; }
        }
        public string KeyGroup
        {
            get { return configuration.KeyGroup; }
            set { configuration.KeyGroup = value; }
        }

        public string Description
        {
            get { return configuration.Description; }
            set { configuration.Description = value; }
        }
        #endregion

        #region Private Methods

        private Constants.ResultStatus ValidateConfigurationSettings()
        {
            if (!configuration.IsValid())
                return Constants.ResultStatus.ConfigurationTypeMismatch;
            return Constants.ResultStatus.Ok;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>ResultStatus enum value.</returns>
        public Constants.ResultStatus Save()
        {
            //Helper.LogInformation("Configuration", "Save method is invoked.");
            Constants.ResultStatus resultStatus = ValidateConfigurationSettings();
            if (resultStatus == Constants.ResultStatus.Ok)
            {
                try
                {
                    configurationRepository.SaveConfigurations(configuration);
                    resultStatus = Constants.ResultStatus.Updated;

                }
                catch
                {
                    throw;
                }
            }
            return resultStatus;
        }

        #endregion
    }
}




