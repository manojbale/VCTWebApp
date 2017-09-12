using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	OrderDetail  	
    /// Purpose		:	Domain class for table OrderDetail  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	May 28 2015  5:38PM  		
    /// </summary>	
    public class OrderDetail
    {
        #region "Instance variables"

        private string _customerName;
        private string _accountNumber;
        private int _orderId;
        private string _orderNumber;
        private string _lineNumber;
        private string _refNum;
        
        private Int16 _orderedQty;
        private Int16? _shippedQty;
        private Int16? _cancelledQty;

        private Int16? _receivedQty;
        private Int16? _adjustQty;
        private Int16? _remainingQty;

        private string _orderStatus;
        private DateTime _orderDate;
        private DateTime? _shippedDate;
        private string _createdBy;
        

        private Int64 _orderAdjustmentId;
        private int _dispositionTypeId;
        private string _dispositionType;
        private Int16 _qty;
        private string _remarks;
        private string _updatedBy;
        private DateTime _updatedOn;


        #endregion

        #region "ctors"

        public OrderDetail()
        {
            //write constructor logic here
        }
        #endregion


        #region "public Properties"
              
        public int OrderId
        {
            get
            {
                return _orderId;
            }
            set
            {
                if (_orderId != value)
                {
                    _orderId = value;

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

        public string OrderNumber
        {
            get
            {
                return _orderNumber;
            }
            set
            {
                if (_orderNumber != value)
                {
                    _orderNumber = value;

                }
            }
        }

        public string LineNumber
        {
            get
            {
                return _lineNumber;
            }
            set
            {
                if (_lineNumber != value)
                {
                    _lineNumber = value;

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

        public Int16 OrderedQty
        {
            get
            {
                return _orderedQty;
            }
            set
            {
                if (_orderedQty != value)
                {
                    _orderedQty = value;

                }
            }
        }

        public Int16? ShippedQty
        {
            get
            {
                return _shippedQty;
            }
            set
            {
                if (_shippedQty != value)
                {
                    _shippedQty = value;

                }
            }
        }

        public Int16? CancelledQty
        {
            get
            {
                return _cancelledQty;
            }
            set
            {
                if (_cancelledQty != value)
                {
                    _cancelledQty = value;

                }
            }
        }

        public string OrderStatus
        {
            get
            {
                return _orderStatus;
            }
            set
            {
                if (_orderStatus != value)
                {
                    _orderStatus = value;

                }
            }
        }

        public DateTime OrderDate
        {
            get
            {
                return _orderDate;
            }
            set
            {
                if (_orderDate != value)
                {
                    _orderDate = value;

                }
            }
        }

        public DateTime? ShippedDate
        {
            get
            {
                return _shippedDate;
            }
            set
            {
                if (_shippedDate != value)
                {
                    _shippedDate = value;

                }
            }
        }

        public string CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                if (_createdBy != value)
                {
                    _createdBy = value;

                }
            }
        }

        public Int16? ReceivedQty
        {
            get
            {
                return _receivedQty;
            }
            set
            {
                if (_receivedQty != value)
                {
                    _receivedQty = value;

                }
            }
        }

        public Int16? AdjustQty
        {
            get
            {
                return _adjustQty;
            }
            set
            {
                if (_adjustQty != value)
                {
                    _adjustQty = value;

                }
            }
        }

        public Int16? RemainingQty
        {
            get
            {
                return _remainingQty;
            }
            set
            {
                if (_remainingQty != value)
                {
                    _remainingQty = value;
                }
            }
        }


        public Int64 OrderAdjustmentId
        {
            get
            {
                return _orderAdjustmentId;
            }
            set
            {
                if (_orderAdjustmentId != value)
                {
                    _orderAdjustmentId = value;

                }
            }
        }    

        public int DispositionTypeId
        {
            get
            {
                return _dispositionTypeId;
            }
            set
            {
                if (_dispositionTypeId != value)
                {
                    _dispositionTypeId = value;

                }
            }
        }

        public string DispositionType
        {
            get
            {
                return _dispositionType;
            }
            set
            {
                if (_dispositionType != value)
                {
                    _dispositionType = value;
                }
            }
        }

        public Int16 Qty
        {
            get
            {
                return _qty;
            }
            set
            {
                if (_qty != value)
                {
                    _qty = value;

                }
            }
        }

        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;

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







