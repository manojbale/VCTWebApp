using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	CustomerPARLevel  	
    /// Purpose		:	Domain class for table CustomerPARLevel  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	May 11 2015  8:51PM  		
    /// </summary>
    [Serializable]
    public class CustomerPARLevel
    {
        #region "Instance variables"

        private string _accountNumber;
        private string _refNum;
        private string _description;
        private Int16 _pARLevelQty;
        private string _updatedBy;
        private DateTime _updatedOn;

        #endregion

        #region "ctors"

        public CustomerPARLevel()
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

        public string RefNum
        {
            get
            {
                return _refNum;
            }
            set
            {
                if (_refNum != value)
                {
                    _refNum = value;

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

        public Int16 PARLevelQty
        {
            get
            {
                return _pARLevelQty;
            }
            set
            {
                if (_pARLevelQty != value)
                {
                    _pARLevelQty = value;

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






