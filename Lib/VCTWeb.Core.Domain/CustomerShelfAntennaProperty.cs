using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	CustomerShelfAntennaProperty  	
    /// Purpose		:	Domain class for table CustomerShelfAntennaProperty  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	Dec 28 2015 
    [Serializable]
    public class CustomerShelfAntennaProperty
    {
        #region "Instance variables"

        private int _customerShelfAntennaId;
        private int _antennaPropertyId;
        private string _propertyName;
        private string _propertyDescription;
        private string _propertyValue;
        private DateTime? _lastUpdatedOn;
        private string _modifiedPropertyValue;
        private bool _hasModified;
        private DateTime? _modifiedOn;

        private string _dataType;
        private int _maximumLength;
        private string _listValues;
        private bool _isEditable;     

        #endregion

        #region "ctors"

        public CustomerShelfAntennaProperty()
        {
            //write constructor logic here
        }
        #endregion


        #region "public Properties"


        public int CustomerShelfAntennaId
        {
            get
            {
                return _customerShelfAntennaId;
            }
            set
            {
                if (_customerShelfAntennaId != value)
                {
                    _customerShelfAntennaId = value;

                }
            }
        }

        public int AntennaPropertyId
        {
            get
            {
                return _antennaPropertyId;
            }
            set
            {
                if (_antennaPropertyId != value)
                {
                    _antennaPropertyId = value;

                }
            }
        }

        public string PropertyNameAntenna
        {
            get
            {
                return _propertyName;
            }
            set
            {
                if (_propertyName != value)
                {
                    _propertyName = value;
                }
            }
        }

        public string PropertyDescriptionAntenna
        {
            get
            {
                return _propertyDescription;
            }
            set
            {
                if (_propertyDescription != value)
                {
                    _propertyDescription = value;
                }
            }
        }

        public string PropertyValueAntenna
        {
            get
            {
                return _propertyValue;
            }
            set
            {
                if (_propertyValue != value)
                {
                    _propertyValue = value;

                }
            }
        }

        public DateTime? LastUpdatedOnAntenna
        {
            get
            {
                return _lastUpdatedOn;
            }
            set
            {
                if (_lastUpdatedOn != value)
                {
                    _lastUpdatedOn = value;

                }
            }
        }

        public string ModifiedPropertyValueAntenna
        {
            get
            {
                return _modifiedPropertyValue;
            }
            set
            {
                if (_modifiedPropertyValue != value)
                {
                    _modifiedPropertyValue = value;

                }
            }
        }

        public bool HasModifiedAntenna
        {
            get
            {
                return _hasModified;
            }
            set
            {
                if (_hasModified != value)
                {
                    _hasModified = value;

                }
            }
        }

        public DateTime? ModifiedOnAntenna
        {
            get
            {
                return _modifiedOn;
            }
            set
            {
                if (_modifiedOn != value)
                {
                    _modifiedOn = value;

                }
            }
        }

        public string DataTypeAntenna
        {
            get
            {
                return _dataType;
            }
            set
            {
                if (_dataType != value)
                {
                    _dataType = value;
                }
            }
        }

        public int MaximumLengthAntenna
        {
            get
            {
                return _maximumLength;
            }
            set
            {
                if (_maximumLength != value)
                {
                    _maximumLength = value;

                }
            }
        }

        public string ListValuesAntenna
        {
            get
            {
                return _listValues;
            }
            set
            {
                if (_listValues != value)
                {
                    _listValues = value;
                }
            }
        }

        public bool IsEditableAntenna
        {
            get
            {
                return _isEditable;
            }
            set
            {
                if (_isEditable != value)
                {
                    _isEditable = value;
                }
            }
        }
        
        #endregion
    }

}






