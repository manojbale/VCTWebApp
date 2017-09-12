using System;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class eParPlusNotificationSetupPresenter : Presenter<IeParPlusNotificationSetup>
    {
        readonly NotificationDetailRepository _notificationDetailRepositoryInstance;
        readonly NotificationRepository _notificationRepositoryInstance;
        private readonly CustomerRepository _customerRepositoryInstance;
        private readonly ConfigurationRepository _configurationRepositoryInstance;
        private readonly DictionaryRepository _dictionaryRepositoryInstance;
        private readonly Helper _helper = new Helper();

        #region Constructors

        public eParPlusNotificationSetupPresenter()
            : this(new CustomerRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public eParPlusNotificationSetupPresenter(CustomerRepository customerRepository)
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusNotificationSetupPresenter", "Constructor is invoked.");
            _customerRepositoryInstance = customerRepository;
            _customerRepositoryInstance = new CustomerRepository(HttpContext.Current.User.Identity.Name);
            _notificationDetailRepositoryInstance = new NotificationDetailRepository(HttpContext.Current.User.Identity.Name);
            _configurationRepositoryInstance = new ConfigurationRepository(HttpContext.Current.User.Identity.Name);
            _dictionaryRepositoryInstance = new DictionaryRepository();
            _notificationRepositoryInstance = new NotificationRepository();
        }

        #endregion

        #region Override Methods

        public override void OnViewLoaded()
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "NotificationSetupPresenter", "OnViewLoaded() is invoked.");
        }

        public override void OnViewInitialized()
        {            
            SetFieldsBlank();
        }

        private void SetFieldsBlank()
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPARLevelPresenter", "SetFieldsBlank() is invoked.");
            PopulatePageSize();
            View.CustomerList = _customerRepositoryInstance.FetchAllCustomer(false, HttpContext.Current.User.Identity.Name);
            FetchAllNotificationType();
            View.Time = "09:00";
            View.ConfigurationSetting = "7";
            View.NotificationDetailList = null;
        }

        #endregion

        private void PopulatePageSize()
        {
            View.PageSize = this._configurationRepositoryInstance.GetGridPageSize();
        }

        public void FetchAllNotificationDetailByAccountNumber()
        {
            View.NotificationDetailList = _notificationDetailRepositoryInstance.FetchAllNotificationDetailByAccountNumber(View.SelectedAccountNumber, View.SelectedNotificationType);
        }

        public void FetchSpecialistRepForCustomer()
        {
            CustomerUser customerUser = _customerRepositoryInstance.FetchUserForCustomer("SPECIALISTREP", View.SelectedAccountNumber);
            if (customerUser != null)
            {
                View.ReceiverEmailId = customerUser.EmailID;
                View.ReceiverName = customerUser.UserName;
            }
        }

        public void FetchManagerForCustomer()
        {
            CustomerUser customerUser = _customerRepositoryInstance.FetchUserForCustomer("MANAGER", View.SelectedAccountNumber);
            if (customerUser != null)
            {
                View.ReceiverEmailId = customerUser.EmailID;
                View.ReceiverName = customerUser.UserName;
            }
        }

        public void FetchSalesRepresentativeForCustomer()
        {
            CustomerUser customerUser = _customerRepositoryInstance.FetchUserForCustomer("SALESREPRESENTATIVE", View.SelectedAccountNumber);
            if (customerUser != null)
            {
                View.ReceiverEmailId = customerUser.EmailID;
                View.ReceiverName = customerUser.UserName;
            }
        }

        public bool Save()
        {
            var notificationDetail = new NotificationDetail
            {
                AccountNumber = View.SelectedAccountNumber,
                ConfigurationSetting = View.ConfigurationSetting,
                NotificationType = View.SelectedNotificationType,
                ReceiverEmailID = View.ReceiverEmailId,
                ReceiverName = View.ReceiverName,
                ReceiverType = View.ReceiverType,
                TimeToSendEmail = View.Time,
                UpdatedBy = HttpContext.Current.User.Identity.Name,
                UTCDifferenceInMins = 0
            };

            if (System.Web.HttpContext.Current.Session["TimeZoneOffset"] != null)
            {
                notificationDetail.UTCDifferenceInMins = 0 - Convert.ToInt32(System.Web.HttpContext.Current.Session["TimeZoneOffset"]);
            }
            return _notificationDetailRepositoryInstance.SaveNotificationDetail(notificationDetail);
        }

        public bool Update(Int64 notificationDetailId, string configurationSetting, string timeToSendEmail)
        {
            var uTcDifferenceInMins = 0;
            if (HttpContext.Current.Session["TimeZoneOffset"] != null)
            {
                uTcDifferenceInMins = 0 - Convert.ToInt32(System.Web.HttpContext.Current.Session["TimeZoneOffset"]);
            }
            return _notificationDetailRepositoryInstance.UpdateNotificationDetailByNotificationDetailId(notificationDetailId, configurationSetting, timeToSendEmail, uTcDifferenceInMins);
        }

        public NotificationDetail FetchNotificationDetailByKey(Int64 notificationDetailId)
        {
            var notificationDetail = _notificationDetailRepositoryInstance.FetchNotificationDetailByKey(notificationDetailId);
            if (notificationDetail != null)
            {
                View.ReceiverName = notificationDetail.ReceiverName;
                View.ReceiverType = notificationDetail.ReceiverType;
                View.ReceiverEmailId = notificationDetail.ReceiverEmailID;
                View.Time = notificationDetail.TimeToSendEmail;
                View.ConfigurationSetting = Convert.ToString(notificationDetail.ConfigurationSetting);
            }
            return notificationDetail;
        }

        public bool Delete(Int64 notificationDetailId)
        {
            return _notificationDetailRepositoryInstance.Delete(notificationDetailId);
        }
        
        private void FetchAllNotificationType()
        {
            View.ListNotification = _notificationRepositoryInstance.FetchAllNotificationType();
        }

        public string FetchDictionaryKeyValue(string key)
        {
            string keyValue = string.Empty;
            Dictionary dictionary = _dictionaryRepositoryInstance.GetDictionaryRule(key);
            if (dictionary != null)            
                keyValue = dictionary.KeyValue;            
            return keyValue;
        }
    }
}
