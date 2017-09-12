using System;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	CustomerShelf  	
    /// Purpose		:	Domain class for table CustomerShelf  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	Dec 28 2015 11:35AM  		
    /// </summary>	
    [Serializable]
    public class CustomerShelf
    {
        #region "Instance variables"

        private int _customerShelfId;
        private string _accountNumber;
        private string _customerName;

        private string _shelfCode;
        private string _shelfName;
        private string _readerIP;
        private string _readerName;
        private string _readerPort;

        private string _readerStatus;
        private DateTime? _readerHealthLastUpdatedOn;

        #endregion

        #region "ctors"

        public CustomerShelf()
        {
            //write constructor logic here
        }
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

        public string ShelfCode
        {
            get
            {
                return _shelfCode;
            }
            set
            {
                if (_shelfCode != value)
                {
                    _shelfCode = value;

                }
            }
        }

        public string ShelfName
        {
            get
            {
                return _shelfName;
            }
            set
            {
                if (_shelfName != value)
                {
                    _shelfName = value;

                }
            }
        }

        public string ReaderIP
        {
            get
            {
                return _readerIP;
            }
            set
            {
                if (_readerIP != value)
                {
                    _readerIP = value;

                }
            }
        }

        public string ReaderName
        {
            get
            {
                return _readerName;
            }
            set
            {
                if (_readerName != value)
                {
                    _readerName = value;

                }
            }
        }

        public string ReaderPort
        {
            get
            {
                return _readerPort;
            }
            set
            {
                if (_readerPort != value)
                {
                    _readerPort = value;

                }
            }
        }

        public string ReaderStatus
        {
            get
            {
                return _readerStatus;
            }
            set
            {
                if (_readerStatus != value)
                {
                    _readerStatus = value;

                }
            }
        }

        public DateTime? ReaderHealthLastUpdatedOn
        {
            get
            {
                return _readerHealthLastUpdatedOn;
            }
            set
            {
                if (_readerHealthLastUpdatedOn != value)
                {
                    _readerHealthLastUpdatedOn = value;

                }
            }
        }

        #endregion
    }

}






