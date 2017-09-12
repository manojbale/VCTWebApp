using System;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	NotificationDetail  	
    /// Purpose		:	Domain class for table NotificationDetail  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	Jun 11 2015  3:58PM  		
    /// </summary>

    [Serializable]
    public class NotificationDetail
    {
        #region "Instance variables"

        private Int64 _notificationDetailId;
        private string _accountNumber;
        private string _notificationType;
        private string _receiverType;
        private string _receiverName;
        private string _receiverEmailId;
        private string _timeToSendEmail;
        private string _configurationSetting;
        private int _utcDifferenceInMins;
        private DateTime? _lastEmailSent;
        private string _updatedBy;
        private DateTime _updatedOn;

        #endregion

        #region "public Properties"

        public Int64 NotificationDetailId
        {
            get
            {
                return _notificationDetailId;
            }
            set
            {
                _notificationDetailId = value;
            }
        }

        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                _accountNumber = value;
            }
        }

        public string NotificationType
        {
            get
            {
                return _notificationType;
            }
            set
            {
                if (_notificationType != value)
                {
                    _notificationType = value;

                }
            }
        }

        public string ReceiverType
        {
            get
            {
                return _receiverType;
            }
            set
            {
                if (_receiverType != value)
                {
                    _receiverType = value;

                }
            }
        }

        public string ReceiverName
        {
            get
            {
                return _receiverName;
            }
            set
            {
                if (_receiverName != value)
                {
                    _receiverName = value;

                }
            }
        }

        public string ReceiverEmailID
        {
            get
            {
                return _receiverEmailId;
            }
            set
            {
                if (_receiverEmailId != value)
                {
                    _receiverEmailId = value;

                }
            }
        }

        public string TimeToSendEmail
        {
            get
            {
                return _timeToSendEmail;
            }
            set
            {
                if (_timeToSendEmail != value)
                {
                    _timeToSendEmail = value;

                }
            }
        }

        public string ConfigurationSetting
        {
            get
            {
                return _configurationSetting;
            }
            set
            {
                if (_configurationSetting != value)
                {
                    _configurationSetting = value;
                }
            }
        }

        public int UTCDifferenceInMins
        {
            get
            {
                return _utcDifferenceInMins;
            }
            set
            {
                if (_utcDifferenceInMins != value)
                {
                    _utcDifferenceInMins = value;
                }
            }
        }

        public DateTime? LastEmailSent
        {
            get
            {
                return _lastEmailSent;
            }
            set
            {
                if (_lastEmailSent != value)
                {
                    _lastEmailSent = value;

                }
            }
        }

        public string UpdatedBy
        {
            get
            {
                return _updatedBy;
            }
            set
            {
                if (_updatedBy != value)
                {
                    _updatedBy = value;

                }
            }
        }

        public DateTime UpdatedOn
        {
            get
            {
                return _updatedOn;
            }
            set
            {
                if (_updatedOn != value)
                {
                    _updatedOn = value;

                }
            }
        }

        #endregion
    }

}






