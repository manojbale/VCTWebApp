using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	ManualConsumption  	
    /// Purpose		:	Domain class for table ManualConsumption  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	May 14 2015  3:30PM  		
    /// </sum
    public class ManualConsumption
    {
        #region "Instance variables"

        private string _accountNumber;
        private string _tagId;
        private bool _isActive;
        private string _updatedBy;
        private DateTime _updatedOn;

        #endregion

        #region "ctors"

        public ManualConsumption()
        {
            //write constructor logic here
        }
        #endregion


        #region "public Properties"


        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                if (_accountNumber != value)
                {
                    _accountNumber = value;

                }
            }
        }

        public string TagId
        {
            get
            {
                return _tagId;
            }
            set
            {
                if (_tagId != value)
                {
                    _tagId = value;

                }
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

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






