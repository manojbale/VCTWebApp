
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// This class represents the business object for Dictionary record
    /// </summary>
    [Serializable]
    public class Configuration : BaseEntityClass
    {
        #region Instance Variables

        private string _keyName;
        private string _keyValue;
        private string _dataType;
        private bool _editable;
        private string _keyGroup;
        private string _description = string.Empty;


        #endregion

        #region Public Properties

        public string KeyName
        {
            get { return _keyName; }
            set
            {
                if (_keyName != value)
                {
                    _keyName = value;
                    this.IsModified = true;
                }
            }
        }

        public string KeyValue
        {
            get { return _keyValue; }
            set
            {
                if (_keyValue != value)
                {
                    _keyValue = value;
                    this.IsModified = true;
                }
            }
        }

        public string DataType
        {
            get { return _dataType; }
            set
            {
                if (_dataType != value)
                {
                    _dataType = value;
                    this.IsModified = true;
                }
            }
        }

        public bool Editable
        {
            get { return _editable; }
            set
            {
                if (_editable != value)
                {
                    _editable = value;
                    this.IsModified = true;
                }
            }
        }

        public string KeyGroup
        {
            get { return _keyGroup; }
            set
            {
                if (_keyGroup != value)
                {
                    _keyGroup = value;
                    this.IsModified = true;
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    this.IsModified = true;
                }
            }
        }

        public string ListValues { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            bool valid = true;
            if (!string.IsNullOrEmpty(_keyValue))
            {
                switch (_dataType.Trim().ToLower())
                {
                    case "int":
                    case "integer":
                        int intValue;
                        valid = int.TryParse(_keyValue, out intValue);
                        break;
                    case "bool":
                    case "boolean":
                        bool boolValue;
                        valid = bool.TryParse(_keyValue, out boolValue);
                        break;
                    case "float":
                        float floatValue;
                        valid = float.TryParse(_keyValue, out floatValue);
                        break;
                    case "decimal":
                        decimal decValue;
                        valid = decimal.TryParse(_keyValue, out decValue);
                        break;
                    default: //Always assumed as string
                        valid = true;
                        break;
                }
            }

            return valid;
        }

        #endregion
    }
}
