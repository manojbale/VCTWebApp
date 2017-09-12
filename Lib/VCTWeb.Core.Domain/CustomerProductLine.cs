using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	CustomerProductLine  	
    /// Purpose		:	Domain class for table CustomerProductLine  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	May 11 2015  5:18PM  		
    
    [Serializable]
    public class CustomerProductLine
    {
        #region "Instance variables"

        private string _accountNumber;
        private string _productLineName;
        private string _productLineDesc;
        private string _updatedBy;
        private DateTime _updatedOn;
        private bool _selected;

        private string _customerName;
        private string _nameAccount;

        #endregion

        #region "ctors"

        public CustomerProductLine()
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

        public string ProductLineName
        {
            get
            {
                return _productLineName;
            }
            set
            {
                if (_productLineName != value)
                {
                    _productLineName = value;

                }
            }
        }

        public string ProductLineDesc
        {
            get
            {
                return _productLineDesc;
            }
            set
            {
                if (_productLineDesc != value)
                {
                    _productLineDesc = value;

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
        
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                }
            }
        }

        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;

                }
            }
        }

        public string NameAccount
        {
            get
            {
                return _nameAccount;
            }
            set
            {
                if (_nameAccount != value)
                {
                    _nameAccount = value;

                }
            }
        }

        #endregion
    }

}






