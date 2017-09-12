using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	CustomerShelfProperty  	
    /// Purpose		:	Domain class for table CustomerShelfProperty  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	Dec 28 2015 11:56AM  		
    [Serializable]
    public class CustomerShelfProperty
    {
        #region "Instance variables"

        private int _customerShelfId;
        private int _readerPropertyId;
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


        #region "public Properties"
        
        public int CustomerShelfId
        {
            get
            {
                return _customerShelfId;
            }
            set
            {
                if (_customerShelfId != value)
                {
                    _customerShelfId = value;

                }
            }
        }

        public int ReaderPropertyId
        {
            get
            {
                return _readerPropertyId;
            }
            set
            {
                if (_readerPropertyId != value)
                {
                    _readerPropertyId = value;

                }
            }
        }

        public string PropertyName
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

        public string PropertyDescription
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

        public string PropertyValue
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

        public DateTime? LastUpdatedOn
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

        public string ModifiedPropertyValue
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

        public bool HasModified
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

        public DateTime? ModifiedOn
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

        public string DataType
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

        public int MaximumLength
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

        public string ListValues
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

        public bool IsEditable
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






