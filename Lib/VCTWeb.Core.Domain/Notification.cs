using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	Notification  	
    /// Purpose		:	Domain class for table Notification  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	Jan 13 2016 12:04PM  		
    /// </summary>	
    
    public class Notification
    {
        #region "Instance variables"

        private string _notificationType;
        private string _description;
        private string _labelText;
        private string _textBoxDefaultValue;        
        private bool _showDays;
        private bool _isTextBoxEditable;
        private bool _isTimerEditable;
        private string _configurationSettingHeader;

        #endregion

        #region "ctors"

        public Notification()
        {
            //write constructor logic here
        }
        #endregion


        #region "public Properties"


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

        public string ConfigurationSettingHeader
        {
            get
            {
                return _configurationSettingHeader;
            }
            set
            {
                if (_configurationSettingHeader != value)
                {
                    _configurationSettingHeader = value;
                }
            }
        }
        

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;

                }
            }
        }

        public string LabelText
        {
            get
            {
                return _labelText;
            }
            set
            {
                if (_labelText != value)
                {
                    _labelText = value;

                }
            }
        }

        public string TextBoxDefaultValue
        {
            get
            {
                return _textBoxDefaultValue;
            }
            set
            {
                if (_textBoxDefaultValue != value)
                {
                    _textBoxDefaultValue = value;
                }
            }
        }


        public bool ShowDays
        {
            get
            {
                return _showDays;
            }
            set
            {
                if (_showDays != value)
                {
                    _showDays = value;

                }
            }
        }

        public bool IsTextBoxEditable
        {
            get
            {
                return _isTextBoxEditable;
            }
            set
            {
                if (_isTextBoxEditable != value)
                {
                    _isTextBoxEditable = value;
                }
            }
        }

        public bool IsTimerEditable
        {
            get
            {
                return _isTimerEditable;
            }
            set
            {
                if (_isTimerEditable != value)
                {
                    _isTimerEditable = value;
                }
            }
        }
        
        

        #endregion
    }
}
